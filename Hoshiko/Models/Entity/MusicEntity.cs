using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hoshiko.Models.Entity
{
    [Table("Music")]
    public class MusicEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [Required]
        [Column("FilePath")]
        [MaxLength(1000)]
        public string SourcePath { get; set; }

        public DateTime UploadDate { get; set; }

        public int UploadedByUserId { get; set; }

        public int GenreId { get; set; }

        [ForeignKey("GenreId")]
        public virtual GenreEntity Genre { get; set; }

        [ForeignKey("UploadedByUserId")]
        public virtual UserEntity UploadedByUser { get; set; }
    }
}