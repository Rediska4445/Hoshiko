using Hoshiko.Models.Entity;
using System.Collections.Generic;

namespace Hoshiko.Repository.Genre
{
    public interface IGenreRepository : IRepository<GenreEntity>
    {
        bool AddFavoriteGenres(UserEntity user, List<GenreEntity> genres);
        bool RemoveFavoriteGenres(UserEntity user, List<GenreEntity> genres);

        List<GenreEntity> GetAllFavoriteGenres(UserEntity user);

        List <GenreEntity> GetAllGenres();
        GenreEntity GetGenreById(int id);
        List<GenreEntity> GetGenresByMediaType(string mediaTypeName);
    }
}
