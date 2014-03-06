using System;
using System.Linq;
using System.Text;
using CdRipper.Rip;

namespace CdRipper.Tagging
{
    public static class MusicBrainzDiscIdCalculator
    {
        //https://musicbrainz.org/doc/Disc_ID_Calculation
        //http://osdir.com/ml/audio.musicbrainz.devel/2007-12/msg00000.html
        public static string CalculateDiscId(TableOfContents toc)
        {
            using (var sha = System.Security.Cryptography.SHA1.Create())
            {
                var firstTrackNumber = Encoding.ASCII.GetBytes(toc.Tracks.First().TrackNumber.ToString("X2"));

                var lastTrackNumber = Encoding.ASCII.GetBytes(toc.Tracks.Last().TrackNumber.ToString("X2"));

                var leadOutOffset = Encoding.ASCII.GetBytes((toc.Tracks.Last().Offset + toc.Tracks.Last().Sectors).ToString("X8"));

                var result = firstTrackNumber.Concat(lastTrackNumber).Concat(leadOutOffset);

                for (int i = 0; i < 99; i++)
                {
                    var track = toc.Tracks.ElementAtOrDefault(i);
                    var trackOffset = Encoding.ASCII.GetBytes((track == null ? 0 : track.Offset).ToString("X8"));
                    result = result.Concat(trackOffset);
                }


                var hash = sha.ComputeHash(result.ToArray());
                return Convert.ToBase64String(hash)
                    .Replace('+', '.')
                    .Replace('/', '_')
                    .Replace('=', '-');
            }
        }
    }
}