using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Models.Entity
{
    [Table("Episodes")]
    public class EpisodeEntity : MediaItem
    {
        public int SeriesId { get; set; }
        public string Title { get; set; }
        public int EpisodeNumber { get; set; }
        public DateTime UploadDate { get; set; }
        public int UploadedByUserId { get; set; }

        public virtual SeriesEntity Series { get; set; }
    }
}
