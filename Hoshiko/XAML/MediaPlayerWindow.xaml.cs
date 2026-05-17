using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Hoshiko.XAML
{
    /// <summary>
    /// Логика взаимодействия для MediaPlayerWindow.xaml
    /// </summary>
    public partial class MediaPlayerWindow : Window
    {
        private DispatcherTimer _timer;
        private bool _isPlaying;

        public MediaPlayerWindow()
        {
            InitializeComponent();

            MediaElement.MediaOpened += MediaElement_MediaOpened;
            MediaElement.MediaEnded += MediaElement_MediaEnded;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _timer.Tick += Timer_Tick;
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

        #region Обработчики кнопки и слайдеров

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MediaElement.Source == null) return;

            // Если прошли до конца — сбросить позицию
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
            // Для WPF‑Slider нет IsDragInProgress, поэтому можно
            // просто обновлять позицию без лишних проверок
            if (_isPlaying) return;  // можно убрать, если хочешь обновлять при воспроизведении

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
