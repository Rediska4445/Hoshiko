using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Models.Entity
{
    public class EpisodeEntity
    {
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public string Title { get; set; }
        public int EpisodeNumber { get; set; }
        public string SourcePath { get; set; }
        public DateTime UploadDate { get; set; }
        public int UploadedByUserId { get; set; }

        public virtual SeriesEntity Series { get; set; }
    }
}
