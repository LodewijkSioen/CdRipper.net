using System;
using System.Diagnostics;
using CdRipper.Tagging;

namespace CdRipper.Encode
{
    public class EncoderSettings
    {
        public string OutputFile { get; set; }
        public TrackIdentification Track { get; set; }
        public Mp3Settings Mp3Settings { get; set; } 
    }

    public class Mp3Settings
    {
        public Mp3Settings()
        {
            Bitrate = StandardBitrates.kbs192;
            Type = BitrateType.Variable;
        }

        public int Bitrate { get; set; }
        public BitrateType Type { get; set; }

        public static Mp3Settings Default
        {
            get
            {
                return new Mp3Settings();
            }
        }

        public static class StandardBitrates
        {
            public const int kbs128 = 128;
            public const int kbs192 = 192;
            public const int kbs256 = 256;
            public const int kbs320 = 320;
        }

        public enum BitrateType
        {
            Variable,
            Constant
        }
    }

    public class LameMp3Encoder : IDisposable
    {
        private Process _lame;

        public LameMp3Encoder(EncoderSettings settings)
        {   
            _lame = new Process();
            _lame.StartInfo.FileName = @"lame.exe";
            _lame.StartInfo.UseShellExecute = false;
            _lame.StartInfo.RedirectStandardInput = true;
            _lame.StartInfo.Arguments = new LameArgumentBuilder(settings).ToString();
                
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
