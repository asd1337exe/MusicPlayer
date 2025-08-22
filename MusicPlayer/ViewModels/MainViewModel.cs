using Microsoft.Win32;
using MusicPlayer.Models;
using MusicPlayer.Services;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace MusicPlayer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly AudioPlayer _player = new();
        private Song _currentSong;

        public Song CurrentSong
        {
            get => _currentSong;
            set { _currentSong = value; OnPropertyChanged(); }
        }

        public void OpenFile()
        {
            if (_player != null)
            {
                _player.Stop();
            }
            OpenFileDialog dialog = new OpenFileDialog();
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                string path = dialog.FileName;
                CurrentSong = new Song { FilePath = path , Duration = AudioPlayer.GetDuration(path)};
                _player.Play(CurrentSong.FilePath);
            }
        }

        public void PlayPause()
        {
            _player.PlayPause();
        }

        public void Stop()
        {
            _player.Stop();
            CurrentSong = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
