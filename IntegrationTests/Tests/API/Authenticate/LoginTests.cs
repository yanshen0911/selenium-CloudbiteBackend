using NUnit.Framework;
using RestSharp;
using CloudbiteBackend.IntegrationTests.Config;
using CloudbiteBackend.IntegrationTests.Data.Request;
using CloudbiteBackend.IntegrationTests.Data.Response;
using Newtonsoft.Json;

namespace CloudbiteBackend.IntegrationTests.Tests.API.Authenticate
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
        public void Login_ReturnsSuccessAndToken()
        {
            // Create the request and model
            var request = new RestRequest("/api/Authenticate/login", Method.Post);
            var loginModel = new LoginModel
            {
                Username = "testUser",
                Password = "password",
                Server = "server-1"
            };

            request.AddJsonBody(loginModel);

            // Execute the request and get the response
            var response = client.Execute(request);

            // Validate that the status code is 200 OK
            Assert.That((int)response.StatusCode, Is.EqualTo(200), "Expected status code 200, but received {0}", response.StatusCode);

            // Check that the response is not empty
            Assert.IsNotNull(response.Content, "Response content is null!");

            // Deserialize the response content into LoginResponseModel
            //var loginResponse = JsonConvert.DeserializeObject<LoginResponseModel>(response.Content);

            // Deserialize using the helper method with strict settings
            var loginResponse = JsonHelper.DeserializeStrict<LoginResponseModel>(response.Content);

            // Validate the content of the response (Token, Username, Success)
            Assert.IsTrue(!string.IsNullOrEmpty(loginResponse.Token), "Token is missing in the response.");
            Assert.That(loginResponse.Username, Is.EqualTo("testUser"), "Logged-in username is incorrect.");
            Assert.IsTrue(loginResponse.Success, "Login response did not indicate success.");
        }

        [Test]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            var request = new RestRequest("/api/Authenticate/login", Method.Post);
            var loginModel = new LoginModel
            {
                Username = "invalidUser",
                Password = "wrongPassword",
                Server = "server-1"
            };

            request.AddJsonBody(loginModel);

            var response = client.Execute(request);

            // Validate that the status code is 401 Unauthorized
            Assert.That((int)response.StatusCode, Is.EqualTo(401), "Expected status code 401 for unauthorized login.");

            // Optionally, check if the error message is returned
            Assert.That(response.Content, Is.Not.Empty, "No error message returned for invalid login.");
        }


        [TearDown]
        public void TearDown()
        {
            client.Dispose();
        }
    }
}
