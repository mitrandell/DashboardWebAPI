
namespace DashboardWebAPI.Models
{
    public class BussinessDay
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public required BussinessDayType Type { get; set; }
    }
}
