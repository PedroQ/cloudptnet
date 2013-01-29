using CloudPTNet.Auth;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPTNet
{
    public class CloudPTClient
    {
        private const string _apiUrl = "https://publicapi.cloudpt.pt";
        private const string _apiContentUrl = "https://api-content.cloudpt.pt";
        private const string _apiOAuthUrl = "https://cloudpt.pt";
        private const string _apiVer = "1";

        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private AuthToken _authToken;

        private RestClient _apiRestClient;
        private RestClient _apiContentRestClient;
        private RestClient _apiOAuthRestClient;
        private RequestBuilder _reqBuilder;

        public AuthToken AuthenticationToken { get; set; }

        public CloudPTClient(string consumerKey, string secret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = secret;

            InitializeRestClients();
        }

        public CloudPTClient(string consumerKey, string secret, string authToken, string authSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = secret;

            AuthenticationToken = new AuthToken(authToken, authSecret);
        }

        private void InitializeRestClients()
        {
            _apiRestClient = new RestClient(_apiUrl);
            _apiContentRestClient = new RestClient(_apiContentUrl);
            _apiOAuthRestClient = new RestClient(_apiOAuthUrl);

            _reqBuilder = new RequestBuilder(_apiVer); //Convert to Singleton?

        }

        #region OAuth Stuff

        public AuthToken GetToken()
        {
            _apiOAuthRestClient.Authenticator = RestSharp.Authenticators.OAuth1Authenticator.ForRequestToken(_consumerKey, _consumerSecret, "oob");

            RestRequest request = _reqBuilder.BuildOAuthTokenRequest();


            IRestResponse response = _apiOAuthRestClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new CloudPTNetException();

            return ParseToken(response.Content);
        }

        public AuthToken GetAccessToken(AuthToken token, string pin)
        {
            _apiOAuthRestClient.Authenticator = RestSharp.Authenticators.OAuth1Authenticator.ForAccessToken(_consumerKey, _consumerSecret, token.Token, token.Secret, pin);

            RestRequest request = _reqBuilder.BuildOAuthAccessTokenRequest();


            IRestResponse response = _apiOAuthRestClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new CloudPTNetException();

            _authToken = ParseToken(response.Content);
            return _authToken;
        }

        public string GetOAuthAuthorizeUrl(AuthToken token)
        {
            return string.Format("{0}/oauth/authorize?oauth_token={1}", _apiOAuthUrl, token.Token);
        }

        private AuthToken ParseToken(string p)
        {
            AuthToken token = new AuthToken();
            foreach (string s in p.Split('&'))
            {
                var urlParam = s.Split('=');

                if (urlParam[0] == "oauth_token")
                    token.Token = urlParam[1];

                if (urlParam[0] == "oauth_token_secret")
                    token.Secret = urlParam[1];
            }

            return token;
        }

        

        #endregion

       //ToDO: Partial Classes (Sync & Async), this goes there.
    }
}
