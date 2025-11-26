using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace MusicPlayer.Services
{
    public class AudioPlayer
    {
        private WaveOutEvent _outputDevice;
        private AudioFileReader _audioFile;

        private float _volume = 0.5f;

        public float Volume
        {
            get => _volume;
            set
            {
                if (value < 0f) value = 0f;
                if (value > 1f) value = 1f;

                _volume = value;

                if (_audioFile != null)
                {
                    _audioFile.Volume = _volume;
                }
            }
        }

        public void Play(string filePath)
        {
            if (_outputDevice == null)
            {
                _outputDevice = new WaveOutEvent();
                _outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (_audioFile == null)
            {
                _audioFile = new AudioFileReader(filePath)
                {
                    Volume = _volume
                };
                _outputDevice.Init(_audioFile);
            }
            _outputDevice.Play();
        }

        public void PlayPause()
        {

            if (_outputDevice?.PlaybackState == PlaybackState.Playing)
            {
                _outputDevice.Pause();
            }
            else
            {
                _outputDevice?.Play();
            }
        }

        public void Stop()
        {
            _outputDevice?.Stop();
        }

        public static string GetTotalTime(string filePath)
        {
            using var reader = new AudioFileReader(filePath);
            return reader.TotalTime.ToString(@"mm\:ss");
        }

        public TimeSpan GetCurrentTime()
        {
            if (_audioFile != null)
            {
                return _audioFile.CurrentTime;
            }
            return TimeSpan.Zero;
        }



        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            _outputDevice.Dispose();
            _outputDevice = null;
            _audioFile.Dispose();
            _audioFile = null;
        }




        public void SetPosition(double seconds)
        {
            if (_audioFile != null)
            {
                if (seconds < 0) seconds = 0;
                if (seconds > _audioFile.TotalTime.TotalSeconds)
                    seconds = _audioFile.TotalTime.TotalSeconds;

                _audioFile.CurrentTime = TimeSpan.FromSeconds(seconds);
            }
        }

        public double GetTotalTimeSeconds()
        {
            return _audioFile?.TotalTime.TotalSeconds ?? 0;
        }

    }
}
