using System.IO;
using System.Text;
using CdRipper.Tagging;

namespace CdRipper.Encode
{
    public class LameArgumentBuilder
    {        
        private string _fileName;
        private Mp3Settings _mp3Settings;
        private TrackIdentification _track;

        public LameArgumentBuilder(EncoderSettings settings)
        {
            _track = settings.Track ?? TrackIdentification.Default;
            _fileName = settings.OutputFile ?? "track-" + Path.GetRandomFileName() + ".mp3";
            _mp3Settings = settings.Mp3Settings ?? Mp3Settings.Default;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder = BuildStartArguments(builder);
            builder = BuildMp3Setting(builder);
            builder = BuildTagInformation(builder);
            builder = BuildEndArguments(builder);
            return builder.ToString();
        }

        private StringBuilder BuildStartArguments(StringBuilder builder)
        {
            builder.Append("-r -m s ");
            return builder;
        }

        private StringBuilder BuildTagInformation(StringBuilder builder)
        {
            AddSwitch(builder, "--tt", _track.Title);
            AddSwitch(builder, "--ta", _track.Artist);
            AddSwitch(builder, "--tg", _track.Genre);
            AddSwitch(builder, "--tl", _track.Disc.Title);
            AddSwitch(builder, "--ty", _track.Disc.Year);
            AddExtraId3Tag(builder, "TPE2", _track.Disc.AlbumArtist); //http://stackoverflow.com/a/5958664/66842
            //AddSwitch("--ti", track.Disc.AlbumArtLocation);
            AddTrackNumber(builder, _track);
            return builder;
        }

        private StringBuilder BuildMp3Setting(StringBuilder builder)
        {
            if (_mp3Settings.Type == Mp3Settings.BitrateType.Variable)
            {
                AddSwitch(builder, "-V", "4"); //Variable bitrate defaults to 4
            }
            AddSwitch(builder, "-b", _mp3Settings.Bitrate.ToString("N"));
            return builder;
        }

        private StringBuilder BuildEndArguments(StringBuilder builder)
        {
            builder.Append("- "); //input is stin
            builder.AppendFormat("\"{0}\"", _fileName);
            return builder;
        }

        private void AddSwitch(StringBuilder builder, string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                builder.AppendFormat("{0} \"{1}\" ", key, value);
            }
        }

        private void AddExtraId3Tag(StringBuilder builder, string tagname, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                builder.AppendFormat("--tv {0}=\"{1}\" ", tagname, value);
            }
        }

        private void AddTrackNumber(StringBuilder builder, TrackIdentification track)
        {
            if (track.TrackNumber > 0 && track.Disc.NumberOfTracks > 0)
            {
                AddSwitch(builder, "--tn", string.Format("{0}/{1}", track.TrackNumber, track.Disc.NumberOfTracks));
            }
            else if(track.TrackNumber > 0 && track.Disc.NumberOfTracks <= 0)
            {
                AddSwitch(builder, "--tn", track.TrackNumber.ToString("N"));
            }
        }        
    }
}