using AsyncOAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BitBucket.Api
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
        private static readonly string BaseUri = "https://bitbucket.org/api/1.0/";
        
        readonly string consumerKey;
        readonly string consumerSecret;
        readonly AccessToken accessToken;
    
        public BitBucketClient(string consumerKey, string consumerSecret, AccessToken accessToken)
        {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
            this.accessToken = accessToken;
        }

        private Task<TResult> GetAsync<TResult>(HttpClient httpClient, string path, params KeyValuePair<string, object>[] parameters)
        {
            //TODO エスケープ処理
            var requestUri = new Uri(BaseUri + path + "?" + string.Join("&", parameters.Where(p => p.Value != null).Select(p => p.Key + "=" + ConvertToString(p.Value))));
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = requestUri
            };
            return SendAsync<TResult>(httpClient, request);
        }

        private Task<TResult> SendAsync<TResult>(HttpClient httpClient, HttpMethod method, string path, params KeyValuePair<string, object>[] parameters)
        {
            var request = new HttpRequestMessage()
            {
                Method = method,
                Content = new FormUrlEncodedContent(parameters.Where(p => p.Value != null).Select(p => new KeyValuePair<string, string>(p.Key, ConvertToString(p.Value)))),
                RequestUri = new Uri(BaseUri + path)
            };
            return SendAsync<TResult>(httpClient, request);
        }

        private async Task<TResult> SendAsync<TResult>(HttpClient httpClient, HttpRequestMessage request)
        {
            var res = await httpClient.SendAsync(request).ConfigureAwait(false);
            if (res.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResult>(await res.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            throw new Exception(string.Format("Failed with code {0}. Message: {1}", res.StatusCode, await res.Content.ReadAsStringAsync()));
        }

        private async Task SendAsync(HttpClient httpClient, HttpMethod httpMethod, string path, params KeyValuePair<string, object>[] parameters)
        {
            var request = new HttpRequestMessage()
            {
                Method = httpMethod,
                Content = new FormUrlEncodedContent(parameters.Where(p => p.Value != null).Select(p => new KeyValuePair<string, string>(p.Key, ConvertToString(p.Value)))),
                RequestUri = new Uri(BaseUri + path)
            };
            var res = await httpClient.SendAsync(request).ConfigureAwait(false);
            if (res.IsSuccessStatusCode)
            {
                return;
            }
            throw new Exception(string.Format("Failed with code {0}. Message: {1}", res.StatusCode, await res.Content.ReadAsStringAsync()));
        }

        private string ConvertToString(object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            string text;
            if (value is IEnumerable<int>)
            {
                text = string.Join(",", (IEnumerable<int>)value);
            }
            else if (value is DateTime)
            {
                text = ((DateTime)value).ToString();
            }
            else if (value is bool)
            {
                text = (bool)value ? "1" : "0";
            }
            else
            {
                text = value.ToString();
            }
            return text;
        }

        public async Task<Repository[]> GetRepositories()
        {
            var httpClient = OAuthUtility.CreateOAuthClient(consumerKey, consumerSecret, accessToken);

            var json = await GetAsync<Repository[]>(httpClient, "user/repositories/");
            return json;
        }
    }

    interface IUser
    {
        Task<Repository[]> GetRepositories();
    }

    interface IIssue
    {
        Task<IssuesResponse> GetIssues(string account, string repo);
    }
}
