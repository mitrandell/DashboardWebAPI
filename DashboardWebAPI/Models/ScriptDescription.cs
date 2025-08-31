using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DashboardWebAPI.Models
{
    public class ScriptDescription
    {
        public long Id { get; set; }
        [MaxLength(20000)]
        public string? Description { get; set; }
        [JsonIgnore]
        public ScriptNote ScriptNote { get; set; }
        public int Position { get; set; }
        [JsonIgnore]
        public DateTime? CreateAt { get; set; }
    }
}
