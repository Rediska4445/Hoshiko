using Hoshiko.Models.Entity;
using Hoshiko.Repository;
using Hoshiko.Repository.Implementaion;
using Hoshiko.Repository.Implementation;
using System;
using System.Collections.Generic;

namespace Hoshiko.Controller
{
    public class MediaController
    {
        private readonly Dictionary<Type, object> _repositories;

        public MediaController()
        {
            _repositories = new Dictionary<Type, object>
            {
                [typeof(MovieEntity)] = new MovieRepository(),
                [typeof(MusicEntity)] = new MusicRepository(),
                [typeof(SeriesEntity)] = new SeriesRepository()
            };
        }

        private IRepository<T> GetRepository<T>() where T : class, new()
        {
            if (_repositories.TryGetValue(typeof(T), out var repoObj)
                && repoObj is IRepository<T> repo)
            {
                return repo;
            }

            throw new InvalidOperationException($"Репозиторий для типа {typeof(T).Name} не найден.");
        }

        public MovieEntity GetMovie(int id) =>
            GetRepository<MovieEntity>().GetById(id);

        public List<MovieEntity> GetAllMovies() =>
            GetRepository<MovieEntity>().GetAll();

        public List<MovieEntity> SearchMovies(string query) =>
            GetRepository<MovieEntity>().Search(query);

        public bool AddMovie(MovieEntity movie) =>
            GetRepository<MovieEntity>().Add(movie) > 0;

        public bool UpdateMovie(MovieEntity movie) =>
            GetRepository<MovieEntity>().Update(movie);

        public bool DeleteMovie(int id) =>
            GetRepository<MovieEntity>().Delete(id);

        public MusicEntity GetMusic(int id) =>
            GetRepository<MusicEntity>().GetById(id);

        public List<MusicEntity> GetAllMusic() =>
            GetRepository<MusicEntity>().GetAll();

        public List<MusicEntity> SearchMusic(string query) =>
            GetRepository<MusicEntity>().Search(query);

        public bool AddMusic(MusicEntity music) =>
            GetRepository<MusicEntity>().Add(music) > 0;

        public bool UpdateMusic(MusicEntity music) =>
            GetRepository<MusicEntity>().Update(music);

        public bool DeleteMusic(int id) =>
            GetRepository<MusicEntity>().Delete(id);

        public SeriesEntity GetSeries(int id) =>
            GetRepository<SeriesEntity>().GetById(id);

        public List<SeriesEntity> GetAllSeries() =>
            GetRepository<SeriesEntity>().GetAll();

        public List<SeriesEntity> SearchSeries(string query) =>
            GetRepository<SeriesEntity>().Search(query);

        public bool AddSeries(SeriesEntity series) =>
            GetRepository<SeriesEntity>().Add(series) > 0;

        public bool UpdateSeries(SeriesEntity series) =>
            GetRepository<SeriesEntity>().Update(series);

        public bool DeleteSeries(int id) =>
            GetRepository<SeriesEntity>().Delete(id);
    }
}
