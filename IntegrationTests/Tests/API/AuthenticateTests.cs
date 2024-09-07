using NUnit.Framework;
using RestSharp;
using ERPPlus.IntegrationTests.Data.Requests;
using ERPPlus.IntegrationTests.Data.Responses;
using ERPPlus.IntegrationTests.Config;

namespace ERPPlus.IntegrationTests.Tests.API
{
    [TestFixture]
    public class AuthenticateTests
    {
        private RestClient client;

        [SetUp]
        public void SetUp()
        {
            client = new RestClient(AppConfig.BaseUrl);
        }

        [Test]
        public void Login_ValidCredentials_ReturnsSuccess()
        {
            var request = new RestRequest("/api/Authenticate/login", Method.Post);
            var loginModel = new LoginModel
            {
                Username = "admin",
                Password = "password"
            };

            request.AddJsonBody(loginModel);
            var response = client.Execute<LoginResponseModel>(request);

            Assert.AreEqual("Success", response.Data.Message);
            Assert.IsTrue(response.Data.IsAuthenticated);
        }

        [Test]
        public void Logout_ReturnsSuccess()
        {
            var request = new RestRequest("/api/Authenticate/logout", Method.Post);
            var response = client.Execute(request);

            Assert.AreEqual(200, (int)response.StatusCode);
        }

        [TearDown]
        public void TearDown()
        {
            client.Dispose();
        }
    }
}
