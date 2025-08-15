using System.ComponentModel.DataAnnotations;

namespace DashboardWebAPI.Models
{
    public class User
    {
        public long Id { get; set; }
        [MaxLength(1000)]
        public string Login { get; set; }
        [MaxLength(1000)]
        public string PasswordHash { get; set; }
    }
}
