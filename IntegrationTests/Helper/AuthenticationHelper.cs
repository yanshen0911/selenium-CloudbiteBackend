using ERPPlus.IntegrationTests.Config;
using RestSharp;

namespace ERPPlus.IntegrationTests.Helpers
{
    public static class AuthenticationHelper
    {
        public static string GetAuthToken()
        {
            var client = new RestClient(AppConfig.BaseUrl);
            var request = new RestRequest("/api/Authenticate/login", Method.Post);

            request.AddJsonBody(new
            {
                username = "admin",
                password = "password"
            });

            var response = client.Execute(request);
            // Extract token from the response (assuming the response has a token field)
            var token = response.Content; // Change based on the actual response structure
            AppConfig.AuthToken = token;
            return token;
        }
    }
}
