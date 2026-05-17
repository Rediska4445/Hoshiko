using Hoshiko.Controller;
using Hoshiko.Models;
using Hoshiko.XAML;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Hoshiko
{
    public partial class MainWindow : Window
    {
        public MediaController mediaController { get; set; }

        public ObservableCollection<MediaItem> Movies { get; }

        public MainWindow()
        {
            InitializeComponent();

            Movies = new ObservableCollection<MediaItem>();
            Movies.Add(new MovieItem
            {
                Title = "Test Movie",
                SourcePath = @"C:\Users\2022\Videos\NVIDIA\Jdk-19\Jdk-19 2026.05.01 - 17.56.43.01.mp4"
            });

            MoviesList.ItemsSource = Movies;
        }

        private void MoviePlayButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            if (button.DataContext is MediaItem item)
            {
                var player = new MediaPlayerWindow { Source = new Uri(item.SourcePath) };
                player.Show();
            }
        }
    }
}
