namespace ERPPlus.IntegrationTests.Config
{
    public static class EnvironmentConfig
    {
        public static string EnvironmentName => "DEV"; // Change to PROD, QA, etc.

        public static string BaseUrl
        {
            get
            {
                switch (EnvironmentName)
                {
                    case "DEV":
                        return "https://localhost:7268";
                    case "QA":
                        return "https://qa-api.erpplus.com";
                    case "PROD":
                        return "https://api.erpplus.com";
                    default:
                        return "https://localhost:7268"; // Default to local
                }
            }
        }

        // Example: Database connection string or credentials
        public static string DbConnectionString
        {
            get
            {
                switch (EnvironmentName)
                {
                    case "DEV":
                        return "Server=localhost;Database=ERPPlusDev;User Id=devUser;Password=devPassword;";
                    case "QA":
                        return "Server=qa-db.erpplus.com;Database=ERPPlusQA;User Id=qaUser;Password=qaPassword;";
                    case "PROD":
                        return "Server=prod-db.erpplus.com;Database=ERPPlusProd;User Id=prodUser;Password=prodPassword;";
                    default:
                        return "Server=localhost;Database=ERPPlusDev;User Id=devUser;Password=devPassword;";
                }
            }
        }

        // Add more environment-specific settings if needed
    }
}
