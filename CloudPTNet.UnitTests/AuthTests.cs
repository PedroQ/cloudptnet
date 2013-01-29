using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CloudPTNet;

namespace CloudPTNet.UnitTests
{
    [TestClass]
    public class AuthTests
    {
        string _consumerKey = "CloudPT Consumer key";
        string _consumerSecret = "CloudPT Secret";

        CloudPTClient client;

        [TestInitialize]
        public void SetUpClient()
        {
            client = new CloudPTClient(_consumerKey, _consumerSecret);
        }

        [TestCleanup]
        public void Teardown()
        {
            client = null;
        }

        [TestMethod]
        public void TestGetToken()
        {
            var token = client.GetToken();

            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Token);
            Assert.IsNotNull(token.Secret);
        }

        [TestMethod]
        public void TestBuildOGetOAuthAuthorizeUrl()
        {
            var authorizeUrl = client.GetOAuthAuthorizeUrl(new Auth.AuthToken("TEST_TOKEN", "TEST_TOKEN_SECRET"));

            Assert.IsNotNull(authorizeUrl);
        }
    }
}
