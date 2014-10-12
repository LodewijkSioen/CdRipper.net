using System;
using System.Net;

namespace CdRipper.Tagging
{
    public interface ICoverArtArchiveApi
    {
        ApiRespose GetReleasesByDiscId(string discId);
    }

    public class CoverArtArchiveApi : ICoverArtArchiveApi
    {
        private readonly Uri _serviceUri;

        public CoverArtArchiveApi(string serviceUrl)
        {
            _serviceUri = new Uri(serviceUrl);
        }

        public ApiRespose GetReleasesByDiscId(string discId)
        {
            var requestUri = new Uri(_serviceUri, string.Format("/release/{0}", discId));

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
                        var response = ((HttpWebResponse)webEx.Response);
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