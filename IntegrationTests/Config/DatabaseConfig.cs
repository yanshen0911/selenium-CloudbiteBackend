namespace ERPPlus.IntegrationTests.Config
{
    public static class DatabaseConfig
    {
        // Database connection strings for different environments
        public static string GetConnectionString()
        {
            return EnvironmentConfig.DbConnectionString;
        }

        // Example method to return a test-specific database connection string
        public static string GetTestDatabaseConnectionString()
        {
            return "Server=localhost;Database=ERPPlusTest;User Id=testUser;Password=testPassword;";
        }
    }
}
