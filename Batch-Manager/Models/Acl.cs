using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Batch_Manager.Models
{
    public class Acl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int AclId { get; set; }
        public List<User> ReadUsers { get; set; } = new();
        public List<Group> ReadGroups { get; set; } = new();

        [ForeignKey(nameof(Batch))]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Batch? Batch { get; set; }
    }
}
