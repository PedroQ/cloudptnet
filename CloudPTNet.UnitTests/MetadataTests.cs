using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudPTNet.UnitTests
{
    [TestClass]
    public class MetadataTests
    {

        CloudPTClient _client;

        [TestInitialize]
        public void SetUpTestClient()
        {
            _client = new CloudPTClient(ApiCredentials.consumerKey, ApiCredentials.consumerSecret, ApiCredentials.accessToken, ApiCredentials.accessTokenSecret, CloudPTClient.AccessRestrictions.CloudPT);
        }

        #region Metadata

        [TestMethod]
        public void TestGetRootMetadata()
        {
            var metadata = _client.GetMetadata(string.Empty);

            Assert.IsNotNull(metadata);
        }

        [TestMethod]
        public void TestGetRootMetadataWithoutContentList()
        {
            var metadata = _client.GetMetadata(string.Empty, false);

            Assert.IsNotNull(metadata);
            Assert.IsNull(metadata.contents);
        } 

        #endregion

        #region MetadataShare

        [TestMethod]
        public void TestMetadataShare()
        {
            var metadata = _client.GetMetadataShare("3d6fd392-9f49-409c-8449-a630609e6f22", "SharedFiles");
            Assert.IsNotNull(metadata);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMetadataShareInvalidShareIdArgument()
        {
            var metadata = _client.GetMetadataShare(null, "SharedFiles");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMetadataShareInvalidPathArgument()
        {
            var metadata = _client.GetMetadataShare("3d6fd392-9f49-409c-8449-a630609e6f22", null);
        } 

        #endregion
    }
}
