using System.ComponentModel.DataAnnotations;

namespace DashboardWebAPI.Models
{
    public class CriticalTask
    {
        public long Id { get; set; }
        [Range(0, 99999999999, ErrorMessage = "Число слишком больше или меньше нуля")]
        public long TaskNumber { get; set; }
        [MaxLength(100)]
        public string? ItsmStatus { get; set; }
        [MaxLength(100)]
        public string? RedmineSatus { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        [MaxLength(100)]
        public string? StartDate { get; set; }
        [MaxLength(100)]
        public string? EndDate { get; set; }
        [MaxLength(100)]
        public string? ActionStatus { get; set; }
        [MaxLength(1000)]
        public string? UrlToRedmineTask { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
