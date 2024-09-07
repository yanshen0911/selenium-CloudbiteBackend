namespace ERPPlus.IntegrationTests.Data.Response
{
    public class LoginResponseModel
    {
        public string Token { get; set; }    // Example: Authentication token after login
        public string Username { get; set; }
        public string Message { get; set; }  // e.g., "Login successful" or error message
        public bool IsAuthenticated { get; set; }  // Boolean flag for success/failure
    }
}
