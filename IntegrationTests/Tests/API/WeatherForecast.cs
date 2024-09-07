using NUnit.Framework;
using RestSharp;
using System.Net;

namespace IntegrationTests.Tests.API
{
    [TestFixture]
    public class WeatherForecastTests
    {
        private RestClient client;

        [SetUp]
        public void SetUp()
        {
            client = new RestClient("https://localhost:7268");
        }

        [Test]
        public void GetWeatherForecast_ReturnsSuccess()
        {
            var request = new RestRequest("/WeatherForecast", Method.Get);
            var response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TearDown]
        public void TearDown()
        {
            client.Dispose();
        }
    }
}
