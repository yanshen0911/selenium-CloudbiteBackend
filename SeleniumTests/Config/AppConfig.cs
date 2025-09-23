namespace CloudbiteBackend.SeleniumTests.Config
{
    public static class AppConfig
    {
        public static string TesterName = "Choo Yan Shen";

        //Staging Env
        public static string BaseUrl => "https://test4.cloudbite-staging.qubeposcloud-uat.com";
        public static string UserName => "khairilamir.nawi@qubeapps.com";
        public static string Password => "password";


        //Recording File Path
        public static string BaseVideoFolder => @"C:\Users\ChooYanShen\Desktop\Cloudbite_Backend\Cloudbite Backend Testing Video";

        //Exported Test Case File Path
        public static string CsvExportFolder => @"C:\Users\ChooYanShen\Desktop\Cloudbite_Backend\TestCase";

        //Test Case Template
        public static string TestCaseFile = @"D:\e-invoice\SeleniumTests\TestCaseTemplate.xlsx";

        //Test Data Template
        public static string TestDataFolder = @"D:\e-invoice\SeleniumTests\TestDataFolder";

        //Downloaded File Path
        public static string DownloadPath = @"C:\Users\ChooYanShen\Downloads";

        //Import Template Empty File
        public static string ImportBECSVFile = @"D:\e-invoice\SeleniumTests\Import Template Without Data\supplier.csv";
        public static string ImportStoreCSVFile = @"D:\e-invoice\SeleniumTests\Import Template Without Data\Store Excel Sheet-Template.csv";   
        public static string ImportCustomerCSVFile = @"D:\e-invoice\SeleniumTests\Import Template Without Data\Customer Excel Sheet-Template.csv";

        //Import Template Data File
        // add action download
        //public static string ImportBECSVFile = @"D:\e-invoice\SeleniumTests\Import Template\supplier.csv";
        //public static string ImportStoreCSVFile = @"D:\e-invoice\SeleniumTests\Import Template\Store Excel Sheet-Template.csv";
        //public static string ImportCustomerCSVFile = @"D:\e-invoice\SeleniumTests\Import Template\Customer Excel Sheet-Template.csv";

        //Image Path
        public static string SampleReceiptImage = @"D:\e-invoice\SeleniumTests\Image\SampleReceipt.jpg";
        public static string UserProfileImage = @"D:\e-invoice\SeleniumTests\Image\UserProfileImage.png";
        public static string BusinessEntityImage = @"D:\e-invoice\SeleniumTests\Image\BusinessEntity.jpg";




        // Function to be Add On
        // add action download for import file (Transaction Page)
        // Resubmit Transaction
        // pending module -> Dashboard, Transaction, Template Editor, Report
        // Change clean message to helper
        // Change export to helper
        // verify inport data

        // Future Enhancement
        // Manual Consolidation

    }
}
