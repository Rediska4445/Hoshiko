using Hoshiko.Models;
using Hoshiko.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Repository.Genre
{
    public interface IGenreRepository : IRepository<GenreEntity>
    {
        bool AddFavoriteGenres(UserEntity user, List<GenreEntity> genres);
        bool RemoveFavoriteGenres(UserEntity user, List<GenreEntity> genres);

        List<GenreEntity> GetAllGenres();

        GenreEntity GetGenreById(int id);

        List<GenreEntity> GetGenresByMediaType(string mediaTypeName);
    }
}
