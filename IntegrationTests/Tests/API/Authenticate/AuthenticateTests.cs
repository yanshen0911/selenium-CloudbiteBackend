using NUnit.Framework;
using RestSharp;
using ERPPlus.IntegrationTests.Config;
using ERPPlus.IntegrationTests.Data.TestData;
using ERPPlus.IntegrationTests.Data.Request;
using ERPPlus.IntegrationTests.Data.Response;

namespace IntegrationTests.Tests.API.NewFolder
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

        [Test, TestCaseSource(typeof(LoginData), nameof(LoginData.LoginTestCases))]
        public void LoginTests(string username, string password, int expectedStatusCode)
        {
            var request = new RestRequest("/api/Authenticate/login", Method.Post);
            var loginModel = new LoginModel
            {
                Username = username,
                Password = password
            };

            request.AddJsonBody(loginModel);
            var response = client.Execute(request);

            Assert.That((int)response.StatusCode, Is.EqualTo(expectedStatusCode));
        }

        // Data-driven test using LoginData
        [Test, TestCaseSource(typeof(LoginData), nameof(LoginData.ValidLoginData))]
        public void Login_ValidCredentials_ReturnsSuccess(string username, string password)
        {
            var request = new RestRequest("/api/Authenticate/login", Method.Post);
            var loginModel = new LoginModel
            {
                Username = username,
                Password = password
            };

            request.AddJsonBody(loginModel);
            var response = client.Execute<LoginResponseModel>(request);

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.IsTrue(response.Data.IsAuthenticated, "Login should be successful.");
        }

        [Test, TestCaseSource(typeof(LoginData), nameof(LoginData.InvalidLoginData))]
        public void Login_InvalidCredentials_ReturnsFailure(string username, string password)
        {
            var request = new RestRequest("/api/Authenticate/login", Method.Post);
            var loginModel = new LoginModel
            {
                Username = username,
                Password = password
            };

            request.AddJsonBody(loginModel);
            var response = client.Execute<LoginResponseModel>(request);

            Assert.That((int)response.StatusCode, Is.EqualTo(401));  // Assuming the API returns 401 for invalid credentials
        }

        [TearDown]
        public void TearDown()
        {
            client.Dispose();
        }
    }
}
