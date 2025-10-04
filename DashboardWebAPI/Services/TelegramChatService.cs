using DashboardWebAPI.Data;
using DashboardWebAPI.Models;
using DocumentFormat.OpenXml.Bibliography;
using Telegram.Bot.Types;

namespace DashboardWebAPI.Services
{
    public interface ITelegramChatService
    {
        Task AddChatAsync(Update update, string botName);
        Task DeleteInactiveChatAsync(long? chatId);
    }

    public class TelegramChatService : ITelegramChatService
    {
        private readonly IDAL _dal;

        public TelegramChatService(IDAL dal)
        {
            _dal = dal;
        }

        public async Task AddChatAsync(Update update, string botName)
        {
            try
            {
                var savedChatIds = await _dal.GetAllChatIdsAsync();

                var chatId = update.Message?.Chat.Id;

                if (chatId.HasValue)
                {
                    if (!savedChatIds.Contains(chatId.Value))
                    {
                        var chatData = new TelegramChat()
                        {
                            BotName = botName,
                            ChatId = chatId.Value
                        };

                        await _dal.AddChatDataAsync(chatData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при добавлении чата: " + ex.Message);
            }
        }

        public async Task DeleteInactiveChatAsync(long? chatId)
        {
            if (!chatId.HasValue)
            {
                throw new ArgumentException("Отсутствует обязательный аргумент: " + chatId);
            }

            try
            {
                await _dal.DeleteChatAsync(chatId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при удалении чата: " + ex.Message);
            }
        }
    }
}
