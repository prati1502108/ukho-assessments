using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Batch_Manager.Models
{
    public class BatchFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int FileId { get; set; } 
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string MimeType { get; set; }
        public string Hash { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [ForeignKey(nameof(Batch))]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; } 
        
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Batch? Batch { get; set; }
        public ICollection<FileAttribute> Attributes { get; set; }
    }
}
