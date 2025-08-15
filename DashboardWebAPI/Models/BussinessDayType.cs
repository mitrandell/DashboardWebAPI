
using System.ComponentModel.DataAnnotations;

namespace DashboardWebAPI.Models
{
    public class BussinessDayType
    {
        public long Id { get; set; }
        [MaxLength(1000)]
        public required string Title { get; set; }
    }
}
