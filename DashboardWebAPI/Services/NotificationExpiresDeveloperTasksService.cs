using DashboardWebAPI.Data;
using DashboardWebAPI.Models;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DashboardWebAPI.Services
{
    public interface INotificationExpiresDeveloperTasksService
    {
        public Task<string> GenerateExpirationNotificationAsync();
    }
    public class NotificationExpiresDeveloperTasksService : INotificationExpiresDeveloperTasksService
    {
        private readonly IDAL _dal;
        private readonly INotificationBuilderServcie _notificationBuilderServcie;

        public NotificationExpiresDeveloperTasksService(IDAL dal, INotificationBuilderServcie notificationBuilderServcie)
        {
            _dal = dal;
            _notificationBuilderServcie = notificationBuilderServcie;
        }

        public async Task<string> GenerateExpirationNotificationAsync()
        {
            var developerTasks = await _dal.GetDeveloperTaskDataAsync();

            var sevenDaysToExpire = GetExpiringDeveloperTasks(developerTasks, 7);
            var threeDaysToExpire = GetExpiringDeveloperTasks(developerTasks, 3);

            var notification = _notificationBuilderServcie.BuildNotification(sevenDaysToExpire, threeDaysToExpire);

            return notification;
        }

        private List<DeveloperTask> GetExpiringDeveloperTasks(List<DeveloperTask> developerTasks, int expireDays)
        {
            return developerTasks.Where(x => DateTime.TryParse(x.EndDate, out var endDate) &&
                (endDate.Date - DateTime.Now.Date).Days == expireDays).ToList();
        }
    }
}
