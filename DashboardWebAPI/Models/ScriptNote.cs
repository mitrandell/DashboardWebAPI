using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DashboardWebAPI.Models
{
    public class ScriptNote
    {
        public long Id { get; set; }
        [MaxLength(1000)]
        public string? Title { get; set; } 
        public List<ScriptDescription>? Descriptions { get; set; }

        [JsonIgnore]
        public DateTime? CreateAt { get; set; }
    }
}
