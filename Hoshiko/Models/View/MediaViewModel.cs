using Hoshiko.Controller;
using Hoshiko.Models.Entity;
using Hoshiko.Models.View.Command;
using Hoshiko.Repository.Genre;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Hoshiko.Models.View
{
    public class MediaViewModel : INotifyPropertyChanged
    {
        private readonly MediaController _controller;
        private readonly GenreController _genreController;
        private readonly RecomendController _recommendController;
        private readonly TvProgramsController _programsController;

        private readonly Logger logger;

        public ObservableCollection<MovieEntity> Movies { get; }
        public ICommand PlayMovieCommand { get; }
        public ICommand LoadAllMoviesCommand { get; }

        public ObservableCollection<EpisodeEntity> Episodes { get; }
        public ICommand LoadEpisodesForSeriesCommand { get; }

        public ObservableCollection<MusicEntity> Musics { get; }
        public ICommand PlayMusicCommand { get; }
        public ICommand LoadAllMusicCommand { get; }

        public ObservableCollection<SeriesEntity> Series { get; }
        public ICommand PlaySeriesCommand { get; }
        public ICommand LoadAllSeriesCommand { get; }

        public ObservableCollection<GenreEntity> Genres { get; }
        public ICommand LoadAllGenresCommand { get; }
        public ICommand AddToFavoriteCommand { get; }
        public ICommand RemoveFromFavoriteCommand { get; }

        public ObservableCollection<GenreEntity> MovieGenres { get; set; }
        public ObservableCollection<GenreEntity> SeriesGenres { get; set; }
        public ObservableCollection<GenreEntity> MusicGenres { get; set; }
        public ObservableCollection<GenreEntity> FavoriteGenres { get; set; }

        private GenreEntity _selectedMovieGenre;
        private GenreEntity _selectedSeriesGenre;
        private GenreEntity _selectedMusicGenre;
        private GenreEntity _selectedFavoriteGenre;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GenreEntity SelectedMovieGenre
        {
            get => _selectedMovieGenre;
            set
            {
                _selectedMovieGenre = value;
                OnPropertyChanged();
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();

                if (_selectedMovieGenre != null)
                {
                    logger.Info($"Клик по списку [Фильмы]: Выбран жанр '{_selectedMovieGenre.Name}' (ID: {_selectedMovieGenre.Id})");

                    SelectedSeriesGenre = null;
                    SelectedMusicGenre = null;
                    SelectedFavoriteGenre = null;
                }
            }
        }

        public GenreEntity SelectedSeriesGenre
        {
            get => _selectedSeriesGenre;
            set
            {
                _selectedSeriesGenre = value;
                OnPropertyChanged();
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();

                if (_selectedSeriesGenre != null)
                {
                    logger.Info($"Клик по списку [Сериалы]: Выбран жанр '{_selectedSeriesGenre.Name}' (ID: {_selectedSeriesGenre.Id})");

                    SelectedMovieGenre = null;
                    SelectedMusicGenre = null;
                    SelectedFavoriteGenre = null;
                }
            }
        }

        public GenreEntity SelectedMusicGenre
        {
            get => _selectedMusicGenre;
            set
            {
                _selectedMusicGenre = value;
                OnPropertyChanged();
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();

                if (_selectedMusicGenre != null)
                {
                    logger.Info($"Клик по списку [Музыка]: Выбран жанр '{_selectedMusicGenre.Name}' (ID: {_selectedMusicGenre.Id})");

                    SelectedMovieGenre = null;
                    SelectedSeriesGenre = null;
                    SelectedFavoriteGenre = null;
                }
            }
        }

        public GenreEntity SelectedFavoriteGenre
        {
            get => _selectedFavoriteGenre;
            set
            {
                _selectedFavoriteGenre = value;
                OnPropertyChanged();
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();

                if (_selectedFavoriteGenre != null)
                {
                    logger.Info($"Клик по списку [Избранное]: Выбран жанр '{_selectedFavoriteGenre.Name}' (ID: {_selectedFavoriteGenre.Id})");

                    SelectedMovieGenre = null;
                    SelectedSeriesGenre = null;
                    SelectedMusicGenre = null;
                }
            }
        }

        private ObservableCollection<MusicEntity> _recommendedMusics = new ObservableCollection<MusicEntity>();

        private ObservableCollection<MovieEntity> _recommendedMovies = new ObservableCollection<MovieEntity>();
        private MovieEntity _selectedRecommendedMovie;

        private ObservableCollection<SeriesEntity> _recommendedSeries = new ObservableCollection<SeriesEntity>();
        private SeriesEntity _selectedRecommendedSeries;

        public ObservableCollection<MusicEntity> RecommendedMusics
        {
            get => _recommendedMusics;
            set
            {
                _recommendedMusics = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MovieEntity> RecommendedMovies
        {
            get => _recommendedMovies;
            set
            {
                _recommendedMovies = value;
                OnPropertyChanged();
            }
        }

        public MovieEntity SelectedRecommendedMovie
        {
            get => _selectedRecommendedMovie;
            set
            {
                if (_selectedRecommendedMovie == value) return;
                _selectedRecommendedMovie = value;
                OnPropertyChanged();
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();

                if (_selectedRecommendedMovie != null)
                {
                    logger.Info($"Выбран фильм из рекомендаций: '{_selectedRecommendedMovie.Title}' (ID: {_selectedRecommendedMovie.Id})");
                }
            }
        }

        public ObservableCollection<SeriesEntity> RecommendedSeries
        {
            get => _recommendedSeries;
            set
            {
                _recommendedSeries = value;
                OnPropertyChanged();
            }
        }

        public SeriesEntity SelectedRecommendedSeries
        {
            get => _selectedRecommendedSeries;
            set
            {
                if (_selectedRecommendedSeries == value) return;
                _selectedRecommendedSeries = value;
                OnPropertyChanged();
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();

                if (_selectedRecommendedSeries != null)
                {
                    logger.Info($"Выбран сериал из рекомендаций: '{_selectedRecommendedSeries.Title}' (ID: {_selectedRecommendedSeries.Id})");
                }
            }
        }

        public List<TvProgramEntity> TvPrograms { get; set; }

        private DataTable _tvScheduleTable;

        public DataTable TvScheduleTable
        {
            get => _tvScheduleTable;
            set
            {
                _tvScheduleTable = value;
                OnPropertyChanged();
            }
        }

        private void LoadPivotSchedule()
        {
            List<TvProgramEntity> rawPrograms = _programsController.GetAllPrograms();

            DataTable dt = new DataTable();

            if (rawPrograms == null || rawPrograms.Count == 0)
            {
                TvScheduleTable = dt;
                return;
            }

            dt.Columns.Add("Время", typeof(string));

            var uniqueChannels = rawPrograms
                .Select(p => p.ChannelName)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            foreach (var channel in uniqueChannels)
            {
                dt.Columns.Add(channel, typeof(string));
            }

            var groupedByTime = rawPrograms
                .GroupBy(p => p.StartTime)
                .OrderBy(g => g.Key);

            foreach (var timeGroup in groupedByTime)
            {
                DataRow row = dt.NewRow();

                row["Время"] = timeGroup.Key.ToString("HH:mm");

                foreach (var program in timeGroup)
                {
                    row[program.ChannelName] = program.Title;
                }

                foreach (var channel in uniqueChannels)
                {
                    if (row[channel] == DBNull.Value)
                    {
                        row[channel] = "-";
                    }
                }

                dt.Rows.Add(row);
            }

            TvScheduleTable = dt;
        }

        public MediaViewModel()
        {
            logger = new Logger();
            _controller = new MediaController();
            _genreController = new GenreController();
            _recommendController = new RecomendController();
            _programsController = new TvProgramsController();

            TvPrograms = _programsController.GetAllPrograms();

            AddToFavoriteCommand = new RelayCommand<object>(
                execute: (param) => AddToFavoriteExecute(),
                canExecute: (param) => AddToFavoriteCanExecute()
            );

            RemoveFromFavoriteCommand = new RelayCommand<object>(
                execute: (param) => RemoveFromFavoriteExecute(),
                canExecute: (param) => RemoveFromFavoriteCanExecute()
            );

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
            LoadPivotSchedule();
            LoadAllRecommendations();
        }

        private void LoadAllRecommendations()
        {
            if (UserController.CurrentUser == null)
            {
                logger.Info("Загрузка всех рекомендаций отменена: Текущий пользователь равен null.");
                return;
            }

            try
            {
                logger.Info("=== Старт загрузки медиа-рекомендаций ===");

                var favoriteGenres = _genreController.GetAllFavoriteGenres(UserController.CurrentUser);
                logger.Info($"Найдено любимых жанров в профиле: {favoriteGenres.Count}");

                var musicRecommendations = _recommendController.GetMusicRecommendations(favoriteGenres, 10);
                RecommendedMusics.Clear();
                foreach (var track in musicRecommendations)
                {
                    RecommendedMusics.Add(track);
                }
                logger.Info($"Загружено музыкальных рекомендаций: {musicRecommendations.Count}");

                var movieRecommendations = _recommendController.GetMovieRecommendations(favoriteGenres, 10);
                RecommendedMovies.Clear();
                foreach (var movie in movieRecommendations)
                {
                    RecommendedMovies.Add(movie);
                }
                logger.Info($"Загружено рекомендаций фильмов: {movieRecommendations.Count}");

                var seriesRecommendations = _recommendController.GetSeriesRecommendations(favoriteGenres, 10);
                RecommendedSeries.Clear();
                foreach (var series in seriesRecommendations)
                {
                    RecommendedSeries.Add(series);
                }
                logger.Info($"Загружено рекомендаций сериалов: {seriesRecommendations.Count}");

                logger.Info("=== Все рекомендации успешно обновлены ===");
            }
            catch (Exception ex)
            {
                logger.Info($"КРИТИЧЕСКАЯ ОШИБКА при загрузке медиа-рекомендаций: {ex.Message}\n{ex.StackTrace}");
            }
        }


        private void AddToFavoriteExecute()
        {
            GenreEntity genreToAdd = null;

            if (SelectedMovieGenre != null) 
                genreToAdd = SelectedMovieGenre;
            else if (SelectedSeriesGenre != null)
                genreToAdd = SelectedSeriesGenre;
            else if (SelectedMusicGenre != null)
                genreToAdd = SelectedMusicGenre;

            if (genreToAdd != null)
            {
                if (!FavoriteGenres.Contains(genreToAdd))
                {
                    _genreController.AddFavoriteGenres(UserController.CurrentUser, new List<GenreEntity> 
                    { 
                        genreToAdd 
                    });

                    FavoriteGenres.Add(genreToAdd);
                    logger.Info($"Кнопка [Добавить]: Жанр '{genreToAdd.Name}' (ID: {genreToAdd.Id}) перенесен в избранное.");

                    SelectedMovieGenre = null;
                    SelectedSeriesGenre = null;
                    SelectedMusicGenre = null;

                    LoadAllRecommendations();
                }
                else
                {
                    logger.Info($"Кнопка [Добавить]: Отмена. Жанр '{genreToAdd.Name}' уже есть в избранном.");
                }
            }
        }

        private bool AddToFavoriteCanExecute()
        {
            return SelectedMovieGenre != null || SelectedSeriesGenre != null || SelectedMusicGenre != null;
        }

        private void RemoveFromFavoriteExecute()
        {
            if (SelectedFavoriteGenre != null)
            {
                logger.Info($"Кнопка [Удалить]: Жанр '{SelectedFavoriteGenre.Name}' (ID: {SelectedFavoriteGenre.Id}) удален из локального избранного.");

                _genreController.RemoveFavoriteGenres(UserController.CurrentUser, new List<GenreEntity>
                {
                    SelectedFavoriteGenre
                });

                FavoriteGenres.Remove(SelectedFavoriteGenre);

                LoadAllRecommendations();
            }
        }

        private bool RemoveFromFavoriteCanExecute()
        {
            return SelectedFavoriteGenre != null;
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