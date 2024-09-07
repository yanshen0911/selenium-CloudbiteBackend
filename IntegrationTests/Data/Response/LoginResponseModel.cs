namespace ERPPlus.IntegrationTests.Data.Response
{
    public class LoginResponseModel
    {
        public string Token { get; set; }  // Token returned upon successful login
        public string Username { get; set; }  // The logged-in username
        public string Role { get; set; }  // Role or access level
        public bool Success { get; set; }  // Whether the login was successful or not
    }
}
