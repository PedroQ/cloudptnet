using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudPTNet.UnitTests
{
    [TestClass]
    public class PublicLinksTests
    {

        CloudPTClient _client;

        [TestInitialize]
        public void SetUpTestClient()
        {
            _client = new CloudPTClient(ApiCredentials.consumerKey, ApiCredentials.consumerSecret, ApiCredentials.accessToken, ApiCredentials.accessTokenSecret, CloudPTClient.AccessRestrictions.CloudPT);
        }

        [TestMethod]
        public void TestGetPublicLinks()
        {
            var myLinks = _client.GetPublicLinks();
            Assert.IsNotNull(myLinks);
        }
    }
}
