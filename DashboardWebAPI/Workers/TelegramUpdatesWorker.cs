using DashboardWebAPI.Data;
using DashboardWebAPI.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
namespace DashboardWebAPI.Workers
{
    public class TelegramUpdatesWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TelegramBotClient _botClient;
        private int _lastUpdateId;

        public TelegramUpdatesWorker(IServiceProvider serviceProvider, TelegramBotClient botClient)
        {
            _serviceProvider = serviceProvider;
            _botClient = botClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(40));

                    await UpdateProcess(stoppingToken);

                    await Task.Delay(TimeSpan.FromDays(1));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошбика при выполнении бэкграунд воркера по получению апдейтов с telegram api: " + ex.Message);

                    await Task.Delay(TimeSpan.FromSeconds(60));
                }
            }
        }

        private async Task UpdateProcess(CancellationToken  cancellationToken)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dal = scope.ServiceProvider.GetRequiredService<IDAL>();
                    var chatIds = await dal.GetAllChatIdsAsync();
                    var updates = await _botClient.GetUpdates(offset: _lastUpdateId, timeout: 100, cancellationToken: cancellationToken);
                
                    foreach(var update in updates)
                    {
                        var chatId = GetChatIdFromUpdate(update);

                        if (chatId.HasValue)
                        {
                            if (!chatIds.Contains(chatId.Value))
                            {
                                var chatData = new TelegramChat()
                                {
                                    BotName = await _botClient.GetMyName(),
                                    ChatId = chatId.Value
                                };

                                await dal.AddChatDataAsync(chatData);

                            }

                            _lastUpdateId += 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка в процессе получения апдейтов с telegram api: " + ex.Message);
            }
        }

        private long? GetChatIdFromUpdate(Update update)
        {
            long? chatId = update.Type switch
            {
                UpdateType.Message => update.Message?.Chat.Id,
                UpdateType.CallbackQuery => update.CallbackQuery?.Message?.Chat.Id,
                UpdateType.MyChatMember => update.MyChatMember?.Chat.Id,
                UpdateType.ChannelPost => update.ChannelPost?.Chat.Id,
                UpdateType.EditedMessage => update.EditedMessage?.Chat.Id,
                UpdateType.EditedChannelPost => update.EditedChannelPost?.Chat.Id,
                UpdateType.ChatMember => update.ChatMember?.Chat.Id,
                UpdateType.ChatJoinRequest => update.ChatJoinRequest?.Chat.Id,
                _ => null
            };

            return chatId;
        }
    }
}
