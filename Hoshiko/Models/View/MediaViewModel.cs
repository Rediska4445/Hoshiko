using Hoshiko.Controller;
using Hoshiko.Models.Entity;
using Hoshiko.Models.View.Command;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Hoshiko.Models.View
{
    public class MediaViewModel
    {
        private readonly MediaController _controller;

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

        private SeriesEntity _selectedSeries;
        public SeriesEntity SelectedSeries
        {
            get => _selectedSeries;
            set
            {
                _selectedSeries = value;
                LoadEpisodesForSeriesCommand.Execute(_selectedSeries);
            }
        }

        public MediaViewModel()
        {
            _controller = new MediaController();

            // === Фильмы ===
            Movies = new ObservableCollection<MovieEntity>();
            LoadAllMovies();

            PlayMovieCommand = new RelayCommand<MovieEntity>(PlayMovie);
            LoadAllMoviesCommand = new RelayCommand(LoadAllMovies);

            // === Музыка ===
            Musics = new ObservableCollection<MusicEntity>();
            LoadAllMusic();

            PlayMusicCommand = new RelayCommand<MusicEntity>(PlayMusic);
            LoadAllMusicCommand = new RelayCommand(LoadAllMusic);

            // === Сериалы ===
            Series = new ObservableCollection<SeriesEntity>();
            LoadAllSeries();

            PlaySeriesCommand = new RelayCommand<SeriesEntity>(PlaySeries);
            LoadAllSeriesCommand = new RelayCommand(LoadAllSeries);
        }

        // ========= Фильмы =========
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

        // ========= Музыка =========
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

        // ========= Сериалы =========
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
            if (series == null) return;

            var player = new Hoshiko.XAML.MediaPlayerWindow { Source = new Uri(series.SourcePath) };
            player.Show();
        }
    }
}