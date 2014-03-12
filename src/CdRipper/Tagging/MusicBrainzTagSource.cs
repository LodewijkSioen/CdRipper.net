using System.Text;
using CdRipper.Rip;
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

        public IEnumerable<DiscIdentification> GetTags(TableOfContents toc)
        {
            var discId = MusicBrainzDiscIdCalculator.CalculateDiscId(toc);
            return GetTags(discId);
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
                       AlbumArtist = ComposeArtistName(release["artist-credit"]),
                       Title = (string)release["title"],
                       NumberOfTracks = (int)disc["track-count"],
                       Year = (string)release["date"]
                   };
                tag.Tracks = from t in disc["tracks"]
                    select new TrackIdentification
                    {
                        Title = (string) t["title"],
                        Artist = ComposeArtistName(release["artist-credit"]),
                        TrackNumber = (int) t["number"],
                        //Genre = (string)t[""], //MusicBrainz doesn't implement genres yet: https://musicbrainz.org/doc/General_FAQ#Why_does_MusicBrainz_not_support_genre_information.3F
                        Disc = tag
                    };

                yield return tag;
            }
        }

        private string ComposeArtistName(IEnumerable<JToken> artistCredit)
        {
            var artistBuilder = new StringBuilder();

            foreach (var artist in artistCredit)
            {
                artistBuilder
                    .Append(artist["name"])
                    .Append(artist["joinphrase"]);
            }

            return artistBuilder.ToString();
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