using System;
using System.Net;

namespace CdRipper.Tagging
{
    public interface IMusicBrainzApi
    {
        ApiRespose GetReleasesByDiscId(string discId);
        ApiRespose GetRelease(string releaseId);
    }

    public class MusicBrainzApi : IMusicBrainzApi
    {
        private readonly Uri _serviceUri;

        public MusicBrainzApi(string serviceUrl)
        {
            _serviceUri = new Uri(new Uri(serviceUrl), "ws/2/");
        }

        public ApiRespose GetReleasesByDiscId(string discId)
        {
            var requestUri = new Uri(_serviceUri, string.Format("discid/{0}?fmt=json", discId));
            return GetResponseFromMusicBrainz(requestUri);
        }

        public ApiRespose GetRelease(string releaseId)
        {
            var requestUri = new Uri(_serviceUri, string.Format("release/{0}?inc=artist-credits+labels+discids+recordings&fmt=json", releaseId));
            return GetResponseFromMusicBrainz(requestUri);
        }

        private static ApiRespose GetResponseFromMusicBrainz(Uri requestUri)
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.UserAgent, Constants.UserAgent);
                    var json = client.DownloadString(requestUri);
                    return new ApiRespose(true, json);
                }
                catch (WebException webEx)
                {
                    if (webEx.Response as HttpWebResponse != null)
                    {
                        var response = ((HttpWebResponse) webEx.Response);
                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            return new ApiRespose(false, null);
                        }
                    }
                    throw;
                }
            }
        }
    }
}