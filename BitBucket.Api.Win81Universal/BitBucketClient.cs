using AsyncOAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BitBucket.Api.Win81Universal
{
    public class BitBucketAuthenticationHandler : DelegatingHandler
    {
        readonly string apiToken;

        public BitBucketAuthenticationHandler(string apiToken)
            : this(apiToken, new System.Net.Http.HttpClientHandler())
        { }

        public BitBucketAuthenticationHandler(string apiToken, HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            this.apiToken = apiToken;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            request.Headers.Add("X-ChatWorkToken", apiToken);
            return base.SendAsync(request, cancellationToken);
        }
    }

    public class BitBucketClient
    {
        static BitBucketClient()
        {
            
        }
    }
}
