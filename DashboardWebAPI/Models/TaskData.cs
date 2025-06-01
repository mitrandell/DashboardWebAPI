namespace DashboardWebAPI.Models
{
    public class TaskData
    {
        public long Id { get; set; } 
        public long TaskNumber { get; set; }
        public required string Status { get; set; }
        public required string InitiatorName { get; set; }
        public required string Title { get; set; }
        public required string ExecutorName { get; set; }
        public required string SystemSectionName { get; set; }
        public TimeSpan? ExecutedTime { get; set; } 
        public DateTime StartTaskDate { get; set; }
        public DateTime? EndTaskDate { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
