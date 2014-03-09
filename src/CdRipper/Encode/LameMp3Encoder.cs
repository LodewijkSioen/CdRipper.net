using System;
using System.Diagnostics;
using CdRipper.Tagging;

namespace CdRipper.Encode
{
    public class EncoderSettings
    {
        public string OutputFile { get; set; }
        public SongTag Song { get; set; }
        public Mp3Settings Mp3Settings { get; set; } 
    }

    public class Id3TagInfo { }
    public class Mp3Settings { }

    public class LameMp3Encoder : IDisposable
    {
        private Process _lame;

        public LameMp3Encoder(EncoderSettings settings)
        {   
            _lame = new Process();
            _lame.StartInfo.FileName = @"lame.exe";
            _lame.StartInfo.UseShellExecute = false;
            _lame.StartInfo.RedirectStandardInput = true;
            _lame.StartInfo.Arguments = String.Format("-r -m s --tt \"{0}\" --ta \"{1}\" --tl \"{2}\" --tn \"{3}/{4}\" - \"{5}\"", settings.Song.Title, settings.Song.Artist, settings.Song.Disc.Title, settings.Song.TrackNumber, settings.Song.Disc.NumberOfTracks, settings.OutputFile);
            _lame.StartInfo.CreateNoWindow = true;
            _lame.Start();            
        }

        public void Write(byte[] buffer)
        {
            _lame.StandardInput.BaseStream.Write(buffer, 0, buffer.Length);
        }

        public void Dispose()
        {
            if (_lame != null)
            {
                _lame.StandardInput.Close();
                _lame.Dispose();
                _lame = null;
            }
        }
    }
}
