using System.Net;

namespace CdRipper.Tagging
{
    public class MusicBrainzApi : IIMusicBrainzApi
    {
        private readonly string _serviceUri;

        public MusicBrainzApi(string serviceUrl)
        {
            _serviceUri = serviceUrl + "ws/2/";
        }

        public string GetReleasesByDiscId(string discId)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(string.Format("{0}discid/{1}?fmt=json", _serviceUri, discId));
            }
        }

        public string GetRelease(string releaseId)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(string.Format("{0}release/{1}?inc=artist-credits+labels+discids+recordings&fmt=json", _serviceUri, releaseId));
            }
        }
    }
}