using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public class Song
    {
        public string FilePath { get; set; }
        public string FileName => System.IO.Path.GetFileName(FilePath);
        public TimeSpan Duration { get; set; }

    

        

    }
}
