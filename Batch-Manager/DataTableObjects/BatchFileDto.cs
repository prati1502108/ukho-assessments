using Batch_Manager.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Batch_Manager.DataTableObjects
{
    [NotMapped]
    public class BatchFileDto
    {
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string MimeType { get; set; }
        public ICollection<FileAttribute> Attributes { get; set; }
    }
}
