using System;
using System.Collections.Generic;
using System.Linq;

namespace CdRipper.Tagging
{
    public class AlbumIdentification
    {
        private readonly List<TrackIdentification> _tracks;

        public AlbumIdentification()
        {
            _tracks = new List<TrackIdentification>();
        }

        public IEnumerable<TrackIdentification> Tracks { get { return _tracks; } }
        public string Id { get; set; }
        public string AlbumArtist { get; set; }
        public string AlbumTitle { get; set; }
        public int NumberOfTracks { get; set; }
        public string Year { get; set; }
        public Uri AlbumArt { get; set; }

        public void AddTrack(TrackIdentification track)
        {
            track.SetAlbum(this);
            _tracks.Add(track);
        }

        public static AlbumIdentification GetEmpty(int numberOfTracks)
        {
            var album =  new AlbumIdentification
            {
                Id = null,
                AlbumArtist = "Unknown Artist",
                AlbumTitle = "Unknown Album",
                NumberOfTracks = numberOfTracks
            };

            foreach (var trackNumber in Enumerable.Range(1, numberOfTracks))
            {
                album.AddTrack(TrackIdentification.GetEmpty(album, trackNumber));
            }

            return album;
        }
    }
}