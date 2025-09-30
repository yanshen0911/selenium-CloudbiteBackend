namespace CloudbiteBackend.SeleniumTests.Config
{
    public static class AppConfig
    {
        public static string TesterName = "Choo Yan Shen";
        public static string FEDeveloperName = "Fahmy";
        public static string BEDeveloperName = "Hanif";
        public static string ManagerName = "Alan Ong";
        public static string ClientName = "";
        public static string ChangeDesc = "";



        //Staging Env
        public static string BaseUrl => "https://test4.cloudbite-staging.qubeposcloud-uat.com";
        public static string UserName => "khairilamir.nawi@qubeapps.com";
        public static string Password => "password";


        //Recording File Path
        public static string BaseVideoFolder => @"C:\Users\ChooYanShen\Desktop\Cloudbite_Backend\Cloudbite Backend Testing Video";

        //Exported Test Case File Path
        public static string CsvExportFolder => @"C:\Users\ChooYanShen\Desktop\Cloudbite_Backend\TestCase";

        //Test Case Template
        public static string TestCaseFile = @"D:\cloudbite\SeleniumTests\TestCaseTemplate.xlsx";

        //Test Data Template
        public static string TestDataFolder = @"D:\cloudbite\SeleniumTests\TestDataFolder";

        //Downloaded File Path
        public static string DownloadPath = @"C:\Users\ChooYanShen\Downloads";

        //Import Template Empty File
        public static string ImportBECSVFile = @"D:\cloudbite\SeleniumTests\Import Template Without Data\supplier.csv";
        public static string ImportStoreCSVFile = @"D:\cloudbite\SeleniumTests\Import Template Without Data\Store Excel Sheet-Template.csv";   
        public static string ImportCustomerCSVFile = @"D:\cloudbite\SeleniumTests\Import Template Without Data\Customer Excel Sheet-Template.csv";

        //Import Template Data File
        // add action download
        //public static string ImportBECSVFile = @"D:\cloudbite\SeleniumTests\Import Template\supplier.csv";
        //public static string ImportStoreCSVFile = @"D:\cloudbite\SeleniumTests\Import Template\Store Excel Sheet-Template.csv";
        //public static string ImportCustomerCSVFile = @"D:\cloudbite\SeleniumTests\Import Template\Customer Excel Sheet-Template.csv";

        //Image Path
        public static string SampleReceiptImage = @"D:\cloudbite\SeleniumTests\Image\SampleReceipt.jpg";
        public static string UserProfileImage = @"D:\cloudbite\SeleniumTests\Image\UserProfileImage.png";
        public static string BusinessEntityImage = @"D:\cloudbite\SeleniumTests\Image\BusinessEntity.jpg";

    }
}
