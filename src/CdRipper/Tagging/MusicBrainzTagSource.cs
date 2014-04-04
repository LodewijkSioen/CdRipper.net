using System.Text;
using CdRipper.Rip;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace CdRipper.Tagging
{    
    public interface ITagSource
    {
        IEnumerable<AlbumIdentification> GetTags(TableOfContents toc);
    }

    public class MusicBrainzTagSource : ITagSource
    {
        private readonly IIMusicBrainzApi _api;

        public MusicBrainzTagSource(IIMusicBrainzApi musicBrainzApi)
        {
            _api = musicBrainzApi;
        }

        public IEnumerable<AlbumIdentification> GetTags(TableOfContents toc)
        {
            var discId = MusicBrainzDiscIdCalculator.CalculateDiscId(toc);
            
            var response = _api.GetReleasesByDiscId(discId);

            if (!response.IsFound)
            {
                return new []{ AlbumIdentification.GetEmpty(toc.Tracks.Count())};
            }

            if (IsCdStub(response))
            {
                return new []{ ParseCdStub(response)};
            }

            return ParseReleases(response, discId);
        }

        private AlbumIdentification ParseCdStub(MusicBrainzResponse response)
        {
            var stub = JObject.Parse(response.Json);

            return new AlbumIdentification
            {
                AlbumTitle = (string)stub["title"],
                AlbumArtist = (string)stub["artist"],
                NumberOfTracks = (int?)stub["track-count"] ?? stub["tracks"].Count(),
                Tracks = stub["tracks"].Select((t, index) => 
                         new TrackIdentification
                         {
                             Artist = (string)t["artist"],
                             Title = (string)t["title"],
                             TrackNumber = index +1,
                             AlbumTitle = (string)stub["title"],
                             AlbumArtist = (string)stub["artist"],
                             TotalNumberOfTracks = (int?)stub["track-count"] ?? stub["tracks"].Count(),
                         })
            };
        }

        private IEnumerable<AlbumIdentification> ParseReleases(MusicBrainzResponse response, string discId)
        {
            foreach (var r in JObject.Parse(response.Json)["releases"])
            {
                var releaseResponse = _api.GetRelease((string)r["id"]);
                if (!releaseResponse.IsFound)
                {
                    //TODO: I should probably log this somewhere..
                    continue;
                }

                var release = JObject.Parse(releaseResponse.Json);
                var disc = release["media"].First(m => m["discs"].Any(d => (string)d["id"] == discId));
                var tag = new AlbumIdentification
                {
                    AlbumArtist = ComposeArtistName(release["artist-credit"]),
                    AlbumTitle = (string)release["title"],
                    NumberOfTracks = (int) disc["track-count"],
                    Year = (string) release["date"],
                    Tracks = from t in disc["tracks"]
                        select new TrackIdentification
                        {
                            Title = (string) t["title"],
                            Artist = ComposeArtistName(release["artist-credit"]),
                            TrackNumber = (int) t["number"],
                            //Genre = (string)t[""], //MusicBrainz doesn't implement genres yet: https://musicbrainz.org/doc/General_FAQ#Why_does_MusicBrainz_not_support_genre_information.3F
                            AlbumArtist = ComposeArtistName(release["artist-credit"]),
                            AlbumTitle = (string) release["title"],
                            TotalNumberOfTracks = (int) disc["track-count"],
                            Year = (string) release["date"]
                        }
                };

                yield return tag;
            }
        }

        private bool IsCdStub(MusicBrainzResponse response)
        {
            return !response.Json.Contains("releases");
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

    public class AlbumIdentification
    {
        public IEnumerable<TrackIdentification> Tracks { get; set; }
        public string AlbumArtist { get; set; }
        public string AlbumTitle { get; set; }
        public int NumberOfTracks { get; set; }
        public string Year { get; set; }

        public static AlbumIdentification GetEmpty(int numberOfTracks)
        {
            return new AlbumIdentification
            {
                AlbumArtist = "Unknown Artist",
                AlbumTitle = "Unknown Album",
                NumberOfTracks = numberOfTracks,
                Tracks = (from trackNum in Enumerable.Range(1, numberOfTracks)
                          select TrackIdentification.GetEmpty(trackNum, numberOfTracks)).ToList()
            };
         
        }
    }

    public class TrackIdentification
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public int TrackNumber { get; set; }
        public string Genre { get; set; }
        
        public string AlbumArtist { get; set; }
        public string AlbumTitle { get; set; }
        public string Year { get; set; }
        public int TotalNumberOfTracks { get; set; }

        public static TrackIdentification GetEmpty(int trackNumber = 0, int totalNumberOfTracks = 0)
        {
            return new TrackIdentification
            {
                Artist = "Unknown Artist",
                AlbumArtist = "Unknown Artist",
                Title = "Unknown Title",
                TrackNumber = trackNumber,
                TotalNumberOfTracks = totalNumberOfTracks
            };
        }
    }
}