using System;
using System.ComponentModel.DataAnnotations;

namespace Hoshiko.Models.Entity
{
    public class MusicEntity : MediaItem
    {
        [MaxLength(300)]
        public string Title { get; set; }

        [Display(Name = "Добавлено")]
        public DateTime UploadDate { get; set; } = DateTime.Now;

        [Display(Name = "Загрузил пользователь (ID)")]
        public int UploadedByUserId { get; set; }
    }
}