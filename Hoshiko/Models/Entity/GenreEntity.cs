using System.ComponentModel.DataAnnotations;

namespace Hoshiko.Models.Entity
{
    public class GenreEntity
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}
