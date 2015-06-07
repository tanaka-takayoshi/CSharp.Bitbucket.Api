using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using AsyncOAuth;

namespace BitBucket.Api
{
    public static class Util
    {
        public static async Task<RequestToken> AuthenticateAndContinue(string consumerKey, string consumerKeySecret, string requestTokenUrl, string authorizeUrl)
        {
            // AsyncOAuthにお願いしてパラメータを作ってもらう
            var param = OAuthUtility.BuildBasicParameters(
                consumerKey,
                consumerKeySecret,
                requestTokenUrl,
                HttpMethod.Post,
                null,
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("oauth_callback", WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString())
                });
            var authrizer = new OAuthAuthorizer(consumerKey, consumerKeySecret);
            // リクエストトークンというものを貰う
            var tokenResponse = await authrizer.GetRequestToken(requestTokenUrl, param.Concat(new[]
                {
                    new KeyValuePair<string, string>("oauth_callback", WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString())
                }));
            var requestToken = tokenResponse.Token;

            // 認証画面のURLを貰う
            var pinRequestUrl = authrizer.BuildAuthorizeUrl(authorizeUrl, requestToken);

            // WebAuthenticationBrokerを使って認証画面を出す
            WebAuthenticationBroker.AuthenticateAndContinue(
                new Uri(pinRequestUrl),
                WebAuthenticationBroker.GetCurrentApplicationCallbackUri());
            return requestToken;
        }

        public static async Task<AccessToken> ContinueAuthentication(string responseData, OAuthAuthorizer authAuthorizer, string accessTokenUrl, RequestToken requestToken)
        {
            var query = responseData.Split('?')[1];
            var parameters = new Dictionary<string, string>();
            foreach (var token in query.Split('&'))
            {
                var keyValue = token.Split('=');
                parameters[keyValue[0]] = WebUtility.UrlDecode(keyValue[1]);
            }

            // アクセストークンを取得する
            var accessTokenResponse = await authAuthorizer.GetAccessToken(accessTokenUrl, requestToken, parameters["oauth_verifier"]);
            return accessTokenResponse.Token;
        }
    }
}
