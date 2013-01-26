using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPTNet
{
    public class RequestBuilder
    {
        private string _apiVer;

        public RequestBuilder(string _apiVer)
        {
            this._apiVer = _apiVer;
        }

        public RestRequest BuildTokenRequest()
        {
            RestRequest request = new RestRequest("{ver}/oauth/request_token");
            request.AddParameter("ver", _apiVer);
            return request;
        }
    }
}
