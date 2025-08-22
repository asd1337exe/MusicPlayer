using MusicPlayer.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicPlayer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _vm = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        public void Open_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                _vm.OpenFile();
            }
        }


        public void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                _vm.PlayPause();
            }
        }

        public void Stop_Click(object sender, RoutedEventArgs e) { 
            _vm?.Stop();
        } 
    }
}
