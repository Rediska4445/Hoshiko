using Hoshiko.Models.Entity;
using Hoshiko.Repository.Recommend;
using Hoshiko.Repository.Recommend.Implement;
using System;
using System.Collections.Generic;

namespace Hoshiko.Controller
{
    public class RecomendController
    {
        private readonly Dictionary<Type, object> _repositories;

        public RecomendController()
        {
            _repositories = new Dictionary<Type, object>
            {
                [typeof(MovieEntity)] = new RecommendRepository<MovieEntity>(),
                [typeof(MusicEntity)] = new RecommendRepository<MusicEntity>(),
                [typeof(SeriesEntity)] = new RecommendRepository<SeriesEntity>()
            };
        }

        private IRecomendRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.TryGetValue(typeof(T), out var repoObj)
                && repoObj is IRecomendRepository<T> repo)
            {
                return repo;
            }

            throw new InvalidOperationException($"Репозиторий рекомендаций для типа {typeof(T).Name} не найден.");
        }

        public List<MovieEntity> GetMovieRecommendations(List<GenreEntity> favoriteGenres, int count) =>
            GetRepository<MovieEntity>().GetRecommendationsByGenres(favoriteGenres, count);

        public List<MusicEntity> GetMusicRecommendations(List<GenreEntity> favoriteGenres, int count) =>
            GetRepository<MusicEntity>().GetRecommendationsByGenres(favoriteGenres, count);

        public List<SeriesEntity> GetSeriesRecommendations(List<GenreEntity> favoriteGenres, int count) =>
            GetRepository<SeriesEntity>().GetRecommendationsByGenres(favoriteGenres, count);
    }
}
