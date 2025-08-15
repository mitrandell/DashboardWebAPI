using System.ComponentModel.DataAnnotations;

namespace DashboardWebAPI.Models
{
    public class TaskData
    {
        public long Id { get; set; } 
        public long TaskNumber { get; set; }
        [MaxLength(100)]
        public required string Status { get; set; }
        [MaxLength(1000)]
        public required string InitiatorName { get; set; }
        [MaxLength(1000)]
        public required string Title { get; set; }
        [MaxLength(1000)]
        public required string ExecutorName { get; set; }
        [MaxLength(1000)]
        public required string SystemSectionName { get; set; }
        public TimeSpan? ExecutedTime { get; set; } 
        public DateTime StartTaskDate { get; set; }
        public DateTime? EndTaskDate { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
