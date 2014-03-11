using System.Reflection.Emit;
using System.Text;
using CdRipper.Tagging;

namespace CdRipper.Encode
{
    public class LameArgumentBuilder
    {
        private readonly StringBuilder _arguments;
        private string _fileName;

        public LameArgumentBuilder()
        {
            _arguments = new StringBuilder("-r "); //Input is raw (headerless) pcm
            _arguments.Append("-m s "); //Stereo mode
        }

        public LameArgumentBuilder AddTagInformation(TrackIdentification track)
        {
            AddSwitch("--tt", track.Title);
            AddSwitch("--ta", track.Artist);
            AddSwitch("--tg", track.Genre);
            AddSwitch("--tl", track.Disc.Title);
            AddSwitch("--ty", track.Disc.Year);
            AddExtraId3Tag("TPE2", track.Disc.AlbumArtist); //http://stackoverflow.com/a/5958664/66842
            //AddSwitch("--ti", track.Disc.AlbumArtLocation);
            AddTrackNumber(track);
            return this;
        }

        public LameArgumentBuilder AddFileName(string filename)
        {
            _fileName = filename;
            return this;
        }

        private void AddSwitch(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _arguments.AppendFormat("{0} \"{1}\" ", key, value);
            }
        }

        private void AddExtraId3Tag(string tagname, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _arguments.AppendFormat("--tv {0}=\"{1}\" ", tagname, value);
            }
        }

        private void AddTrackNumber(TrackIdentification track)
        {
            if (track.TrackNumber > 0 && track.Disc.NumberOfTracks > 0)
            {
                AddSwitch("--tn", string.Format("{0}/{1}", track.TrackNumber, track.Disc.NumberOfTracks));
            }
            else if(track.TrackNumber > 0 && track.Disc.NumberOfTracks <= 0)
            {
                AddSwitch("--tn", track.TrackNumber.ToString("N"));
            }
        }

        public override string ToString()
        {
            if (_arguments[_arguments.Length-1] != ' ')
                _arguments.Append(' ');

            _arguments.Append("- "); //input is stin
            _arguments.AppendFormat("\"{0}\"", _fileName); //output is file
            return _arguments.ToString();
        }

        public LameArgumentBuilder AddMp3Settings(Mp3Settings mp3Settings)
        {
            if (mp3Settings.Type == Mp3Settings.BitrateType.Variable)
            {
                AddSwitch("-V", "4"); //Variable bitrate defaults to 4
            }
            AddSwitch("-b", mp3Settings.Bitrate.ToString("N"));

            return this;
        }
    }
}