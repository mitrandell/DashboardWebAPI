using DashboardWebAPI.Data;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DashboardWebAPI.Services
{
    public interface ITelegramNotificationService
    {
        Task BroadcastMessage(string message, CancellationToken cancellationToken);
    }

    public class TelegramNotificationService : ITelegramNotificationService
    {
        private readonly TelegramBotClient _botClient;
        private readonly IDAL _dal;

        public TelegramNotificationService(TelegramBotClient botClient, IDAL dal)
        {
            _botClient = botClient;
            _dal = dal;
        }

        public async Task BroadcastMessage(string message, CancellationToken cancellationToken)
        {
            try
            {
                var chatIds = await _dal.GetAllChatIdsAsync();
                if(!chatIds.Any())
                {
                    return;
                }

                var tasks = chatIds.Select(x => SendMessageToChat(x, message, cancellationToken));

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Произошла ошибка при рассылке сообщений в чаты: " + ex.Message);
            }
        }

        private async Task SendMessageToChat(long? chatId, string message, CancellationToken cancellationToken)
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
                    await _dal.DeleteChatAsync(chatId);
                }
                else
                {
                    Console.WriteLine("Произошла ошибка при отправке сообщения в чат: " + ex.Message);
                }
            }
        }
    }               

}                   
