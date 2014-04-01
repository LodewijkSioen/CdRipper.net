using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CdRipper.Tagging;

namespace CdRipper.Encode
{
    public class OutputLocation
    {
        private IEnumerable<char> _illegalPathCharacters;

        public OutputLocation()
        {
            BaseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            FileNameMask = "{albumartist}\\{albumtitle}\\{tracknumber}-{title}.m3";

            _illegalPathCharacters = Path.GetInvalidFileNameChars().Where(c => c != '\\');
        }

        public string BaseDirectory { get; set; }
        public string FileNameMask { get; set; }

        public string CreateFileName(TrackIdentification track)
        {
            var replacements = new Dictionary<string, string>
            {
                {"{title}", track.Title},
                {"{artist}", track.Artist},
                {"{genre}", track.Genre},
                {"{tracknumber}", track.TrackNumber.ToString("00")},
                {"{albumartist}", track.AlbumArtist},
                {"{numberoftracks}", track.TotalNumberOfTracks.ToString("00")},
                {"{albumtitle}", track.AlbumTitle},
                {"{year}", track.Year}
            };
            foreach (var character in Path.GetInvalidFileNameChars())
            {
                if (character != Path.DirectorySeparatorChar && character != Path.AltDirectorySeparatorChar)
                {
                    replacements.Add(character.ToString(), string.Empty);
                }
            }

            var fileName = CaseInsentiveReplace(FileNameMask,  replacements);

            return Path.Combine(BaseDirectory, fileName);
        }

        public static OutputLocation Default
        {
            get
            {
                return new OutputLocation();
            }
        }

        private string CaseInsentiveReplace(string input, IEnumerable<KeyValuePair<string, string>> replaceValues)
        {
            return replaceValues.Aggregate(input, (current, replaceValue) => 
                Regex.Replace(current, Regex.Escape(replaceValue.Key), replaceValue.Value ?? string.Empty, RegexOptions.IgnoreCase));
        }
    }
}