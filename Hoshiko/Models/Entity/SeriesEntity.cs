using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hoshiko.Models.Entity
{
    public class SeriesEntity : MediaItem
    {
        [MaxLength(300)]
        public string Title { get; set; }

        [Display(Name = "Добавлено")]
        public DateTime UploadDate { get; set; } = DateTime.Now;

        [Display(Name = "Загрузил пользователь (ID)")]
        public int UploadedByUserId { get; set; }

        public List<EpisodeEntity> Episodes { get; set; } = new List<EpisodeEntity>();
    }
}
