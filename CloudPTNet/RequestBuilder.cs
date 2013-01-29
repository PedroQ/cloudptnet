using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPTNet
{
    internal class RequestBuilder
    {
        private string _apiVer;

        internal RequestBuilder(string _apiVer)
        {
            this._apiVer = _apiVer;
        }

        internal RestRequest BuildOAuthTokenRequest()
        {
            RestRequest request = new RestRequest("oauth/request_token");
            return request;
        }

        internal RestRequest BuildOAuthAuthorizeRequest()
        {
            RestRequest request = new RestRequest("oauth/authorize");
            return request;
        }

        internal RestRequest BuildOAuthAccessTokenRequest()
        {
            RestRequest request = new RestRequest("oauth/access_token");
            return request;
        }
    }
}
