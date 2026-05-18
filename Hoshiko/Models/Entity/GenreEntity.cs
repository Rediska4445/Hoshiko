using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hoshiko.Models.Entity
{
    [Table("Genres")]
    public class GenreEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int MediaContentId { get; set; }

        [ForeignKey("MediaContentId")]
        public virtual MediaContentEntity MediaContent { get; set; }
    }

    [Table("MediaContent")]
    public class MediaContentEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}