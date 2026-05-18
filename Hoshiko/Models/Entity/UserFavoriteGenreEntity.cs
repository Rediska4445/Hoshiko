using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Models.Entity
{
    [Table("UserFavoriteGenres")]
    public class UserFavoriteGenreEntity
    {
        public int UserId { get; set; }

        public int GenreId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserEntity User { get; set; }

        [ForeignKey("GenreId")]
        public virtual GenreEntity Genre { get; set; }
    }
}
