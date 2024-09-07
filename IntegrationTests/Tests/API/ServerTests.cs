using NUnit.Framework;
using RestSharp;
using ERPPlus.IntegrationTests.Config;
using ERPPlus.IntegrationTests.Data.Requests;

namespace ERPPlus.IntegrationTests.Tests.API
{
    [TestFixture]
    public class ServerTests
    {
        private RestClient client;

        [SetUp]
        public void SetUp()
        {
            client = new RestClient(AppConfig.BaseUrl);
        }

        [Test]
        public void GetServerList_ReturnsSuccess()
        {
            var request = new RestRequest("/api/Server/GetServerList", Method.Post);
            var serverModel = new ServerSearchModel
            {
                EnvId = 1,
                ServerId = 2
            };

            request.AddJsonBody(serverModel);
            var response = client.Execute(request);

            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.IsNotNull(response.Content); // Check that the server list is returned
        }

        [TearDown]
        public void TearDown()
        {
            client.Dispose();
        }
    }
}
