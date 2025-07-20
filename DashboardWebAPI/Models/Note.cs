namespace DashboardWebAPI.Models
{
    public class Note
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}
