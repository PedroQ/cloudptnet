using CloudPTNet.Auth;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CloudPTNet
{
    public class CloudPTClient
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private AuthToken _authToken;

        private RestClient _apiRestClient;

        private RequestBuilder _reqBuilder;

        public AuthToken AuthenticationToken
        {
            get
            {
                return _authToken;
            }
            set
            {
                _authToken = value;
                SetRestClientsAuthenticator();
            }
        }

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

            InitializeRestClients();

            AuthenticationToken = new AuthToken(authToken, authSecret);
        }

        private void InitializeRestClients()
        {
            _apiRestClient = new RestClient();
            _reqBuilder = new RequestBuilder(); //Convert to Singleton?

        }

        private void SetRestClientsAuthenticator()
        {
            _apiRestClient.Authenticator = RestSharp.Authenticators.OAuth1Authenticator.ForProtectedResource(_consumerKey, _consumerSecret, AuthenticationToken.Token, AuthenticationToken.Secret);

        }

        #region OAuth Stuff

        public AuthToken GetToken()
        {
            _apiRestClient.Authenticator = RestSharp.Authenticators.OAuth1Authenticator.ForRequestToken(_consumerKey, _consumerSecret, "oob");

            RestRequest request = _reqBuilder.BuildOAuthTokenRequest();


            IRestResponse response = _apiRestClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new CloudPTNetException();

            return ParseToken(response.Content);
        }

        public AuthToken GetAccessToken(AuthToken token, string pin)
        {
            _apiRestClient.Authenticator = RestSharp.Authenticators.OAuth1Authenticator.ForAccessToken(_consumerKey, _consumerSecret, token.Token, token.Secret, pin);

            RestRequest request = _reqBuilder.BuildOAuthAccessTokenRequest();


            IRestResponse response = _apiRestClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new CloudPTNetException();

            AuthenticationToken = ParseToken(response.Content);
            return _authToken;
        }

        public string GetOAuthAuthorizeUrl(AuthToken token)
        {
            return _reqBuilder.BuildOAuthAuthorizeUrl(token.Token);
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

        #region Account

        public AccountInfo AccountInfo()
        {
            RestRequest request = _reqBuilder.BuildAccountRequest();
            request.OnBeforeDeserialization = UidFix;
            var response = _apiRestClient.Execute<AccountInfo>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new CloudPTNetException();

            return response.Data;
        }

        //Forces the RestSharp Json parser to parse the uid field as a string by surrounding it with quotation marks,
        //otherwise it will try to parse it as a long (and will fail).
        //To be parsed as a double it should be in scientific notation format
        private void UidFix(IRestResponse response)
        {
            if (response.StatusCode != System.Net.HttpStatusCode.OK || string.IsNullOrWhiteSpace(response.Content))
                return;

            var content = response.Content;

            var regexPattern = "(\"uid\"): (?<uid>\\d+)";

            var replacePattern = "\"uid\": \"${uid}\"";

            content = Regex.Replace(content, regexPattern, replacePattern, RegexOptions.IgnoreCase);
            response.Content = content;
        }

        #endregion

        //ToDO: Partial Classes (Sync & Async), this goes there.
    }
}
