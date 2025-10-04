using DashboardWebAPI.Data;
using DashboardWebAPI.Services;
using Telegram.Bot;

namespace DashboardWebAPI.Workers
{
    public class TelegramNotificationWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITelegramBotClientService _botClient;

        public TelegramNotificationWorker(IServiceProvider serviceProvider, ITelegramBotClientService botClient)
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
                    await Task.Delay(TimeSpan.FromSeconds(10));

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var expiresTaskService = scope.ServiceProvider.GetRequiredService<INotificationExpiresDeveloperTasksService>();

                        var message = await expiresTaskService.GenerateExpirationNotificationAsync();
                        if (!string.IsNullOrEmpty(message))
                        {
                            await _botClient.BroadcastMessage(message, stoppingToken);
                        }
                    }

                    await Task.Delay(TimeSpan.FromSeconds(30));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка: " + ex.Message);
                }
            }
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        try
        //        {
        //            await Task.Delay(TimeSpan.FromSeconds(10));

        //            using (var scope = _serviceProvider.CreateScope())
        //            {
        //                var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramNotificationService>();
        //                var expiresTaskService = scope.ServiceProvider.GetRequiredService<INotificationExpiresDeveloperTasksService>();

        //                var message = await expiresTaskService.GenerateExpirationNotificationAsync();
        //                if(!string.IsNullOrEmpty(message))
        //                {
        //                    await telegramService.BroadcastMessage(message, stoppingToken);
        //                }
        //            }

        //            await Task.Delay(TimeSpan.FromSeconds(30));
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Произошла ошибка: " + ex.Message);
        //        }
        //    }

        //}
    }
}
