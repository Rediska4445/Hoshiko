using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Models.Entity
{
    [Table("Episodes")]
    public class EpisodeEntity
    {
        [Key]
        public int Id { get; set; }

        public int SeriesId { get; set; }

        [MaxLength(300)]
        public string Title { get; set; }

        public int EpisodeNumber { get; set; }

        [Column("FilePath")]
        [MaxLength(1000)]
        public string FilePath { get; set; }

        public DateTime UploadDate { get; set; }

        public int UploadedByUserId { get; set; }

        [ForeignKey("SeriesId")]
        public virtual SeriesEntity Series { get; set; }

        [ForeignKey("UploadedByUserId")]
        public virtual UserEntity UploadedByUser { get; set; }
    }
}
