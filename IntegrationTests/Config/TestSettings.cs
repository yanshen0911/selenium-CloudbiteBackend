namespace ERPPlus.IntegrationTests.Config
{
    public static class TestSettings
    {
        // Default timeout settings in seconds
        public static readonly int DefaultTimeout = 30;

        // Number of retries for failed tests or flaky tests
        public static readonly int MaxRetryCount = 3;

        // Default values for common test parameters
        public static readonly string DefaultUsername = "testUser";
        public static readonly string DefaultPassword = "Password123";

        // Delay between test steps (useful for simulating user delays in UI tests)
        public static readonly int StepDelayInMilliseconds = 500;

        // Add more global test settings if needed
    }
}
