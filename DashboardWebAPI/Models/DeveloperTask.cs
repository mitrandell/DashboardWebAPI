namespace DashboardWebAPI.Models
{
    public class DeveloperTask
    {
        public long Id { get; set; }
        public long TaskNumber { get; set; }
        public string? ItsmStatus { get; set; }
        public string? RedmineSatus { get; set; }
        public string? Description { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? ActionStatus { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
