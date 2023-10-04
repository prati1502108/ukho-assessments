using Batch_Manager.DataTableObjects;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Batch_Manager.Models
{
    public class Batch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        public string BatchId { get; set; }
        public string Status { get; set; }
        public string BusinessUnit { get; set; }
        public string ExpiryDate { get; set; }
        public string BatchPublishedDate { get; set; }
        public ICollection<BatchFileDto> Files { get; set; }
        public Acl Acl { get; set; }
        public ICollection<BatchAttribute> Attributes { get; set; }
    }
}
