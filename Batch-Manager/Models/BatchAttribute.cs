using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Batch_Manager.Models
{
    public class BatchAttribute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int AttributeId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [ForeignKey(nameof(Batch))]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Batch? Batch { get; set; }
    }
}
