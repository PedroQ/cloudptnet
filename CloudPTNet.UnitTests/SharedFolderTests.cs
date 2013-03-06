using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudPTNet.UnitTests
{
    [TestClass]
    public class SharedFolderTests
    {

        CloudPTClient _client;

        [TestInitialize]
        public void SetUpTestClient()
        {
            _client = new CloudPTClient(ApiCredentials.consumerKey, ApiCredentials.consumerSecret, ApiCredentials.accessToken, ApiCredentials.accessTokenSecret, CloudPTClient.AccessRestrictions.CloudPT);
        }

        [TestMethod]
        public void TestListSharedFolders()
        {
            var folders = _client.ListSharedFolders();
        }
    }
}
