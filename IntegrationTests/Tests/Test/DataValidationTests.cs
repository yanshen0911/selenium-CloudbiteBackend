using NUnit.Framework;
using RestSharp;
using CloudbiteBackend.IntegrationTests.Config;

namespace CloudbiteBackend.IntegrationTests.Tests.Test
{
    [TestFixture]
    public class DataValidationTests
    {
        private RestClient client;

        [SetUp]
        public void SetUp()
        {
            client = new RestClient(AppConfig.BaseUrl);
        }

        [Test]
        public void Login_InvalidData_ReturnsValidationError()
        {
            var request = new RestRequest("/api/Authenticate/login", Method.Post);
            var invalidData = new
            {
                Username = "", // Empty username
                Password = "short" // Short password
            };

            request.AddJsonBody(invalidData);
            var response = client.Execute(request);

            Assert.AreEqual(400, (int)response.StatusCode); // Check that validation errors return a 400 Bad Request
            Assert.IsTrue(response.Content.Contains("Username is required"));
        }

        [TearDown]
        public void TearDown()
        {
            client.Dispose();
        }
    }
}
