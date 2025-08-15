using System.ComponentModel.DataAnnotations;

namespace DashboardWebAPI.Models
{
    public class Note
    {
        public long Id { get; set; }
        [MaxLength(10000)]
        public string Content { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}
