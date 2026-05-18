using Hoshiko.Controller;
using Hoshiko.Models;
using Hoshiko.Models.Entity;
using Hoshiko.Models.View;
using Hoshiko.XAML;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Hoshiko
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MediaViewModel();
        }
    }
}
