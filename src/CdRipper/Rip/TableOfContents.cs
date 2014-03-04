using System;
using System.Collections.Generic;
using System.Linq;

namespace CdRipper.Rip
{
    public class TableOfContents
    {
        public IList<Track> Tracks { get; private set; }

        public TableOfContents(IList<Track> tracks)
        {
            Tracks = tracks;
        }

        private static IEnumerable<Track> GetTracks(Win32Functions.CDROM_TOC toc)
        {
            for (var i = toc.FirstTrack - 1; i < toc.LastTrack; i++)
            {
                if (toc.TrackData[i].Control == 0)
                {
                    var start = GetStartSector(toc.TrackData[i]) - 150; //With the two seconds lead-in for reading the track
                    var end = GetStartSector(toc.TrackData[i + 1]) - 151;

                    yield return new Track(toc.TrackData[i].TrackNumber, start, end);    
                }
            }
        }

        private static int GetStartSector(Win32Functions.TRACK_DATA data)
        {
            return (data.Address_1 * 60 * 75 + data.Address_2 * 75 + data.Address_3);
        }

        internal static TableOfContents Create(Win32Functions.CDROM_TOC toc)
        {
            var tracks = GetTracks(toc).ToList();
            return new TableOfContents(tracks);
        }
    }
}