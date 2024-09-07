using NUnit.Framework;
using RestSharp;
using ERPPlus.IntegrationTests.Config;
using ERPPlus.IntegrationTests.Data.Request;
using ERPPlus.IntegrationTests.Data.Request;

namespace ERPPlus.IntegrationTests.Tests.API.Authenticate
{
    [TestFixture]
    public class LoginTests
    {
        private RestClient client;

        [SetUp]
        public void SetUp()
        {
            client = new RestClient(AppConfig.BaseUrl);
        }

        [Test]
        public void Login_ReturnsSuccess()
        {
            var request = new RestRequest("/api/Authenticate/login", Method.Post);
            var loginModel = new LoginModel
            {
                Username = "testUser",
                Password = "password",
                Server = "server-1"
            };

            request.AddJsonBody(loginModel);
            var response = client.Execute(request);

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.IsNotNull(response.Content); // Check if login is successful
        }

        [TearDown]
        public void TearDown()
        {
            client.Dispose();
        }
    }
}
