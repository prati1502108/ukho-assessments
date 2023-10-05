using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Batch_Manager.Models
{
    public class User
    {
        [Key]
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int UserId { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(Acl))]
        [SwaggerSchema(ReadOnly = true)]
        [JsonIgnore]
        public int AclId { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Acl? Acl { get; set; }
        
    }
}
