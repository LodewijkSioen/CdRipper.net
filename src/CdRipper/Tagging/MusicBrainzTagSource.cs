using System;
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
        private readonly IMusicBrainzApi _api;
        private readonly ICoverArtArchiveApi _coverApi;

        public MusicBrainzTagSource(IMusicBrainzApi musicBrainzApi, ICoverArtArchiveApi coverApi)
        {
            _api = musicBrainzApi;
            _coverApi = coverApi;
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
                var stub = ParseCdStub(response);
                stub.AlbumArt = GetAlbumArt(stub.Id); 
                return new []{ stub };
            }

            var releases = ParseReleases(response, discId).ToList();
            foreach (var release in releases)
            {
                release.AlbumArt = GetAlbumArt(release.Id);
            }

            return releases;
        }

        public Uri GetAlbumArt(string id)
        {
            var response = _coverApi.GetReleasesByDiscId(id);
            return response.IsFound ? ParseAlbumArt(response) : null;
        }

        private Uri ParseAlbumArt(ApiRespose response)
        {
            var art = JObject.Parse(response.Json);
            var imageUrl = (from image in art["images"]
                            where image.Value<bool>("front")
                            select image.Value<string>("image")).FirstOrDefault();

            return imageUrl == null ? null : new Uri(imageUrl);
        }

        private AlbumIdentification ParseCdStub(ApiRespose response)
        {
            var stub = JObject.Parse(response.Json);

            var tag = new AlbumIdentification
            {
                Id = (string)stub["id"],
                AlbumTitle = (string)stub["title"],
                AlbumArtist = (string)stub["artist"],
                NumberOfTracks = (int?)stub["track-count"] ?? stub["tracks"].Count(),
            };

            for (int index = 0; index < stub["tracks"].Count(); index++)
            {
                var t = stub["tracks"][index];
                tag.AddTrack(new TrackIdentification()
                         {
                             Artist = (string)t["artist"],
                             Title = (string)t["title"],
                             TrackNumber = index +1
                         });
            }

            return tag;
        }

        private IEnumerable<AlbumIdentification> ParseReleases(ApiRespose response, string discId)
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
                    Id = (string)release["id"],
                    AlbumArtist = ComposeArtistName(release["artist-credit"]),
                    AlbumTitle = (string)release["title"],
                    NumberOfTracks = (int) disc["track-count"],
                    Year = (string) release["date"]
                };

                foreach (var t in disc["tracks"])
                {
                    tag.AddTrack(new TrackIdentification()
                        {
                            Title = (string) t["title"],
                            Artist = ComposeArtistName(release["artist-credit"]),
                            TrackNumber = (int) t["number"],
                            //Genre = (string)t[""], //MusicBrainz doesn't implement genres yet: https://musicbrainz.org/doc/General_FAQ#Why_does_MusicBrainz_not_support_genre_information.3F
                        });
                }       

                yield return tag;
            }
        }

        private bool IsCdStub(ApiRespose response)
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
}