using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Hoshiko.XAML
{
    public class PlaylistItem
    {
        public string Title { get; set; }
        public Uri Source { get; set; }

        public PlaylistItem(string title, Uri source)
        {
            Title = title;
            Source = source;
        }
    }

    /// <summary>
    /// Логика взаимодействия для MediaPlayerWindow.xaml
    /// </summary>
    public partial class MediaPlayerWindow : Window
    {
        private DispatcherTimer _timer;
        private bool _isPlaying;

        private ObservableCollection<PlaylistItem> _playlistItems;
        public ObservableCollection<PlaylistItem> PlaylistItems
        {
            get => _playlistItems;
            set
            {
                _playlistItems = value;
                Playlist.ItemsSource = _playlistItems;
            }
        }

        private PlaylistItem _selectedItem;
        public PlaylistItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    Source = _selectedItem.Source;
                    PlayPauseButton_Click(this, null);
                }
            }
        }

        public MediaPlayerWindow()
        {
            InitializeComponent();

            PlaylistItems = new ObservableCollection<PlaylistItem>();

            DataContext = this;

            MediaElement.MediaOpened += MediaElement_MediaOpened;
            MediaElement.MediaEnded += MediaElement_MediaEnded;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _timer.Tick += Timer_Tick;
        }

        private void PlayItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is PlaylistItem item)
            {
                SelectedItem = item;
            }
        }

        #region Свойство Source

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                nameof(Source),
                typeof(Uri),
                typeof(MediaPlayerWindow),
                new PropertyMetadata(null, OnSourceChanged));

        public Uri Source
        {
            get => (Uri)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MediaPlayerWindow win)
            {
                win.MediaElement.Source = (Uri)e.NewValue;
            }
        }
        #endregion

        #region Утилиты плейлиста
        public void SetPlaylist(IEnumerable<Uri> uris)
        {
            var items = new List<PlaylistItem>();
            for (int i = 0; i < uris.Count(); i++)
            {
                var uri = uris.ElementAt(i);
                var title = System.IO.Path.GetFileName(uri.LocalPath) ?? $"Item {i + 1}";
                items.Add(new PlaylistItem(title, uri));
            }

            PlaylistItems = new ObservableCollection<PlaylistItem>(items);
        }

        private void PlayNextIfAvailable()
        {
            if (PlaylistItems == null || PlaylistItems.Count == 0 || SelectedItem == null)
                return;

            int index = PlaylistItems.IndexOf(SelectedItem);
            if (index >= 0 && index < PlaylistItems.Count - 1)
            {
                SelectedItem = PlaylistItems[index + 1];
            }
        }

        #endregion

        #region Обработчики кнопки и слайдеров

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MediaElement.Source == null) return;

            if (MediaElement.NaturalDuration.HasTimeSpan &&
                MediaElement.Position >= MediaElement.NaturalDuration.TimeSpan)
            {
                MediaElement.Position = TimeSpan.Zero;
            }

            if (_isPlaying)
            {
                MediaElement.Pause();
                _isPlaying = false;
                PlayPauseButton.Content = "Play";
                _timer.Stop();
            }
            else
            {
                MediaElement.Play();
                _isPlaying = true;
                PlayPauseButton.Content = "Pause";
                _timer.Start();
            }
        }

        private void PositionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MediaElement.NaturalDuration.HasTimeSpan)
            {
                MediaElement.Position = TimeSpan.FromSeconds(PositionSlider.Value);
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaElement.Volume = VolumeSlider.Value;
        }

        #endregion

        #region MediaElement events

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (MediaElement.NaturalDuration.HasTimeSpan)
            {
                PositionSlider.Minimum = 0;
                PositionSlider.Maximum = MediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                PositionSlider.Value = 0;
            }
            PlayPauseButton.IsEnabled = true;
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            PlayPauseButton.Content = "Play";
            PlayPauseButton.IsEnabled = true;
            _isPlaying = false;
            _timer.Stop();
            PositionSlider.Value = PositionSlider.Minimum;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (MediaElement.NaturalDuration.HasTimeSpan &&
                MediaElement.Source != null)
            {
                PositionSlider.Value = MediaElement.Position.TotalSeconds;
            }
        }

        #endregion
    }
}
