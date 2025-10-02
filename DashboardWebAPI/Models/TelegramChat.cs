namespace DashboardWebAPI.Models
{
    public class TelegramChat
    {
        public long Id { get; set; }
        public string? BotName { get; set; }
        public long? ChatId { get; set; }
        public DateTime? CreateAt { get; set; }
    }
}
