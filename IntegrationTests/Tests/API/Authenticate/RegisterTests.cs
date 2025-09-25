using RestSharp;
using CloudbiteBackend.IntegrationTests.Config;
using CloudbiteBackend.IntegrationTests.Data.Request;

namespace CloudbiteBackend.IntegrationTests.Tests.API.Authenticate
{
    [TestFixture]
    public class RegisterTests
    {
        private RestClient client;

        [SetUp]
        public void SetUp()
        {
            client = new RestClient(AppConfig.BaseUrl);
        }

        [Test]
        public void Register_ReturnsSuccess()
        {
            var request = new RestRequest("/api/Authenticate/register", Method.Post);
            var registerModel = new RegisterModel
            {
                Username = "testUser",
                Email = "user@example.com",
                Name = "Test User",
                CellPhone = "1234567890",
                OfficePhone = "9876543210",
                NetworkId = "network-123",
                Status = "active",
                Supervisor = "supervisor-123",
                Password = "password",
                ReportTo = false,
                AccessLevel = "admin",
                UnitOffice = "HQ",
                Department = "IT"
            };

            request.AddJsonBody(registerModel);
            var response = client.Execute(request);

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.IsNotNull(response.Content); // Check if the registration response is returned
        }

        [TearDown]
        public void TearDown()
        {
            client.Dispose();
        }
    }
}
