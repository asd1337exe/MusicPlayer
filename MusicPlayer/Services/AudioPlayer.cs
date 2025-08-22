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

        public void Play (string filePath)
        {
            if (_outputDevice == null)
            {
                _outputDevice = new WaveOutEvent();
                _outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (_audioFile == null)
            {
                _audioFile = new AudioFileReader(filePath);
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

        public void Stop () {
            _outputDevice?.Stop();
        }

        public static string GetDuration(string filePath)
        {
            using var reader = new AudioFileReader(filePath);
            return reader.TotalTime.ToString(@"mm\:ss");
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            _outputDevice.Dispose();
            _outputDevice = null;
            _audioFile.Dispose();
            _audioFile = null;
        }
    }
}
