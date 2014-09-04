using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CdRipper.Tagging;
using System.Net;

namespace CdRipper.Encode
{
    public class OutputLocationBuilder
    {
        private IEnumerable<char> _illegalPathCharacters;

        public OutputLocationBuilder(string baseDirectory = null, string fileNameMask = null)
        {
            BaseDirectory = baseDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            FileNameMask = fileNameMask ?? "{albumartist}\\{albumtitle}\\{tracknumber}-{title}.m3";

            _illegalPathCharacters = Path.GetInvalidFileNameChars().Where(c => c != '\\');
        }

        public string BaseDirectory { get; private set; }
        public string FileNameMask { get; private set; }

        public OutputLocation PrepareOutput(TrackIdentification track)
        {
            var fileName = CreateFileName(track);
            var directory = CreateDirectory(fileName);
            var coverFile = CreateCoverFile(directory, track);

            return new OutputLocation(fileName, coverFile);
        }

        private string CreateCoverFile(DirectoryInfo directory, TrackIdentification track)
        {
            if (track.AlbumArt == null)
            {
                return null;
            }

            var coverFile = Path.Combine(directory.ToString(), "cover.jpg");

            if (File.Exists(coverFile))
            {
                return coverFile;
            }

            using (var client = new WebClient())
            {
                client.DownloadFile(track.AlbumArt, coverFile);
            }
            
            return coverFile;
        }

        private DirectoryInfo CreateDirectory(string fileName)
        {
            var directory = new DirectoryInfo(Path.GetDirectoryName(fileName));
            if (!directory.Exists)
            {
                directory.Create();
            }
            return directory;
        }
        
        private string CreateFileName(TrackIdentification track)
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

        public static OutputLocationBuilder Default
        {
            get
            {
                return new OutputLocationBuilder();
            }
        }

        private string CaseInsentiveReplace(string input, IEnumerable<KeyValuePair<string, string>> replaceValues)
        {
            return replaceValues.Aggregate(input, (current, replaceValue) => 
                Regex.Replace(current, Regex.Escape(replaceValue.Key), replaceValue.Value ?? string.Empty, RegexOptions.IgnoreCase));
        }
    }

    public class OutputLocation
    {
        public OutputLocation(string fileName, string coverFile)
        {
            FileName = fileName;
            CoverFile = coverFile;
        }

        public string FileName { get; private set; }
        public string CoverFile { get; private set; }
    }
}