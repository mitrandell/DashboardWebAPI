using DashboardWebAPI.Data;
using DashboardWebAPI.Models;
using DocumentFormat.OpenXml.Office.CustomXsn;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DashboardWebAPI.Services
{

    public interface ITelegramBotClientService
    {
        void ListenForMessagesAsync(CancellationToken cancellationToken);
        Task BroadcastMessage(string message, CancellationToken cancellationToken);
        Task SendMessageToChat(long? chatId, string message, CancellationToken cancellationToken);
    }

    public class TelegramBotClientService : ITelegramBotClientService
    {
        private readonly TelegramBotClient _botClient;
        private readonly INotificationBuilderServcie _notificationBuilder;
        private readonly IServiceProvider _serviceProvider;

        public TelegramBotClientService(TelegramBotClient botClient, INotificationBuilderServcie notificationBuilder, IServiceProvider serviceProvider)
        {
            _botClient = botClient;
            _notificationBuilder = notificationBuilder;
            _serviceProvider = serviceProvider;
        }

        public void ListenForMessagesAsync(CancellationToken cancellationToken)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                errorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cancellationToken
            );
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
    
            try
            {
                using var scope = _serviceProvider.CreateScope();

                var updateChatService = scope.ServiceProvider.GetRequiredService<ITelegramChatService>();
                var dal = scope.ServiceProvider.GetRequiredService<IDAL>();

                await updateChatService.AddChatAsync(update, await botClient.GetMyName());

                if (update.Message is not { } message)
                {
                    return;
                }

                if (message.Text is not { } messageText)
                {
                    return;
                }

                var sendMessage = string.Empty;

                switch (update.Message.Text)
                {
                    case "/задачи":
                        var tasks = await dal.GetDeveloperTaskDataAsync();
                        if(tasks.Count == 0)
                        {
                            sendMessage = "Отсутствуют открытые задачи на разработку 🎉";
                            break;
                        }

                        sendMessage = _notificationBuilder.BuildNotification(tasks, "🔧<b>Открытые задачи на разработку:</b>");

                        break;

                    default:
                        sendMessage = """
                            <b>Доступны следующие команды:</b>
                            <b>/задачи</b> - получить список открытых задач на разработку
                            """;
                        break;
                }

                if (!string.IsNullOrEmpty(sendMessage))
                {

                    var dalService = scope.ServiceProvider.GetRequiredService<IDAL>();
                    try
                    {
                        await _botClient.SendMessage(
                            update.Message.Chat.Id, 
                            sendMessage, 
                            parseMode: ParseMode.Html);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("bot was blocked") || ex.Message.Contains("chat not found") ||
                                ex.Message.Contains("Forbidden"))
                        {
                            await dalService.DeleteChatAsync(update.Message.Chat.Id);
                        }
                        else
                        {
                            Console.WriteLine("Произошла ошибка при отправке сообщения в чат: " + ex.Message);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении изменения из telegram api: " + ex.Message);
            }
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public async Task BroadcastMessage(string message, CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();

                var chatService = scope.ServiceProvider.GetRequiredService<ITelegramChatService>();
                var dal = scope.ServiceProvider.GetRequiredService<IDAL>();

                var chatIds = await dal.GetAllChatIdsAsync();
                if (!chatIds.Any())
                {
                    return;
                }

                using var semaphor = new SemaphoreSlim(1, 1);
                var tasks = chatIds.Select(async chatId =>
                {
                    await semaphor.WaitAsync();
                    try
                    {
                        await SendMessageToChat(chatId, message, cancellationToken);
                    }
                    finally
                    {
                        semaphor.Release();
                    }
                });

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Произошла ошибка при рассылке сообщений в чаты: " + ex.Message);
            }
        }

        public async Task SendMessageToChat(long? chatId, string message, CancellationToken cancellationToken)
        {
            try
            {
                await _botClient.SendMessage(chatId: chatId, text: message, parseMode: ParseMode.Html, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("bot was blocked") || ex.Message.Contains("chat not found") ||
                        ex.Message.Contains("Forbidden"))
                {
                    using var scope = _serviceProvider.CreateScope();

                    var chatService = scope.ServiceProvider.GetRequiredService<ITelegramChatService>();

                    await chatService.DeleteInactiveChatAsync(chatId);
                }
                else
                {
                    Console.WriteLine("Произошла ошибка при отправке сообщения в чат: " + ex.Message);
                }
            }
        }
    }
}
