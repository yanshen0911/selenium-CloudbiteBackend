namespace CloudbiteBackend.IntegrationTests.Data.Request
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Server { get; set; } = null;  // Optional server field
    }
}
