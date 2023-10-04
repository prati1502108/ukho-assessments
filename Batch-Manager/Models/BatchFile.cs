using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

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
        
        [ForeignKey(nameof(Batch))]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; } 
        
        public virtual Batch? Batch { get; set; }
        public ICollection<FileAttribute> Attributes { get; set; }

        [AllowNull]
        public string BatchGuid { get; set; }
        [AllowNull]
        public byte[] FileContent { get; set; }

    }
}
