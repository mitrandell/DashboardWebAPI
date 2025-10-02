using DashboardWebAPI.Services;

namespace DashboardWebAPI.Workers
{
    public class TelegramNotificationWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public TelegramNotificationWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
                        var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramNotificationService>();
                        var expiresTaskService = scope.ServiceProvider.GetRequiredService<INotificationExpiresDeveloperTasksService>();

                        var message = await expiresTaskService.GenerateExpirationNotificationAsync();
                        if(!string.IsNullOrEmpty(message))
                        {
                            await telegramService.BroadcastMessage(message, stoppingToken);
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
    }
}
