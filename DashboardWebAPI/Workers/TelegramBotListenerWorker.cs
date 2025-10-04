using DashboardWebAPI.Services;
using Telegram.Bot;

namespace DashboardWebAPI.Workers
{
    public class TelegramBotListenerWorker : BackgroundService
    {
        private readonly ITelegramBotClientService _botService;

        public TelegramBotListenerWorker(ITelegramBotClientService botService)
        {
            _botService = botService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            _botService.ListenForMessagesAsync(stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

    }
}
