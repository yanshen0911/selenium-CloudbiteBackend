namespace ERPPlus.IntegrationTests.Data.Responses
{
    public class RegisterResponseModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }  // Example: "Success" or "Failed"
        public string Message { get; set; } // Success or error message
    }
}
