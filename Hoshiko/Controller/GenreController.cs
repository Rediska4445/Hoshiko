using Hoshiko.Models;
using Hoshiko.Models.Entity;
using Hoshiko.Repository.Genre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Controller
{
    public class GenreController
    {
        private readonly IGenreRepository _genreRepository;

        public GenreController()
        {
            _genreRepository = new GenreRepository();
        }

        public GenreController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public List<GenreEntity> GetAllGenres() => _genreRepository.GetAllGenres();

        public GenreEntity GetGenreById(int id) => _genreRepository.GetGenreById(id);

        public List<GenreEntity> Search(string query) => _genreRepository.Search(query);

        public List<GenreEntity> GetGenresByMediaType(string mediaTypeName)
            => _genreRepository.GetGenresByMediaType(mediaTypeName);

        public bool AddFavoriteGenres(UserEntity user, List<GenreEntity> genres)
            => ((GenreRepository)_genreRepository).AddFavoriteGenres(user, genres);

        public bool RemoveFavoriteGenres(UserEntity user, List<GenreEntity> genres)
            => ((GenreRepository)_genreRepository).RemoveFavoriteGenres(user, genres);

        public List<GenreEntity> GetAllFavoriteGenres(UserEntity currentUser) => _genreRepository.GetAllFavoriteGenres(currentUser);
    }
}
