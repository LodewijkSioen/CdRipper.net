using System;
using System.Net;

namespace CdRipper.Tagging
{
    public interface IMusicBrainzApi
    {
        MusicBrainzResponse GetReleasesByDiscId(string discId);
        MusicBrainzResponse GetRelease(string releaseId);
    }

    public class MusicBrainzResponse
    {
        public bool IsFound { get; private set; }
        public string Json { get; private set; }

        public MusicBrainzResponse(bool isFound, string json)
        {
            Json = json;
            IsFound = isFound;
        }

        public override bool Equals(object obj)
        {
            var other = obj as MusicBrainzResponse;
            if (other == null) return false;

            return other.IsFound == this.IsFound && other.Json == this.Json;
        }
    }

    public class MusicBrainzApi : IMusicBrainzApi
    {
        private readonly Uri _serviceUri;

        public MusicBrainzApi(string serviceUrl)
        {
            _serviceUri = new Uri(new Uri(serviceUrl), "ws/2/");
        }

        public MusicBrainzResponse GetReleasesByDiscId(string discId)
        {
            var requestUri = new Uri(_serviceUri, string.Format("discid/{0}?fmt=json", discId));
            return GetResponseFromMusicBrainz(requestUri);
        }

        public MusicBrainzResponse GetRelease(string releaseId)
        {
            var requestUri = new Uri(_serviceUri, string.Format("release/{0}?inc=artist-credits+labels+discids+recordings&fmt=json", releaseId));
            return GetResponseFromMusicBrainz(requestUri);
        }

        private static MusicBrainzResponse GetResponseFromMusicBrainz(Uri requestUri)
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.UserAgent, "OpenHomeServer/0.1 (https://github.com/LodewijkSioen/OpenHomeServer)");
                    var json = client.DownloadString(requestUri);
                    return new MusicBrainzResponse(true, json);
                }
                catch (WebException webEx)
                {
                    if (webEx.Response as HttpWebResponse != null)
                    {
                        var response = ((HttpWebResponse) webEx.Response);
                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            return new MusicBrainzResponse(false, null);
                        }
                    }
                    throw;
                }
            }
        }
    }
}