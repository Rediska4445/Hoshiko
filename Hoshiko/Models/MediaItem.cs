using System.ComponentModel.DataAnnotations.Schema;

namespace Hoshiko.Models
{
    public class MediaItem
    {
        public int Id { get; set; }
        [Column("FilePath")]
        public string SourcePath { get; set; }
    }
}
