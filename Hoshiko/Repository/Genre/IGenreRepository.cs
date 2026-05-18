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
        List<MovieEntity> GetAllMoviesGenres();
        List<SeriesEntity> GetAllSeriesGenres();
        List<MusicEntity> GetAllMusicGenres();

        bool AddToFavorites<T>(T media) where T : MediaItem;
        bool RemoveFromFavorites<T>(T media) where T : MediaItem;

        List<MovieEntity> GetMoviesByGenre(int genreId);
        List<SeriesEntity> GetSeriesByGenre(int genreId);
        List<MusicEntity> GetTracksByGenre(int genreId);

        List<MediaItem> GetAllMediaByGenre(int genreId);

        GenreEntity GetGenreById(int id);
        List<GenreEntity> GetAllGenres();
    }
}
