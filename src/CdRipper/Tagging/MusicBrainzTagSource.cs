using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CdRipper.Tagging
{
    //http://musicbrainz.org/ws/2/discid/xvIXvh0ibMHH1NNGkT_txTh.2f4-?fmt=json
    public class MusicBrainzTagSource
    {
        private readonly IIMusicBrainzApi _api;

        public MusicBrainzTagSource(IIMusicBrainzApi musicBrainzApi)
        {
            _api = musicBrainzApi;
        }

        public IEnumerable<DiscTag> GetTags(string discId)
        {
            var json = _api.GetReleasesByDiscId(discId);

            foreach (var r in JObject.Parse(json)["releases"])
            {
                var releaseJson = _api.GetRelease((string)r["id"]);
                var release = JObject.Parse(releaseJson);
                var disc = release["media"].First(m => m["discs"].Any(d => (string)d["id"] == discId));
                var tag = new DiscTag
                   {
                       Artist = (string)release["artist-credit"][0]["name"],
                       Title = (string)release["title"],
                       NumberOfTracks = (int)disc["track-count"]
                   };
                tag.Songs = from t in disc["tracks"]
                    select new SongTag
                    {
                        Title = (string) t["title"],
                        Artist = (string) t["artist-credit"][0]["name"],
                        TrackNumber = (int) t["number"],
                        Disc = tag
                    };

                yield return tag;
            }
        }
    }

    public class DiscTag
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public IEnumerable<SongTag>  Songs { get; set; }
        public int NumberOfTracks { get; set; }
    }

    public class SongTag
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public int TrackNumber { get; set; }
        public DiscTag Disc { get; set; }
    }
}