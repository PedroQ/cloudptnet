using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudPTNet.UnitTests
{
    [TestClass]
    public class AccountTests
    {

        CloudPTClient _client;

        [TestInitialize]
        public void SetUpTestClient()
        {
            _client = new CloudPTClient(ApiCredentials.consumerKey, ApiCredentials.consumerSecret, ApiCredentials.accessToken, ApiCredentials.accessTokenSecret);
        }

        [TestMethod]
        public void TestAccountInfo()
        {
            var accountInfo = _client.AccountInfo();

            Assert.IsNotNull(accountInfo);
        }
    }
}
