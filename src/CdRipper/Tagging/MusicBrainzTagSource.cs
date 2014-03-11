using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace CdRipper.Tagging
{    
    public class MusicBrainzTagSource
    {
        private readonly IIMusicBrainzApi _api;

        public MusicBrainzTagSource(IIMusicBrainzApi musicBrainzApi)
        {
            _api = musicBrainzApi;
        }

        public IEnumerable<DiscIdentification> GetTags(string discId)
        {
            var json = _api.GetReleasesByDiscId(discId);

            foreach (var r in JObject.Parse(json)["releases"])
            {
                var releaseJson = _api.GetRelease((string)r["id"]);
                var release = JObject.Parse(releaseJson);
                var disc = release["media"].First(m => m["discs"].Any(d => (string)d["id"] == discId));
                var tag = new DiscIdentification
                   {
                       AlbumArtist = (string)release["artist-credit"][0]["name"],
                       Title = (string)release["title"],
                       NumberOfTracks = (int)disc["track-count"],
                       Year = (string)release["date"]
                   };
                tag.Tracks = from t in disc["tracks"]
                    select new TrackIdentification
                    {
                        Title = (string) t["title"],
                        Artist = (string) t["artist-credit"][0]["name"],
                        TrackNumber = (int) t["number"],
                        //Genre = (string)t[""], //MusicBrainz doesn't implement genres yet: https://musicbrainz.org/doc/General_FAQ#Why_does_MusicBrainz_not_support_genre_information.3F
                        Disc = tag
                    };

                yield return tag;
            }
        }
    }

    public class DiscIdentification
    {
        public string AlbumArtist { get; set; }
        public string Title { get; set; }
        public IEnumerable<TrackIdentification> Tracks { get; set; }
        public int NumberOfTracks { get; set; }
        public string Year { get; set; }
    }

    public class TrackIdentification
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public int TrackNumber { get; set; }
        public DiscIdentification Disc { get; set; }
        public string Genre { get; set; }
    }
}