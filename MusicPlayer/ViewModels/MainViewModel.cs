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

        private bool _isUserDragging = false;

        private readonly System.Windows.Threading.DispatcherTimer _timer;

        private double _volume;
        public double Volume
        {
            get => _volume;
            set
            {
                if (_volume != value)
                {
                    _volume = value;
                    OnPropertyChanged();
                    _player.Volume = (float)_volume;
                }
            }
        }

        public Song CurrentSong
        {
            get => _currentSong;
            set
            {
                _currentSong = value;
                OnPropertyChanged();
            }
        }

        public void OpenFile()
        {
            if (_player != null)
            {
                _player.Stop();
            }

            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Audio Files|*.mp3;*.wav;*.wma;*.aac"
            };

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string path = dialog.FileName;

                CurrentSong = new Song
                {
                    FilePath = path,
                    Duration = AudioPlayer.GetTotalTime(path)
                };

                _player.Play(CurrentSong.FilePath);

                TotalTimeSeconds = _player.GetTotalTimeSeconds();
                CurrentTimeSeconds = 0;
            }
        }

        public void PlayPause() => _player.PlayPause();

        public void Stop()
        {
            _player.Stop();
            CurrentSong = null;
            CurrentTimeSeconds = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MainViewModel()
        {
            _timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1)
            };


            _timer.Tick += (s, e) =>
            {
                if (CurrentSong != null && !_isUserDragging)
                {
                    _currentTimeSeconds = _player.GetCurrentTime().TotalSeconds;
                    OnPropertyChanged(nameof(CurrentTime));
                }
            };

            _timer.Start();

            Volume = 0.5;
        }

        private double _currentTimeSeconds;
        private double _totalTimeSeconds;


        public string CurrentTime => TimeSpan.FromSeconds(CurrentTimeSeconds).ToString(@"mm\:ss");

        public double CurrentTimeSeconds
        {
            get => _currentTimeSeconds;
            set
            {
                _currentTimeSeconds = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentTime));

                if (_isUserDragging)
                {
                    _player.SetPosition(value);
                }
            }
        }

        public double TotalTimeSeconds
        {
            get => _totalTimeSeconds;
            set
            {
                _totalTimeSeconds = value;
                OnPropertyChanged();
            }
        }

        public void SliderDragStarted()
        {
            _isUserDragging = true;
        }

        public void SliderDragCompleted()
        {
            _isUserDragging = false;
            _player.SetPosition(CurrentTimeSeconds);
        }
    }


}

