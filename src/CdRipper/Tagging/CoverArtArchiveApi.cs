using System;
using System.Net;

namespace CdRipper.Tagging
{
    public interface ICoverArtArchiveApi
    {
        string GetReleasesByDiscId(string discId);
    }

    public class CoverArtArchiveApi : ICoverArtArchiveApi
    {
        private readonly Uri _serviceUri;

        public CoverArtArchiveApi(string serviceUrl)
        {
            _serviceUri = new Uri(serviceUrl);
        }

        public string GetReleasesByDiscId(string discId)
        {
            var requestUri = new Uri(_serviceUri, string.Format("/release/{0}/front", discId));

            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.UserAgent = Constants.UserAgent;
            request.AllowAutoRedirect = false;
            try
            {
                var response = (HttpWebResponse) request.GetResponse();
                return response.StatusCode == HttpStatusCode.RedirectKeepVerb ? response.Headers["Location"] : null;
            }
            catch (WebException ex)
            {
                return null;
            }
        }
    }
}