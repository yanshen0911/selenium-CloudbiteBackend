using NUnit.Framework;
using RestSharp;
using CloudbiteBackend.IntegrationTests.Config;

namespace CloudbiteBackend.IntegrationTests.Tests.Test
{
    [TestFixture]
    public class LogTests
    {
        private RestClient client;

        [SetUp]
        public void SetUp()
        {
            client = new RestClient(AppConfig.BaseUrl);
        }

        [Test]
        public void TestLogs_AreGeneratedCorrectly()
        {
            var request = new RestRequest("/api/Test/TestLog", Method.Get);
            var response = client.Execute(request);

            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.IsNotNull(response.Content);
            Assert.IsTrue(response.Content.Contains("Log entry"), "Log was not generated as expected.");
        }

        [TearDown]
        public void TearDown()
        {
            client.Dispose();
        }
    }
}
