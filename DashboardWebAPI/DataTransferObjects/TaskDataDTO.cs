namespace DashboardWebAPI.DataTransferObjects
{
    public class TaskDataDTO
    {
        public long TaskNumber { get; set; }
        public required string Status { get; set; }
        public required string InitiatorName { get; set; }
        public required string Title { get; set; }
        public required string ExecutorName { get; set; }
        public required string SystemSectionName { get; set; }
        public TimeSpan? ExecutedTime { get; set; }
        public string? StartTaskDate { get; set; }
        public string? EndTaskDate { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
