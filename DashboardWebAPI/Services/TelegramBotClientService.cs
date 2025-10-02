using DashboardWebAPI.Data;
using DashboardWebAPI.Models;
using DocumentFormat.OpenXml.Office.CustomXsn;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DashboardWebAPI.Services
{

    public interface ITelegramBotListenerService
    {
        void ListenForMessagesAsync(CancellationToken cancellationToken);
    }

    public class TelegramBotListenerService : ITelegramBotListenerService
    {
        private readonly TelegramBotClient _botClient;
        private readonly INotificationBuilderServcie _notificationBuilder;
        private readonly IServiceProvider _serviceProvider;

        public TelegramBotListenerService(TelegramBotClient botClient, INotificationBuilderServcie notificationBuilder, IServiceProvider serviceProvider)
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
            if (update.Message is not { } message)
            {
                return;
            }

            if (message.Text is not { } messageText)
            {
                return;
            }

            try
            {
                using var scope = _serviceProvider.CreateScope();

                var dalService = scope.ServiceProvider.GetRequiredService<IDAL>();
                var updateChatService = scope.ServiceProvider.GetRequiredService<ITelegramChatUpdateService>();

                await updateChatService.AddChat(update, await botClient.GetMyName());

                var sendMessage = string.Empty;

                switch (update.Message.Text)
                {
                    case "/задачи":
                        var tasks = await dalService.GetDeveloperTaskDataAsync();
                        if(tasks.Count == 0)
                        {
                            sendMessage = "Отсутствуют открытые задачи на разработку 🎉";
                            break;
                        }

                        sendMessage = _notificationBuilder.BuildNotification(tasks, "🔧<b>Открытые задачи на разработку:</b>");
                        break;

                    default: break;

                }

                if(!string.IsNullOrEmpty(sendMessage))
                {
                    try
                    {
                        await _botClient.SendMessage(update.Message.Chat.Id, sendMessage, parseMode: ParseMode.Html);
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

    }
}
