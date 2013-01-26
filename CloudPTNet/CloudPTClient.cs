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
        private const string _apiVer = "1";

        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private AuthToken _authToken;

        private RestClient _apiRestClient;
        private RestClient _apiContentRestClient;
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


            _reqBuilder = new RequestBuilder(_apiVer); //Convert to Singleton?

        }

        #region OAuth Stuff

        public AuthToken GetToken()
        {
            throw new NotImplementedException();
        }

        private AuthToken ParseToken(string p)
        {
            throw new NotImplementedException();
        }

        public void GetTokenAsync(Action<AuthToken> success, Action<CloudPTNetException> fail)
        {

        }

        //ToDO: Partial Classes (Sync & Async), this goes there.

        #endregion


    }
}
