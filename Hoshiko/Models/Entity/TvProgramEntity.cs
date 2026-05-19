using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Models.Entity
{
    [Table("TvPrograms")]
    public class TvProgramEntity
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("StartTime")]
        public DateTime StartTime { get; set; }

        [Required]
        [StringLength(300)]
        [Column("Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        [Column("ChannelName")] 
        public string ChannelName { get; set; }
    }
}
