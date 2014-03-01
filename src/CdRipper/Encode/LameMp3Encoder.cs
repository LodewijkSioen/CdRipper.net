using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdRipper.Encode
{
    public class EncoderSettings
    {
        public string OutputFile { get; set; }
        public Id3TagInfo TagInfo { get; set; }
        public Mp3Settings Mp3Settings { get; set; } 
    }

    public class Id3TagInfo { }
    public class Mp3Settings { }

    public class LameMp3Encoder : IDisposable
    {
        private Process Lame;

        public LameMp3Encoder(EncoderSettings settings)
        {
            string outputFileName = settings.OutputFile;
            Lame = new Process();
            Lame.StartInfo.FileName = @"lame.exe";
            Lame.StartInfo.UseShellExecute = false;
            Lame.StartInfo.RedirectStandardInput = true;
            Lame.StartInfo.Arguments = "-r -m s - \"" + outputFileName + "\"";
            Lame.StartInfo.CreateNoWindow = true;
            Lame.Start();            
        }

        public void Write(byte[] buffer)
        {
            Lame.StandardInput.BaseStream.Write(buffer, 0, buffer.Length);
        }

        public void Dispose()
        {
            if (Lame != null)
            {
                Lame.StandardInput.Close();
                Lame.Dispose();
                Lame = null;
            }
        }
    }
}
