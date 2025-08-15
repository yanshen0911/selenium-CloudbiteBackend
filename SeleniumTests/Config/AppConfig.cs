using OpenQA.Selenium.DevTools.V136.Runtime;

namespace EInvoice.SeleniumTests.Config
{
    public static class AppConfig
    {
        public static string TesterName = "Choo Yan Shen";
        //Dev Env
        //public static string BaseUrl => "https://test.einvoice-dev.qubeposcloud-uat.com";
        //public static string UserName => "test@einvoice.com";
        //public static string Password => "password";

        //Staging Env diy (All type of log)
        public static string BaseUrl => "https://diy.einvoice-staging.qubeposcloud-uat.com";
        public static string UserName => "yanshen.choo@qubeapps.com";
        public static string Password => "Password123!";

        //Staging Env qubeappstest1 super admin
        //public static string BaseUrl => "https://qubeappstest1.einvoice-staging.qubeposcloud-uat.com";
        //public static string UserName => "support@qubeappstest1.com";
        //public static string Password => @"]alQa)-$\A";

        //Staging Env qubeappstest1
        //public static string BaseUrl => "https://qubeappstest1.einvoice-staging.qubeposcloud-uat.com";
        //public static string UserName => "yanshen.choo@qubeapps.com";
        //public static string Password => "Password123!";

        //Recording File Path
        public static string BaseVideoFolder => @"C:\Users\ChooYanShen\Desktop\E-Invoice\E-Invoice Testing Video";

        //Exported Test Case File Path
        public static string CsvExportFolder => @"C:\Users\ChooYanShen\Desktop\E-Invoice\TestCase";

        //Test Case Template
        public static string TestCaseFile = @"D:\e-invoice\SeleniumTests\TestCaseTemplate.xlsx";

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
        // Log add filter
        // Resubmit Transaction
        // pending module -> Dashboard, Profile, Transaction, Template Editor, Report
        // Change clean message to helper
        // Chnage export to helper

        // Future Enhancement
        // Manual Consolidation

    }
}
