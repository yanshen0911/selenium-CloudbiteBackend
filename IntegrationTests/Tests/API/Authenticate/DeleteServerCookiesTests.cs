using NUnit.Framework;
using RestSharp;
using ERPPlus.IntegrationTests.Config;

namespace ERPPlus.IntegrationTests.Tests.API.Authenticate
{
    [TestFixture]
    public class DeleteServerCookiesTests
    {
        private RestClient client;

        [SetUp]
        public void SetUp()
        {
            client = new RestClient(AppConfig.BaseUrl);
        }

        [Test]
        public void DeleteServerCookies_ReturnsSuccess()
        {
            var request = new RestRequest("/api/Authenticate/deleteServerCookies", Method.Post);
            var response = client.Execute(request);

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
        }

        [TearDown]
        public void TearDown()
        {
            client.Dispose();
        }
    }
}
