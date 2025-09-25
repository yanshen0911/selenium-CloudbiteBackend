using NUnit.Framework;
using RestSharp;
using CloudbiteBackend.IntegrationTests.Config;

namespace CloudbiteBackend.IntegrationTests.Tests.API.Authenticate
{
    [TestFixture]
    public class LogoutTests
    {
        private RestClient client;

        [SetUp]
        public void SetUp()
        {
            client = new RestClient(AppConfig.BaseUrl);
        }

        [Test]
        public void Logout_ReturnsSuccess()
        {
            var request = new RestRequest("/api/Authenticate/logout", Method.Post);
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
