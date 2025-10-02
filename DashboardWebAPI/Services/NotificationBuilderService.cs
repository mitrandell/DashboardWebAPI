using DashboardWebAPI.Models;
using System.Text;

namespace DashboardWebAPI.Services
{
    public interface INotificationBuilderServcie
    {
        string BuildNotification(List<DeveloperTask> tasks, string header);
        string BuildNotification(List<DeveloperTask> sevendDaysToExpire, List<DeveloperTask> threeDaysToExpire);

    }

    public class NotificationBuilderService : INotificationBuilderServcie
    {
        public string BuildNotification(List<DeveloperTask> tasks, string header)

        {
            StringBuilder sb = new StringBuilder();

            AppendSection(sb, tasks, header);

            return sb.ToString();   
        }

        public string BuildNotification(List<DeveloperTask> sevendDaysToExpire, List<DeveloperTask> threeDaysToExpire)
        {
            if(sevendDaysToExpire.Count == 0 && threeDaysToExpire.Count == 0)
            {
                return String.Empty; 
            }

            StringBuilder sb = new StringBuilder();

            AppendSection(sb, sevendDaysToExpire, "<b>⚠Через 7 дней истекают задачи:</b>");
            AppendSection(sb, threeDaysToExpire, "<b>‼Через 3 дня истекают задачи:</b>");


            return sb.ToString();
        }

        private void AppendSection(StringBuilder sb, List<DeveloperTask> tasks, string header)
        {
            if(tasks.Count > 0)
            {
                sb.AppendLine(header);
                sb.AppendLine();

                foreach (var task in tasks)
                {
                    sb.Append($"""
                        <b>Номер задачи в ITSM:</b> {task.TaskNumber}
                        <b>Описание:</b> {task.Description}
                        <b>Крайний срок:</b> {task.EndDate}
                        <b>Статус в redmine:</b> {task.RedmineSatus}
                        <b>Ссылка в redmine:</b> {task.UrlToRedmineTask}


                        """);
                }
            }
        }

    }
}
