using DashboardWebAPI.Data;
using DashboardWebAPI.Models;
using Telegram.Bot.Types;

namespace DashboardWebAPI.Services
{
    public interface ITelegramChatUpdateService
    {
        Task AddChat(Update update, string botName);
    }

    public class TelegramChatUpdateService : ITelegramChatUpdateService
    {
        private readonly IDAL _dal;

        public TelegramChatUpdateService(IDAL dal)
        {
            _dal = dal;
        }

        public async Task AddChat(Update update, string botName)
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
    }
}
