using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPTNet
{
    internal class RequestBuilder
    {
        private const string _apiUrl = "https://publicapi.cloudpt.pt";
        private const string _apiContentUrl = "https://api-content.cloudpt.pt";
        private const string _apiOAuthUrl = "https://cloudpt.pt";
        private const string _apiVer = "1";

        internal RequestBuilder()
        {

        }

        #region OAuth

        internal RestRequest BuildOAuthTokenRequest()
        {
            RestRequest request = new RestRequest(string.Format("{0}/{1}", _apiOAuthUrl, "oauth/request_token"));
            return request;
        }

        internal string BuildOAuthAuthorizeUrl(string token)
        {
            return string.Format("{0}/oauth/authorize?oauth_token={1}", _apiOAuthUrl, token); ;
        }

        internal RestRequest BuildOAuthAccessTokenRequest()
        {
            RestRequest request = new RestRequest(string.Format("{0}/{1}", _apiOAuthUrl, "oauth/access_token"));
            return request;
        } 

        #endregion

        #region Account

        internal RestRequest BuildAccountRequest()
        {
            RestRequest request = new RestRequest(string.Format("{0}/{1}", _apiUrl, "{version}/Account/Info"));
            request.AddUrlSegment("version", _apiVer);
            return request;
        }

        #endregion
    }
}
