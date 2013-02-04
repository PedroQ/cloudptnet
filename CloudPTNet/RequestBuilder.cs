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
        private string _accessRestrictionString;

        private CloudPTClient.AccessRestrictions _restricions;
        internal CloudPTClient.AccessRestrictions Restrictions
        {
            get
            {
                return _restricions;
            }
            set
            {
                _restricions = value;

                if (value == CloudPTClient.AccessRestrictions.CloudPT)
                    _accessRestrictionString = "cloudpt";
                else
                    _accessRestrictionString = "sandbox";
            }
        }

        internal RequestBuilder(CloudPTClient.AccessRestrictions securityMode)
        {
            Restrictions = securityMode;
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

        #region Metadata
        internal RestRequest BuildMetadataRequest(string path, bool listContents, int fileLimit, bool includeDeleted)
        {
            RestRequest request = new RestRequest(string.Format("{0}/{1}", _apiUrl, "{version}/Metadata/{access}/{path}"));

            request.AddUrlSegment("version", _apiVer);
            request.AddUrlSegment("access", _accessRestrictionString);
            request.AddUrlSegment("path", path);


            //Add only parameters that are different from default. Shoud save some bytes

            if (!listContents)
                request.AddParameter("list", false);

            if (fileLimit != 10000) //10 000 is the default value
                request.AddParameter("file_limit", fileLimit);

            if (includeDeleted)
                request.AddParameter("include_deleted", true);

            return request;
        }
        #endregion

        #region MetadataShare

        internal RestRequest BuildMetadataShareRequest(string shareId, string path, int fileLimit, CloudPTClient.MetadataShareOrderBy sortOrder, bool orderAscending, string cursor, string mimeType)
        {
            RestRequest request = new RestRequest(string.Format("{0}/{1}", _apiUrl, "{version}/MetadataShare/{shareId}/{path}"));

            request.AddUrlSegment("version", _apiVer);
            request.AddUrlSegment("shareId", shareId);
            request.AddUrlSegment("path", path);

            if (fileLimit != 10000)
                request.AddParameter("file_limit", fileLimit);

            switch (sortOrder)
            {
                case CloudPTClient.MetadataShareOrderBy.ModifiedDate:
                    request.AddParameter("order_by", "mtime");
                    break;
                case CloudPTClient.MetadataShareOrderBy.MimeType:
                    request.AddParameter("order_by", "mime_type");
                    break;
                case CloudPTClient.MetadataShareOrderBy.Size:
                    request.AddParameter("order_by", "size");
                    break;
                case CloudPTClient.MetadataShareOrderBy.Folder:
                    request.AddParameter("order_by", "folder");
                    break;
                default:
                case CloudPTClient.MetadataShareOrderBy.Name:
                    request.AddParameter("order_by", "name");
                    break;
            }

            if (!orderAscending)
                request.AddParameter("order_ascending", false);

            if (!string.IsNullOrWhiteSpace(cursor))
                request.AddParameter("cursor", cursor);

            if (!string.IsNullOrWhiteSpace(mimeType))
                request.AddParameter("mime_type", mimeType);

            return request;
        }

        #endregion

        #region Public Links

        internal RestRequest BuildListLinksRequest()
        {
            RestRequest request = new RestRequest(string.Format("{0}/{1}", _apiUrl, "{version}/ListLinks"));
            request.AddUrlSegment("version", _apiVer);
            return request;
        }

        internal RestRequest BuildDeleteLinkRequest(string shareId)
        {
            RestRequest request = new RestRequest(string.Format("{0}/{1}", _apiUrl, "{version}/DeleteLink"), Method.POST);
            request.AddUrlSegment("version", _apiVer);

            request.AddParameter("shareid", shareId);

            return request;
        }

        internal RestRequest BuildCreatePublicLinkRequest(string path)
        {
            RestRequest request = new RestRequest(string.Format("{0}/{1}", _apiUrl, "{version}/Shares/{access}/{path}"), Method.POST);
            request.AddUrlSegment("version", _apiVer);
            request.AddUrlSegment("access", _accessRestrictionString);
            request.AddUrlSegment("path", path);

            return request;
        }

        #endregion
    }
}
