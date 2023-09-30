using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Batch_Manager.Models
{
    public class Error
    {
        [JsonIgnore]
        [Key]
        public int Id { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
    }
}
