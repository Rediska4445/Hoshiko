using Hoshiko.Controller;
using Hoshiko.Models.Entity;
using Hoshiko.Models.View.Command;
using Hoshiko.Repository.Genre;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Hoshiko.Models.View
{
    public class MediaViewModel
    {
        private readonly MediaController _controller;
        private readonly GenreController _genreController;

        private readonly Logger logger = new Logger();

        // Фильмы
        public ObservableCollection<MovieEntity> Movies { get; }
        public ICommand PlayMovieCommand { get; }
        public ICommand LoadAllMoviesCommand { get; }

        public ObservableCollection<EpisodeEntity> Episodes { get; }
        public ICommand LoadEpisodesForSeriesCommand { get; }

        // Музыка
        public ObservableCollection<MusicEntity> Musics { get; }
        public ICommand PlayMusicCommand { get; }
        public ICommand LoadAllMusicCommand { get; }

        // Сериалы
        public ObservableCollection<SeriesEntity> Series { get; }
        public ICommand PlaySeriesCommand { get; }
        public ICommand LoadAllSeriesCommand { get; }

        // Жанры
        public ObservableCollection<GenreEntity> Genres { get; }
        public ICommand LoadAllGenresCommand { get; }

        public ObservableCollection<GenreEntity> MovieGenres { get; set; }
        public ObservableCollection<GenreEntity> SeriesGenres { get; set; }
        public ObservableCollection<GenreEntity> MusicGenres { get; set; }
        public ObservableCollection<GenreEntity> FavoriteGenres { get; set; }

        public MediaViewModel()
        {
            _controller = new MediaController();
            _genreController = new GenreController();

            Movies = new ObservableCollection<MovieEntity>();
            LoadAllMovies();

            PlayMovieCommand = new RelayCommand<MovieEntity>(PlayMovie);
            LoadAllMoviesCommand = new RelayCommand(LoadAllMovies);

            Musics = new ObservableCollection<MusicEntity>();
            LoadAllMusic();

            PlayMusicCommand = new RelayCommand<MusicEntity>(PlayMusic);
            LoadAllMusicCommand = new RelayCommand(LoadAllMusic);

            Series = new ObservableCollection<SeriesEntity>();
            LoadAllSeries();

            PlaySeriesCommand = new RelayCommand<SeriesEntity>(PlaySeries);
            LoadAllSeriesCommand = new RelayCommand(LoadAllSeries);

            Genres = new ObservableCollection<GenreEntity>();
            LoadAllGenresCommand = new RelayCommand(LoadAllGenres);

            MovieGenres = new ObservableCollection<GenreEntity>();
            SeriesGenres = new ObservableCollection<GenreEntity>();
            MusicGenres = new ObservableCollection<GenreEntity>();
            FavoriteGenres = new ObservableCollection<GenreEntity>();

            LoadAllGenres();
        }

        private void LoadAllMovies()
        {
            var fromDb = _controller.GetAllMovies();
            Movies.Clear();
            foreach (var movie in fromDb)
            {
                Movies.Add(movie);
            }
        }

        private void PlayMovie(MovieEntity movie)
        {
            if (movie == null) return;

            var player = new Hoshiko.XAML.MediaPlayerWindow { Source = new Uri(movie.SourcePath) };
            player.Show();
        }

        private void LoadAllMusic()
        {
            var fromDb = _controller.GetAllMusic();
            Musics.Clear();
            foreach (var music in fromDb)
            {
                Musics.Add(music);
            }
        }

        private void PlayMusic(MusicEntity music)
        {
            logger.Info(music.SourcePath);

            if (music == null) 
                return;

            var player = new Hoshiko.XAML.MediaPlayerWindow { Source = new Uri(music.SourcePath) };
            player.Show();
        }

        private void LoadAllSeries()
        {
            var fromDb = _controller.GetAllSeries();
            Series.Clear();
            foreach (var series in fromDb)
            {
                Series.Add(series);
            }
        }

        private void PlaySeries(SeriesEntity series)
        {
            logger.Info($"shit = {series.SourcePath}");

            if (series == null)
                return;

            var playlist = series.Episodes
                                 .OrderBy(e => e.EpisodeNumber)
                                 .Select(e => new Uri(e.FilePath))
                                 .ToList();

            var player = new Hoshiko.XAML.MediaPlayerWindow();
            player.SetPlaylist(playlist);
            player.Show();
        }

        private void LoadAllGenres()
        {
            var movieGenres = _genreController.GetGenresByMediaType("Movie");
            MovieGenres.Clear();

            foreach (var g in movieGenres)
            {
                MovieGenres.Add(g);
            }

            logger.Info($"MovieGenres: Count = {movieGenres.Count}");

            var seriesGenres = _genreController.GetGenresByMediaType("Series");
            SeriesGenres.Clear();

            foreach (var g in seriesGenres)
            {
                SeriesGenres.Add(g);
            }

            logger.Info($"SeriesGenres: Count = {seriesGenres.Count}");

            var musicGenres = _genreController.GetGenresByMediaType("Music");
            MusicGenres.Clear();

            foreach (var g in musicGenres)
            {
                MusicGenres.Add(g);
            }

            logger.Info($"MusicGenres: Count = {musicGenres.Count}");

            if (UserController.CurrentUser != null)
            {
                var favoriteGenres = _genreController.GetAllFavoriteGenres(UserController.CurrentUser);
                FavoriteGenres.Clear();
                foreach (var g in favoriteGenres)
                {
                    FavoriteGenres.Add(g);
                }
                logger.Info($"FavoriteGenres: Count = {favoriteGenres.Count} для пользователя {UserController.CurrentUser.Username}");
            }
            else
            {
                logger.Info("FavoriteGenres не загружены: Текущий пользователь (CurrentUser) равен null.");
            }
        }
    }
}