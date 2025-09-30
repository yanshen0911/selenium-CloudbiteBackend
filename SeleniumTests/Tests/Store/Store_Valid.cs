using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using CloudbiteBackend.SeleniumTests.Config;
using CloudbiteBackend.SeleniumTests.Drivers;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Log;
using OpenQA.Selenium.Support.UI;
using ScreenRecorderLib;
using SeleniumExtras.WaitHelpers;
using SeleniumTests.Helpers;
using SeleniumTests.Pages.Store;
using System;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Globalization;
using System.Media;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using helperFunction = SeleniumTests.Helper.HelperFunction;

namespace SeleniumTests.Tests.Store
{

    public static class ExcelDataReaderStoreValid
    {
        public static IEnumerable<object[]> GetStoreGroupTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string Group_Code = worksheet.Cells[row, 1].Text?.Trim();
                    string Group_Desc = worksheet.Cells[row, 2].Text?.Trim();
                    string Group_Status = worksheet.Cells[row, 3].Text?.Trim();

                    yield return new object[]
                    {
                        Group_Code, Group_Desc, Group_Status
                    };

                }
            }
        }

        public static IEnumerable<object[]> GetUpdateStoreGroupTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string Group_Code = worksheet.Cells[row, 1].Text?.Trim();
                    string Group_Desc = worksheet.Cells[row, 2].Text?.Trim();
                    string Group_Status = worksheet.Cells[row, 3].Text?.Trim();

                    yield return new object[]
                    {
                        Group_Code, Group_Desc, Group_Status
                    };

                }
            }
        }

        public static IEnumerable<object[]> GetSearchStoreGroupTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string searchText = worksheet.Cells[row, 1].Text?.Trim();


                    yield return new object[]
                    {
                        searchText
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetStoreCountryTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string Country_Code = worksheet.Cells[row, 1].Text?.Trim();
                    string Country_Desc = worksheet.Cells[row, 2].Text?.Trim();
                    string Country_Status = worksheet.Cells[row, 3].Text?.Trim();

                    yield return new object[]
                    {
                        Country_Code, Country_Desc, Country_Status
                    };

                }
            }
        }

        public static IEnumerable<object[]> GetUpdateStoreCountryTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string Country_Code = worksheet.Cells[row, 1].Text?.Trim();
                    string Country_Desc = worksheet.Cells[row, 2].Text?.Trim();
                    string Country_Status = worksheet.Cells[row, 3].Text?.Trim();

                    yield return new object[]
                    {
                        Country_Code, Country_Desc, Country_Status
                    };
                }
            }
        }

        public static IEnumerable<object[]> GetSearchStoreCountryTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string searchText = worksheet.Cells[row, 1].Text?.Trim();


                    yield return new object[]
                    {
                        searchText
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetStoreStateTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string Country_Code = worksheet.Cells[row, 1].Text?.Trim();
                    string State_Code = worksheet.Cells[row, 2].Text?.Trim();
                    string State_Name = worksheet.Cells[row, 3].Text?.Trim();

                    yield return new object[]
                    {
                        Country_Code, State_Code, State_Name
                    };

                }
            }
        }

        public static IEnumerable<object[]> GetUpdateStoreStateTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string Country_Code = worksheet.Cells[row, 1].Text?.Trim();
                    string State_Code = worksheet.Cells[row, 2].Text?.Trim();
                    string State_Name = worksheet.Cells[row, 3].Text?.Trim();

                    yield return new object[]
                    {
                        Country_Code, State_Code, State_Name
                    };

                }
            }
        }

        public static IEnumerable<object[]> GetSearchStoreStateTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string searchText = worksheet.Cells[row, 1].Text?.Trim();

                    yield return new object[]
                    {
                        searchText
                    };

                }
            }
        }

        public static IEnumerable<object[]> GetStoreCityTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string Country_Code = worksheet.Cells[row, 1].Text?.Trim();
                    string State_Code = worksheet.Cells[row, 2].Text?.Trim();
                    string City_Code = worksheet.Cells[row, 3].Text?.Trim();
                    string City_Name = worksheet.Cells[row, 4].Text?.Trim();

                    yield return new object[]
                    {
                        Country_Code, State_Code, City_Code, City_Name
                    };

                }
            }
        }

        public static IEnumerable<object[]> GetUpdateStoreCityTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string Country_Code = worksheet.Cells[row, 1].Text?.Trim();
                    string State_Code = worksheet.Cells[row, 2].Text?.Trim();
                    string City_Code = worksheet.Cells[row, 3].Text?.Trim();
                    string City_Name = worksheet.Cells[row, 4].Text?.Trim();

                    yield return new object[]
                    {
                        Country_Code, State_Code, City_Code, City_Name
                    };
                }
            }
        }

        public static IEnumerable<object[]> GetSearchStoreCityTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string searchText = worksheet.Cells[row, 1].Text?.Trim();

                    yield return new object[]
                    {
                        searchText
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetSearchStoreTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string searchText = worksheet.Cells[row, 1].Text?.Trim();


                    yield return new object[]
                    {
                        searchText
                    };

                }
            }
        }

        public static IEnumerable<object[]> GetStoreTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string StoreCode = worksheet.Cells[row, 1].Text?.Trim();
                    string StoreName = worksheet.Cells[row, 2].Text?.Trim();
                    string StoreStatus = worksheet.Cells[row, 3].Text?.Trim();
                    string StoreLogo = worksheet.Cells[row, 4].Text?.Trim();
                    string StoreDesc = worksheet.Cells[row, 5].Text?.Trim();
                    string ContactPerson = worksheet.Cells[row, 6].Text?.Trim();
                    string Address1 = worksheet.Cells[row, 7].Text?.Trim();
                    string Address2 = worksheet.Cells[row, 8].Text?.Trim();
                    string Address3 = worksheet.Cells[row, 9].Text?.Trim();
                    string Address4 = worksheet.Cells[row, 10].Text?.Trim();
                    string Postcode = worksheet.Cells[row, 11].Text?.Trim();
                    string Email = worksheet.Cells[row, 12].Text?.Trim();
                    string PhoneNumber = worksheet.Cells[row, 13].Text?.Trim();
                    string EatmolID = worksheet.Cells[row, 14].Text?.Trim();
                    string StoreCountry = worksheet.Cells[row, 15].Text?.Trim();
                    string StoreState = worksheet.Cells[row, 16].Text?.Trim();
                    string City = worksheet.Cells[row, 17].Text?.Trim();
                    string PriceLevel = worksheet.Cells[row, 18].Text?.Trim();
                    string StoreGroup = worksheet.Cells[row, 19].Text?.Trim();
                    string eReceiptStatus = worksheet.Cells[row, 20].Text?.Trim();
                    string eReceiptEmail = worksheet.Cells[row, 21].Text?.Trim();
                    string eReceiptPDF = worksheet.Cells[row, 22].Text?.Trim();
                    string FooterPhoneNumber = worksheet.Cells[row, 23].Text?.Trim();
                    string FooterEmail = worksheet.Cells[row, 24].Text?.Trim();
                    string FooterMessage = worksheet.Cells[row, 25].Text?.Trim();

                    yield return new object[]
                    {
                        StoreCode, StoreName, StoreStatus, StoreLogo, StoreDesc, ContactPerson, Address1, Address2, Address3, Address4,
                        Postcode, Email, PhoneNumber, EatmolID, StoreCountry, StoreState, City, PriceLevel, StoreGroup, eReceiptStatus,
                        eReceiptEmail, eReceiptPDF, FooterPhoneNumber, FooterEmail, FooterMessage
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetUpdateStoreTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string StoreCode = worksheet.Cells[row, 1].Text?.Trim();
                    string StoreName = worksheet.Cells[row, 2].Text?.Trim();
                    string StoreStatus = worksheet.Cells[row, 3].Text?.Trim();
                    string StoreLogo = worksheet.Cells[row, 4].Text?.Trim();
                    string StoreDesc = worksheet.Cells[row, 5].Text?.Trim();
                    string ContactPerson = worksheet.Cells[row, 6].Text?.Trim();
                    string Address1 = worksheet.Cells[row, 7].Text?.Trim();
                    string Address2 = worksheet.Cells[row, 8].Text?.Trim();
                    string Address3 = worksheet.Cells[row, 9].Text?.Trim();
                    string Address4 = worksheet.Cells[row, 10].Text?.Trim();
                    string Postcode = worksheet.Cells[row, 11].Text?.Trim();
                    string Email = worksheet.Cells[row, 12].Text?.Trim();
                    string PhoneNumber = worksheet.Cells[row, 13].Text?.Trim();
                    string EatmolID = worksheet.Cells[row, 14].Text?.Trim();
                    string StoreCountry = worksheet.Cells[row, 15].Text?.Trim();
                    string StoreState = worksheet.Cells[row, 16].Text?.Trim();
                    string City = worksheet.Cells[row, 17].Text?.Trim();
                    string PriceLevel = worksheet.Cells[row, 18].Text?.Trim();
                    string StoreGroup = worksheet.Cells[row, 19].Text?.Trim();
                    string eReceiptStatus = worksheet.Cells[row, 20].Text?.Trim();
                    string eReceiptEmail = worksheet.Cells[row, 21].Text?.Trim();
                    string eReceiptPDF = worksheet.Cells[row, 22].Text?.Trim();
                    string FooterPhoneNumber = worksheet.Cells[row, 23].Text?.Trim();
                    string FooterEmail = worksheet.Cells[row, 24].Text?.Trim();
                    string FooterMessage = worksheet.Cells[row, 25].Text?.Trim();
                    string TerminalID = worksheet.Cells[row, 26].Text?.Trim();
                    string TerminalDesc = worksheet.Cells[row, 27].Text?.Trim();
                    string TerminalStatus = worksheet.Cells[row, 28].Text?.Trim();

                    yield return new object[]
                    {
                        StoreCode, StoreName, StoreStatus, StoreLogo, StoreDesc, ContactPerson, Address1, Address2, Address3, Address4,
                        Postcode, Email, PhoneNumber, EatmolID, StoreCountry, StoreState, City, PriceLevel, StoreGroup, eReceiptStatus,
                        eReceiptEmail, eReceiptPDF, FooterPhoneNumber, FooterEmail, FooterMessage, TerminalID, TerminalDesc, TerminalStatus
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetUpdateStoreTerminalTestData(string filePath, string sheetName)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception($"❌ Sheet '{sheetName}' is empty or missing in {filePath}");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string StoreCode = worksheet.Cells[row, 1].Text?.Trim();
                    string StoreName = worksheet.Cells[row, 2].Text?.Trim();
                    string StoreStatus = worksheet.Cells[row, 3].Text?.Trim();
                    string StoreLogo = worksheet.Cells[row, 4].Text?.Trim();
                    string StoreDesc = worksheet.Cells[row, 5].Text?.Trim();
                    string ContactPerson = worksheet.Cells[row, 6].Text?.Trim();
                    string Address1 = worksheet.Cells[row, 7].Text?.Trim();
                    string Address2 = worksheet.Cells[row, 8].Text?.Trim();
                    string Address3 = worksheet.Cells[row, 9].Text?.Trim();
                    string Address4 = worksheet.Cells[row, 10].Text?.Trim();
                    string Postcode = worksheet.Cells[row, 11].Text?.Trim();
                    string Email = worksheet.Cells[row, 12].Text?.Trim();
                    string PhoneNumber = worksheet.Cells[row, 13].Text?.Trim();
                    string EatmolID = worksheet.Cells[row, 14].Text?.Trim();
                    string StoreCountry = worksheet.Cells[row, 15].Text?.Trim();
                    string StoreState = worksheet.Cells[row, 16].Text?.Trim();
                    string City = worksheet.Cells[row, 17].Text?.Trim();
                    string PriceLevel = worksheet.Cells[row, 18].Text?.Trim();
                    string StoreGroup = worksheet.Cells[row, 19].Text?.Trim();
                    string eReceiptStatus = worksheet.Cells[row, 20].Text?.Trim();
                    string eReceiptEmail = worksheet.Cells[row, 21].Text?.Trim();
                    string eReceiptPDF = worksheet.Cells[row, 22].Text?.Trim();
                    string FooterPhoneNumber = worksheet.Cells[row, 23].Text?.Trim();
                    string FooterEmail = worksheet.Cells[row, 24].Text?.Trim();
                    string FooterMessage = worksheet.Cells[row, 25].Text?.Trim();
                    string TerminalID = worksheet.Cells[row, 26].Text?.Trim();
                    string TerminalDesc = worksheet.Cells[row, 27].Text?.Trim();
                    string TerminalStatus = worksheet.Cells[row, 28].Text?.Trim();

                    yield return new object[]
                    {
                        StoreCode, StoreName, StoreStatus, StoreLogo, StoreDesc, ContactPerson, Address1, Address2, Address3, Address4,
                        Postcode, Email, PhoneNumber, EatmolID, StoreCountry, StoreState, City, PriceLevel, StoreGroup, eReceiptStatus,
                        eReceiptEmail, eReceiptPDF, FooterPhoneNumber, FooterEmail, FooterMessage, TerminalID, TerminalDesc, TerminalStatus
                    };

                }
            }
        }


    }
        
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Store - Valid")]
    [AllureEpic("ERP-117")]
    public class Store_Valid
    {
        private IWebDriver _driver;
        private StorePage _StorePage;
        private WebDriverWait _wait;
        private LoginHelper _loginHelper;
        private Recorder _recorder;
        private string _recordingFilePath;
        private ManualResetEvent _recordingCompletedEvent = new ManualResetEvent(false);
        private List<string> _logMessages = new List<string>();
        private string _moduleName = "";


        private static string ExcelPath = Path.Combine(AppConfig.TestDataFolder, "StoreTestDataValid.xlsx");

        public static IEnumerable<object[]> StoreGroupTestData =>
        ExcelDataReaderStoreValid.GetStoreGroupTestData(ExcelPath, "StoreGroupTestData");

        public static IEnumerable<object[]> UpdateStoreGroupTestData =>
        ExcelDataReaderStoreValid.GetUpdateStoreGroupTestData(ExcelPath, "UpdateStoreGroupTestData");

        public static IEnumerable<object[]> SearchStoreGroupTestData =>
        ExcelDataReaderStoreValid.GetSearchStoreGroupTestData(ExcelPath, "SearchStoreGroupTestData");

        public static IEnumerable<object[]> StoreCountryTestData =>
        ExcelDataReaderStoreValid.GetStoreCountryTestData(ExcelPath, "StoreCountryTestData");

        public static IEnumerable<object[]> UpdateStoreCountryTestData =>
        ExcelDataReaderStoreValid.GetUpdateStoreCountryTestData(ExcelPath, "UpdateStoreCountryTestData");

        public static IEnumerable<object[]> SearchStoreCountryTestData =>
        ExcelDataReaderStoreValid.GetSearchStoreCountryTestData(ExcelPath, "SearchStoreCountryTestData");

        public static IEnumerable<object[]> StoreStateTestData =>
        ExcelDataReaderStoreValid.GetStoreStateTestData(ExcelPath, "StoreStateTestData");

        public static IEnumerable<object[]> UpdateStoreStateTestData =>
        ExcelDataReaderStoreValid.GetUpdateStoreStateTestData(ExcelPath, "UpdateStoreStateTestData");

        public static IEnumerable<object[]> SearchStoreStateTestData =>
        ExcelDataReaderStoreValid.GetSearchStoreStateTestData(ExcelPath, "SearchStoreStateTestData");

        public static IEnumerable<object[]> StoreCityTestData =>
        ExcelDataReaderStoreValid.GetStoreCityTestData(ExcelPath, "StoreCityTestData");

        public static IEnumerable<object[]> UpdateStoreCityTestData =>
        ExcelDataReaderStoreValid.GetUpdateStoreCityTestData(ExcelPath, "UpdateStoreCityTestData");

        public static IEnumerable<object[]> SearchStoreCityTestData =>
        ExcelDataReaderStoreValid.GetSearchStoreCityTestData(ExcelPath, "SearchStoreCityTestData");

        public static IEnumerable<object[]> StoreTestData =>
        ExcelDataReaderStoreValid.GetStoreTestData(ExcelPath, "StoreTestData");

        public static IEnumerable<object[]> UpdateStoreTestData =>
        ExcelDataReaderStoreValid.GetUpdateStoreTestData(ExcelPath, "UpdateStoreTestData");

        public static IEnumerable<object[]> UpdateStoreTerminalTestData =>
        ExcelDataReaderStoreValid.GetUpdateStoreTerminalTestData(ExcelPath, "UpdateStoreTerminalTestData");
        public static IEnumerable<object[]> SearchStoreTestData =>
        ExcelDataReaderStoreValid.GetSearchStoreTestData(ExcelPath, "SearchStoreTestData");

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string moduleName = "Store Page"; // You can make this dynamic if needed

            // 🔹 Build base folder
            string folderWithModule = Path.Combine(AppConfig.CsvExportFolder, moduleName, today);
            Directory.CreateDirectory(folderWithModule);

            // 🔹 Find the next version number
            int version = 1;
            string baseFileName;
            string exportPath;
            do
            {
                baseFileName = $"TestResults_{moduleName.Replace(" ", "_")}_{today}_v{version}.xlsx";
                exportPath = Path.Combine(folderWithModule, baseFileName);
                version++;
            } while (File.Exists(exportPath));

            // Store for use later in ExportTestResultToExcel
            _exportFilePath = exportPath;

            Console.WriteLine($"📂 Using export file: {_exportFilePath}");

            // ✅ Continue with test setup
            _driver = DriverFactory.CreateDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login");

            // ✅ Capture footer before login
            try
            {
                var footerElement = _wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("/html/body/app-root/body/app-login/div/div[1]/div[2]/app-footer/div")
                ));
                _footerValue = footerElement.Text.Trim();
                Console.WriteLine($"📄 Footer captured on login page: {_footerValue}");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("⚠️ Footer not found on login page.");
                _footerValue = string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Failed to capture footer: {ex.Message}");
                _footerValue = string.Empty;
            }

            // 🔑 Perform login AFTER capturing footer
            _loginHelper = new LoginHelper(_driver, _wait);
            _loginHelper.PerformLogin(AppConfig.UserName, AppConfig.Password, false);
            helperFunction.WaitForPageToLoad(_wait);
        }




        [SetUp]
        public void SetUp()
        {
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/dashboard/sales-db");
            helperFunction.WaitForPageToLoad(_wait);
            _StorePage = new StorePage(_driver);
            _logMessages.Clear();

            _moduleName = "Store Page";

            // Build file path details
            string testName = NUnit.Framework.TestContext.CurrentContext.Test.MethodName;
            string baseFolderPath = AppConfig.BaseVideoFolder;
            string todayFolderName = DateTime.Now.ToString("yyyy-MM-dd");
            string timeStampReadable = DateTime.Now.ToString("HH-mm-ss");

            string fullFolderPath = Path.Combine(baseFolderPath, todayFolderName, _moduleName);
            Directory.CreateDirectory(fullFolderPath);

            // 🔹 Add versioning for recordings
            int version = 1;
            string recordingFileName;
            do
            {
                recordingFileName = $"{_moduleName}_{testName}_v{version}.mp4";
                _recordingFilePath = Path.Combine(fullFolderPath, recordingFileName);
                version++;
            } while (File.Exists(_recordingFilePath));

            _recordingCompletedEvent.Reset();

            try
            {
                var options = new RecorderOptions
                {
                    RecorderMode = RecorderMode.Video,
                    VideoOptions = new VideoOptions
                    {
                        Framerate = 30,
                        Bitrate = 8000 * 1000
                    },
                    AudioOptions = new AudioOptions
                    {
                        IsAudioEnabled = false
                    }
                };

                _recorder = Recorder.CreateRecorder(options);
                _recorder.OnRecordingComplete += (s, e) => _recordingCompletedEvent.Set();
                _recorder.OnRecordingFailed += (s, e) => _recordingCompletedEvent.Set();
                _recorder.Record(_recordingFilePath);
                Thread.Sleep(2000);

                Console.WriteLine($"📹 Recording started: {_recordingFilePath}");
            }
            catch (Exception ex)
            {
                LogStep($"❌ Failed to start recorder: {ex.Message}");
            }
        }



        [Test]
        [Category("Store")]
        [Order(1)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCaseSource(nameof(StoreGroupTestData))]
        public void CreateStoreGroup(string Groupcode, string GroupDesc, string GroupStatus)
        {
            try
            {
                // Step 0: Navigate to the Store page
                LogStep("Navigate to Store group page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-group");
                WaitForUIEffect();

                LogStep("Start Store Group Creation");

                LogStep("Click 'Add Group' button.");
                _StorePage.ClickNewGroupButton();
                WaitForUIEffect();

                LogStep($"Enter Group Code: {Groupcode}");
                _StorePage.EnterGroupcode(Groupcode);
                WaitForUIEffect();

                LogStep($"Enter Group Description: {GroupDesc}");
                _StorePage.EnterGroupDesc(GroupDesc);
                WaitForUIEffect();

                // Convert GroupStatus ("Active"/"Inactive") into true/false
                bool isGroupStatusChecked = GroupStatus.Equals("Active", StringComparison.OrdinalIgnoreCase);

                // Apply checkbox state
                _StorePage.SetCheckboxState(isGroupStatusChecked);
                WaitForUIEffect();
                LogStep($"Group Status Checkbox set to: {isGroupStatusChecked}");

                LogStep("Click 'Save' button.");
                var saveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group-dialog/div[1]/button[2]")));
                ScrollToElement(saveBtn);
                saveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Save' Confirmation button.");
                var confirmsaveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-confirm-dialog/div[2]/button[2]")));
                ScrollToElement(confirmsaveBtn);
                confirmsaveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Check for success modal.");
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"Modal Message: {message}");

                // Capture screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Validate modal message
                if (!message.ToLower().Contains("saved"))
                {
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                LogStep("✅ Store creation test completed successfully.");
            }
            catch (Exception ex)
            {
                // Save screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Log detailed error
                LogStep($"❌ Exception occurred: {ex.Message}");

                // Fail with full reason
                Assert.Fail($"Test failed due to exception: {ex.Message}");
            }
        }



        [Test]
        [Category("Store")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Update")]
        [TestCaseSource(nameof(UpdateStoreGroupTestData))]
        public void UpdateStoreGroup(string Groupcode, string GroupDesc, string GroupStatus)
        {
            try
            {
                // Step 0: Navigate to the Store page
                LogStep("Navigate to Store group page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-group");
                WaitForUIEffect();

                LogStep("Clicking 'Edit' button.");
                _StorePage.ClickEditButton(Groupcode);
                WaitForUIEffect();

                LogStep("Starting Update Store Group");

                LogStep($"Enter Group Description: {GroupDesc}");
                _StorePage.EnterGroupDesc(GroupDesc);
                WaitForUIEffect();

                // Convert GroupStatus ("Active"/"Inactive") into true/false
                bool isGroupStatusChecked = GroupStatus.Equals("Active", StringComparison.OrdinalIgnoreCase);

                // Apply checkbox state
                _StorePage.SetCheckboxState(isGroupStatusChecked);
                WaitForUIEffect();
                LogStep($"Group Status Checkbox set to: {isGroupStatusChecked}");

                LogStep("Click 'Save' button.");
                var saveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group-dialog/div[1]/button[2]")));
                ScrollToElement(saveBtn);
                saveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Save' Confirmation button.");
                var confirmsaveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-confirm-dialog/div[2]/button[2]")));
                ScrollToElement(confirmsaveBtn);
                confirmsaveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Check for success modal.");
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"Modal Message: {message}");

                // Capture screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Validate modal message
                if (!message.ToLower().Contains("saved"))
                {
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                LogStep("✅ Store update test completed successfully.");
            }
            catch (Exception ex)
            {
                // Save screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Log detailed error
                LogStep($"❌ Exception occurred: {ex.Message}");

                // Fail with full reason
                Assert.Fail($"Test failed due to exception: {ex.Message}");
            }
        }




        [Test]
        [Category("Store")]
        [Order(3)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Search - General Match (Partial Match Accepted)")]
        [TestCaseSource(nameof(SearchStoreGroupTestData))]
        public void Search_Store_Group(string searchText)
        {

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store group page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-group");
            WaitForUIEffect();


            LogStep($"🔍 Starting search for: {searchText}");
            _StorePage.SearchStore(searchText);
            helperFunction.WaitForSTRGroupTableToLoad(_wait);
            WaitForUIEffect();

            bool isMatchFound = false;

            while (true)
            {
                WaitForUIEffect(800);

                var rows = _driver.FindElements(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group/div[2]/p-table/div/div/table/tbody/tr"));
                LogStep($"📄 Rows found in current page: {rows.Count}");

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));

                    foreach (var cell in cells)
                    {
                        string cellText;
                        try
                        {
                            cellText = cell.FindElement(By.TagName("span")).Text.Trim();
                        }
                        catch
                        {
                            cellText = cell.Text.Trim();
                        }

                        LogStep($"🔎 Checking cell: '{cellText}' vs '{searchText}'");

                        if (cellText.Replace(" ", "").Contains(searchText.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                        {
                            isMatchFound = true;
                            LogStep($"✅ Match found for '{searchText}' in cell: '{cellText}'");
                            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                            break;
                        }
                    }

                    if (isMatchFound) break;
                }

                if (isMatchFound) break;

                try
                {
                    var nextButton = _driver.FindElement(By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-store/div/div[3]/div/div[2]/app-global-pagination/div/div[2]/ul/li[4]"));

                    if (!nextButton.GetAttribute("class").Contains("disabled"))
                    {
                        LogStep("⏭ Going to next page...");
                        nextButton.Click();
                        helperFunction.WaitForSTRGroupTableToLoad(_wait);
                        WaitForUIEffect(500);
                    }
                    else
                    {
                        _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                        var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                        File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                        LogStep("🛑 Reached last page. No more data.");
                        break;
                    }
                }
                catch (NoSuchElementException)
                {
                    LogStep("❌ Pagination not found. Ending search.");
                    break;
                }
            }

            WaitForUIEffect();
            LogStep($"Final match result for '{searchText}': {isMatchFound}");
            Assert.IsTrue(isMatchFound, $"❌ Match not found for '{searchText}' in any table cell.");
        }



        [Test]
        [Category("Store")]
        [Order(4)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create - Export Store Group Report")]
        public void ExportStoreReport()
        {
            string downloadPath = AppConfig.DownloadPath;
            string filePrefix = "Store Group";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store group page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-group");
            WaitForUIEffect();


            LogStep("Clicking 'Export' button...");
            helperFunction.WaitForElementToBeClickable(_wait,
                By.CssSelector("body > app-management > div > mat-sidenav-container > mat-sidenav-content > div:nth-child(2) > app-store > div > div > app-store-group > div.footerMarginTop > button"));
            _StorePage.ClickExportButton();


            LogStep("📄 Waiting for downloaded file to appear...");
            bool fileDownloaded = _StorePage.WaitForFileDownload(downloadPath, filePrefix, TimeSpan.FromSeconds(15));
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            Assert.IsTrue(fileDownloaded, "❌ No new download detected.");
            LogStep("✅ Export file downloaded successfully.");
        }



        [Test]
        [Category("Store")]
        [Order(5)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCaseSource(nameof(StoreCountryTestData))]
        public void CreateStoreCountry(string Countrycode, string CountryDesc, string CountryStatus)
        {
            try
            {
                // Step 0: Navigate to the Store page
                LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
                WaitForUIEffect();

                LogStep("Start Store Location Creation");

                LogStep("Click 'Add Country' button.");
                _StorePage.ClickNewCountryButton();
                WaitForUIEffect();

                LogStep($"Enter Country Code: {Countrycode}");
                _StorePage.EnterCountrycode(Countrycode);
                WaitForUIEffect();

                LogStep($"Enter Country Description: {CountryDesc}");
                _StorePage.EnterCountryDesc(CountryDesc);
                WaitForUIEffect();

                // Convert GroupStatus ("Active"/"Inactive") into true/false
                bool isCountryStatusChecked = CountryStatus.Equals("Active", StringComparison.OrdinalIgnoreCase);

                // Apply checkbox state
                _StorePage.SetCountryCheckboxState(isCountryStatusChecked);
                WaitForUIEffect();
                LogStep($"Country Status Checkbox set to: {isCountryStatusChecked}");

                LogStep("Click 'Save' button.");
                var saveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone-dialog/div[1]/button[2]")));
                ScrollToElement(saveBtn);
                saveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Save' Confirmation button.");
                var confirmsaveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-confirm-dialog/div[2]/button[2]")));
                ScrollToElement(confirmsaveBtn);
                confirmsaveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Check for success modal.");
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"Modal Message: {message}");

                // Capture screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Validate modal message
                if (!message.ToLower().Contains("saved"))
                {
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                LogStep("✅ Store creation test completed successfully.");
            }
            catch (Exception ex)
            {
                // Save screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Log detailed error
                LogStep($"❌ Exception occurred: {ex.Message}");

                // Fail with full reason
                Assert.Fail($"Test failed due to exception: {ex.Message}");
            }
        }


        [Test]
        [Category("Store")]
        [Order(6)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Update")]
        [TestCaseSource(nameof(UpdateStoreCountryTestData))]
        public void UpdateStoreCountry(string Countrycode, string CountryDesc, string CountryStatus)
        {
            try
            {
                // Step 0: Navigate to the Store page
                LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
                WaitForUIEffect();

                LogStep("Clicking 'Edit' button.");
                _StorePage.ClickEditCountryButton(Countrycode);
                WaitForUIEffect();

                LogStep("Starting Update Store Group");

                LogStep($"Enter Country Description: {CountryDesc}");
                _StorePage.EnterCountryDesc(CountryDesc);
                WaitForUIEffect();

                // Convert GroupStatus ("Active"/"Inactive") into true/false
                bool isCountryStatusChecked = CountryStatus.Equals("Active", StringComparison.OrdinalIgnoreCase);

                // Apply checkbox state
                _StorePage.SetCountryCheckboxState(isCountryStatusChecked);
                WaitForUIEffect();
                LogStep($"Country Status Checkbox set to: {isCountryStatusChecked}");

                LogStep("Click 'Save' button.");
                var saveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone-dialog/div[1]/button[2]")));
                ScrollToElement(saveBtn);
                saveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Save' Confirmation button.");
                var confirmsaveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-confirm-dialog/div[2]/button[2]")));
                ScrollToElement(confirmsaveBtn);
                confirmsaveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Check for success modal.");
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"Modal Message: {message}");

                // Capture screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Validate modal message
                if (!message.ToLower().Contains("saved"))
                {
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                LogStep("✅ Store creation test completed successfully.");
            }
            catch (Exception ex)
            {
                // Save screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Log detailed error
                LogStep($"❌ Exception occurred: {ex.Message}");

                // Fail with full reason
                Assert.Fail($"Test failed due to exception: {ex.Message}");
            }
        }


        [Test]
        [Category("Store")]
        [Order(7)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Search - General Match (Partial Match Accepted)")]
        [TestCaseSource(nameof(SearchStoreCountryTestData))]
        public void Search_Store_Country(string searchText)
        {

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store Location page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
            WaitForUIEffect();

            LogStep($"🔍 Starting search for: {searchText}");
            _StorePage.SearchStoreCountry(searchText);
            helperFunction.WaitForCountryTableToLoad(_wait);
            WaitForUIEffect();

            bool isMatchFound = false;

            while (true)
            {
                WaitForUIEffect(800);

                var rows = _driver.FindElements(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/div/table/tbody/tr"));
                LogStep($"📄 Rows found in current page: {rows.Count}");

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));

                    foreach (var cell in cells)
                    {
                        string cellText;
                        try
                        {
                            cellText = cell.FindElement(By.TagName("span")).Text.Trim();
                        }
                        catch
                        {
                            cellText = cell.Text.Trim();
                        }

                        LogStep($"🔎 Checking cell: '{cellText}' vs '{searchText}'");

                        if (cellText.Replace(" ", "").Contains(searchText.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                        {
                            isMatchFound = true;
                            LogStep($"✅ Match found for '{searchText}' in cell: '{cellText}'");
                            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                            break;
                        }
                    }

                    if (isMatchFound) break;
                }

                if (isMatchFound) break;

                try
                {
                    var nextButton = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/p-paginator/div/button[3]"));

                    if (!nextButton.GetAttribute("class").Contains("disabled"))
                    {
                        LogStep("⏭ Going to next page...");
                        nextButton.Click();
                        helperFunction.WaitForCountryTableToLoad(_wait);
                        WaitForUIEffect(500);
                    }
                    else
                    {
                        _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                        var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                        File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                        LogStep("🛑 Reached last page. No more data.");
                        break;
                    }
                }
                catch (NoSuchElementException)
                {
                    LogStep("❌ Pagination not found. Ending search.");
                    break;
                }
            }

            WaitForUIEffect();
            LogStep($"Final match result for '{searchText}': {isMatchFound}");
            Assert.IsTrue(isMatchFound, $"❌ Match not found for '{searchText}' in any table cell.");
        }



        [Test]
        [Category("Store")]
        [Order(8)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create - Export Store Country Report")]
        public void ExportStoreCountryReport()
        {
            string downloadPath = AppConfig.DownloadPath;
            string filePrefix = "Store Location Country";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store Location page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
            WaitForUIEffect();


            LogStep("Clicking 'Export' button...");
            helperFunction.WaitForElementToBeClickable(_wait,
                By.CssSelector("body > app-management > div > mat-sidenav-container > mat-sidenav-content > div:nth-child(2) > app-store > div > div > app-store-zone > div.footerMarginTop > button"));
            _StorePage.ClickExportLocationButton();


            LogStep("📄 Waiting for downloaded file to appear...");
            bool fileDownloaded = _StorePage.WaitForFileDownload(downloadPath, filePrefix, TimeSpan.FromSeconds(15));
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            Assert.IsTrue(fileDownloaded, "❌ No new download detected.");
            LogStep("✅ Export file downloaded successfully.");
        }


        [Test]
        [Category("Store")]
        [Order(9)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCaseSource(nameof(StoreStateTestData))]
        public void CreateStoreState(string Country, string Statecode, string StateDesc)
        {
            try
            {
                // Step 0: Navigate to the Store page
                LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
                WaitForUIEffect();

                LogStep("Click 'State' tab.");
                var stateBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/mat-tab-header/div/div/div/div[2]")));
                ScrollToElement(stateBtn);
                stateBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Start Store State Creation");

                LogStep("Click 'Add State' button.");
                _StorePage.ClickNewStateButton();
                WaitForUIEffect(1000);

                LogStep($"Select Store Country For Creation : {Country}");
                var dropdownElement = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-region-dialog/div[2]/div/div/form/div/div/div[1]/div/select"));
                var selectCountry = new SelectElement(dropdownElement);
                selectCountry.SelectByText(Country);

                LogStep($"Enter State Code: {Statecode}");
                _StorePage.EnterStatecode(Statecode);
                WaitForUIEffect();

                LogStep($"Enter State Name: {StateDesc}");
                _StorePage.EnterStateDesc(StateDesc);
                WaitForUIEffect();

                LogStep("Click 'Save' button.");
                var saveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-region-dialog/div[1]/button[2]")));
                ScrollToElement(saveBtn);
                saveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Save' Confirmation button.");
                var confirmsaveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-confirm-dialog/div[2]/button[2]")));
                ScrollToElement(confirmsaveBtn);
                confirmsaveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Check for success modal.");
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"Modal Message: {message}");

                // Capture screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Validate modal message
                if (!message.ToLower().Contains("saved"))
                {
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                LogStep("✅ Store creation test completed successfully.");
            }
            catch (Exception ex)
            {
                // Save screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Log detailed error
                LogStep($"❌ Exception occurred: {ex.Message}");

                // Fail with full reason
                Assert.Fail($"Test failed due to exception: {ex.Message}");
            }
        }


        [Test]
        [Category("Store")]
        [Order(10)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Update")]
        [TestCaseSource(nameof(UpdateStoreStateTestData))]
        public void UpdateStoreState(string Country, string Statecode, string StateDesc)
        {
            try
            {
                // Step 0: Navigate to the Store page
                LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
                WaitForUIEffect();

                LogStep("Click 'State' tab.");
                var stateBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/mat-tab-header/div/div/div/div[2]")));
                ScrollToElement(stateBtn);
                stateBtn.Click();

                LogStep("Update Store State.");

                LogStep("Clicking 'Edit' button.");
                _StorePage.ClickEditStateButton(Statecode);
                WaitForUIEffect();

                var dropdownElement = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-region-dialog/div[2]/div/div/form/div/div/div[1]/div/select"));
                var selectCountry = new SelectElement(dropdownElement);
                selectCountry.SelectByText(Country);

                LogStep($"Enter State Description: {StateDesc}");
                _StorePage.EnterStateDesc(StateDesc);
                WaitForUIEffect();

                LogStep("Click 'Save' button.");
                var saveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-region-dialog/div[1]/button[2]")));
                ScrollToElement(saveBtn);
                saveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Save' Confirmation button.");
                var confirmsaveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-confirm-dialog/div[2]/button[2]")));
                ScrollToElement(confirmsaveBtn);
                confirmsaveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Check for success modal.");
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"Modal Message: {message}");

                // Capture screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Validate modal message
                if (!message.ToLower().Contains("saved"))
                {
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                LogStep("✅ Store creation test completed successfully.");
            }
            catch (Exception ex)
            {
                // Save screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Log detailed error
                LogStep($"❌ Exception occurred: {ex.Message}");

                // Fail with full reason
                Assert.Fail($"Test failed due to exception: {ex.Message}");
            }
        }


        [Test]
        [Category("Store")]
        [Order(11)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Search - General Match (Partial Match Accepted)")]
        [TestCaseSource(nameof(SearchStoreStateTestData))]
        public void Search_Store_State(string searchText)
        {

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store Location page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
            WaitForUIEffect();

            LogStep("Click 'State' tab.");
            var stateBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/mat-tab-header/div/div/div/div[2]")));
            ScrollToElement(stateBtn);
            stateBtn.Click();

            LogStep($"🔍 Starting search for: {searchText}");
            _StorePage.SearchStoreState(searchText);
            helperFunction.WaitForStateTableToLoad(_wait);
            WaitForUIEffect();

            bool isMatchFound = false;

            while (true)
            {
                WaitForUIEffect(800);

                var rows = _driver.FindElements(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[2]/div/div/p-table/div/div/table/tbody/tr"));
                LogStep($"📄 Rows found in current page: {rows.Count}");

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));

                    foreach (var cell in cells)
                    {
                        string cellText;
                        try
                        {
                            cellText = cell.FindElement(By.TagName("span")).Text.Trim();
                        }
                        catch
                        {
                            cellText = cell.Text.Trim();
                        }

                        LogStep($"🔎 Checking cell: '{cellText}' vs '{searchText}'");

                        if (cellText.Replace(" ", "").Contains(searchText.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                        {
                            isMatchFound = true;
                            LogStep($"✅ Match found for '{searchText}' in cell: '{cellText}'");
                            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                            break;
                        }
                    }

                    if (isMatchFound) break;
                }

                if (isMatchFound) break;

                try
                {
                    var nextButton = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/p-paginator/div/button[3]"));

                    if (!nextButton.GetAttribute("class").Contains("disabled"))
                    {
                        LogStep("⏭ Going to next page...");
                        nextButton.Click();
                        helperFunction.WaitForStateTableToLoad(_wait);
                        WaitForUIEffect(500);
                    }
                    else
                    {
                        _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                        var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                        File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                        LogStep("🛑 Reached last page. No more data.");
                        break;
                    }
                }
                catch (NoSuchElementException)
                {
                    LogStep("❌ Pagination not found. Ending search.");
                    break;
                }
            }

            WaitForUIEffect();
            LogStep($"Final match result for '{searchText}': {isMatchFound}");
            Assert.IsTrue(isMatchFound, $"❌ Match not found for '{searchText}' in any table cell.");
        }



        [Test]
        [Category("Store")]
        [Order(12)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create - Export Store State Report")]
        public void ExportStoreStateReport()
        {
            string downloadPath = AppConfig.DownloadPath;
            string filePrefix = "Store Location State";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store Location page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
            WaitForUIEffect();

            LogStep("Click 'State' tab.");
            var stateBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/mat-tab-header/div/div/div/div[2]")));
            ScrollToElement(stateBtn);
            stateBtn.Click();
            WaitForUIEffect();


            LogStep("Clicking 'Export' button...");
            helperFunction.WaitForElementToBeClickable(_wait,
                By.CssSelector("body > app-management > div > mat-sidenav-container > mat-sidenav-content > div:nth-child(2) > app-store > div > div > app-store-zone > div.footerMarginTop > button"));
            _StorePage.ClickExportLocationButton();


            LogStep("📄 Waiting for downloaded file to appear...");
            bool fileDownloaded = _StorePage.WaitForFileDownload(downloadPath, filePrefix, TimeSpan.FromSeconds(15));
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            Assert.IsTrue(fileDownloaded, "❌ No new download detected.");
            LogStep("✅ Export file downloaded successfully.");
        }

        [Test]
        [Category("Store")]
        [Order(13)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCaseSource(nameof(StoreCityTestData))]
        public void CreateStoreCity(string Country, string State, string Citycode, string CityDesc)
        {
            try
            {
                // Step 0: Navigate to the Store page
                LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
                WaitForUIEffect();

                LogStep("Click 'City' tab.");
                var cityBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/mat-tab-header/div/div/div/div[3]")));
                ScrollToElement(cityBtn);
                cityBtn.Click();
                WaitForUIEffect();

                LogStep("Start Store City Creation");

                LogStep("Click 'Add City' button.");
                _StorePage.ClickNewCityButton();
                WaitForUIEffect();

                var dropdownElement = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-area-dialog/div[2]/div/div/form/div/div/div[1]/div/select"));
                var selectCountry = new SelectElement(dropdownElement);
                selectCountry.SelectByText(Country);

                var dropdownElementState = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-area-dialog/div[2]/div/div/form/div/div/div[2]/div/select"));
                var selectState = new SelectElement(dropdownElementState);
                selectState.SelectByText(State);

                LogStep($"Enter City Code: {Citycode}");
                _StorePage.EnterCitycode(Citycode);
                WaitForUIEffect();

                LogStep($"Enter City Description: {CityDesc}");
                _StorePage.EnterCityDesc(CityDesc);
                WaitForUIEffect();

                LogStep("Click 'Save' button.");
                var saveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-area-dialog/div[1]/button[2]")));
                ScrollToElement(saveBtn);
                saveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Save' Confirmation button.");
                var confirmsaveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-confirm-dialog/div[2]/button[2]")));
                ScrollToElement(confirmsaveBtn);
                confirmsaveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Check for success modal.");
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"Modal Message: {message}");

                // Capture screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Validate modal message
                if (!message.ToLower().Contains("saved"))
                {
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                LogStep("✅ Store creation test completed successfully.");
            }
            catch (Exception ex)
            {
                // Save screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Log detailed error
                LogStep($"❌ Exception occurred: {ex.Message}");

                // Fail with full reason
                Assert.Fail($"Test failed due to exception: {ex.Message}");
            }
        }


        [Test]
        [Category("Store")]
        [Order(14)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Update")]
        [TestCaseSource(nameof(UpdateStoreCityTestData))]
        public void UpdateStoreCity(string Country, string State, string Citycode, string CityDesc)
        {
            try
            {
                // Step 0: Navigate to the Store page
                LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
                WaitForUIEffect();

                LogStep("Click 'City' tab.");
                var cityBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/mat-tab-header/div/div/div/div[3]")));
                ScrollToElement(cityBtn);
                cityBtn.Click();

                LogStep("Update Store State.");

                LogStep("Clicking 'Edit' button.");
                _StorePage.ClickEditCityButton(Citycode);
                WaitForUIEffect();

                var dropdownElement = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-area-dialog/div[2]/div/div/form/div/div/div[1]/div/select"));
                var selectCountry = new SelectElement(dropdownElement);
                selectCountry.SelectByText(Country);

                var dropdownElementState = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-area-dialog/div[2]/div/div/form/div/div/div[2]/div/select"));
                var selectState = new SelectElement(dropdownElementState);
                selectState.SelectByText(State);

                LogStep($"Enter City Description: {CityDesc}");
                _StorePage.EnterCityDesc(CityDesc);
                WaitForUIEffect();

                LogStep("Click 'Save' button.");
                var saveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-area-dialog/div[1]/button[2]")));
                ScrollToElement(saveBtn);
                saveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Save' Confirmation button.");
                var confirmsaveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-confirm-dialog/div[2]/button[2]")));
                ScrollToElement(confirmsaveBtn);
                confirmsaveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Check for success modal.");
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"Modal Message: {message}");

                // Capture screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Validate modal message
                if (!message.ToLower().Contains("saved"))
                {
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                LogStep("✅ Store creation test completed successfully.");
            }
            catch (Exception ex)
            {
                // Save screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Log detailed error
                LogStep($"❌ Exception occurred: {ex.Message}");

                // Fail with full reason
                Assert.Fail($"Test failed due to exception: {ex.Message}");
            }
        }


        [Test]
        [Category("Store")]
        [Order(15)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Search - General Match (Partial Match Accepted)")]
        [TestCaseSource(nameof(SearchStoreCityTestData))]
        public void Search_Store_City(string searchText)
        {

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store Location page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
            WaitForUIEffect();

            LogStep("Click 'City' tab.");
            var cityBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/mat-tab-header/div/div/div/div[3]")));
            ScrollToElement(cityBtn);
            cityBtn.Click();

            LogStep($"🔍 Starting search for: {searchText}");
            _StorePage.SearchStoreCity(searchText);
            helperFunction.WaitForCityTableToLoad(_wait);
            WaitForUIEffect();

            bool isMatchFound = false;

            while (true)
            {
                WaitForUIEffect(800);

                var rows = _driver.FindElements(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[3]/div/div/p-table/div/div/table/tbody/tr"));
                LogStep($"📄 Rows found in current page: {rows.Count}");

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));

                    foreach (var cell in cells)
                    {
                        string cellText;
                        try
                        {
                            cellText = cell.FindElement(By.TagName("span")).Text.Trim();
                        }
                        catch
                        {
                            cellText = cell.Text.Trim();
                        }

                        LogStep($"🔎 Checking cell: '{cellText}' vs '{searchText}'");

                        if (cellText.Replace(" ", "").Contains(searchText.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                        {
                            isMatchFound = true;
                            LogStep($"✅ Match found for '{searchText}' in cell: '{cellText}'");
                            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                            break;
                        }
                    }

                    if (isMatchFound) break;
                }

                if (isMatchFound) break;

                try
                {
                    var nextButton = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/p-paginator/div/button[3]"));

                    if (!nextButton.GetAttribute("class").Contains("disabled"))
                    {
                        LogStep("⏭ Going to next page...");
                        nextButton.Click();
                        helperFunction.WaitForCityTableToLoad(_wait);
                        WaitForUIEffect(500);
                    }
                    else
                    {
                        _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                        var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                        File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                        LogStep("🛑 Reached last page. No more data.");
                        break;
                    }
                }
                catch (NoSuchElementException)
                {
                    LogStep("❌ Pagination not found. Ending search.");
                    break;
                }
            }

            WaitForUIEffect();
            LogStep($"Final match result for '{searchText}': {isMatchFound}");
            Assert.IsTrue(isMatchFound, $"❌ Match not found for '{searchText}' in any table cell.");
        }



        [Test]
        [Category("Store")]
        [Order(16)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create - Export Store City Report")]
        public void ExportStoreCityReport()
        {
            string downloadPath = AppConfig.DownloadPath;
            string filePrefix = "Store Location City";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store Location page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
            WaitForUIEffect();

            LogStep("Click 'City' tab.");
            var cityBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/mat-tab-header/div/div/div/div[3]")));
            ScrollToElement(cityBtn);
            cityBtn.Click();
            WaitForUIEffect();


            LogStep("Clicking 'Export' button...");
            helperFunction.WaitForElementToBeClickable(_wait,
                By.CssSelector("body > app-management > div > mat-sidenav-container > mat-sidenav-content > div:nth-child(2) > app-store > div > div > app-store-zone > div.footerMarginTop > button"));
            _StorePage.ClickExportLocationButton();


            LogStep("📄 Waiting for downloaded file to appear...");
            bool fileDownloaded = _StorePage.WaitForFileDownload(downloadPath, filePrefix, TimeSpan.FromSeconds(15));
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            Assert.IsTrue(fileDownloaded, "❌ No new download detected.");
            LogStep("✅ Export file downloaded successfully.");
        }


        [Test]
        [Category("Store")]
        [Order(17)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCaseSource(nameof(StoreTestData))]
        public void CreateStore(string Storecode, string Storename, string Storestatus, string Storelogo, string StoreDesc,
                                string Contactperson, string Address1, string Address2, string Address3, string Address4,
                                string Postcode, string Email, string PhoneNum, string EatmolID, string Storecountry, string Storestate,
                                string City, string Pricelevel, string Storegroup, string eReceipt, string eReceiptEmail, string eReceiptPDF,
                                string FooterPhoneNum, string FooterEmail, string FooterMessage)
        {
            try
            {
                // Step 0: Navigate to the Store page
                LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-setup");
                WaitForUIEffect();


                LogStep("Start Store Creation");

                LogStep("Click 'Add Store' button.");
                _StorePage.ClickNewStoreButton();
                WaitForUIEffect();

                LogStep($"Enter Store Code: {Storecode}");
                _StorePage.EnterStorecode(Storecode);
                WaitForUIEffect();

                LogStep($"Enter Store Name: {Storename}");
                _StorePage.EnterStoreName(Storename);
                WaitForUIEffect();

                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                if (string.IsNullOrEmpty(Storelogo) || !File.Exists(Storelogo))
                {
                    LogStep($"❌ File not found at: {Storelogo}");
                    Assert.Fail("File not found: " + Storelogo);
                }
                LogStep($"📂 File found, ready to upload: {Storelogo}");

                var fileInput = wait.Until(ExpectedConditions.ElementExists(
                    By.CssSelector("app-store-dialog input[type='file']"))
                );

                fileInput.SendKeys(Storelogo);
                WaitForUIEffect();
                LogStep("📤 File upload initiated.");

                bool isStoreStatusChecked = Storestatus.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetCheckboxStoreState(isStoreStatusChecked);
                WaitForUIEffect();
                LogStep($"Group Status Checkbox set to: {isStoreStatusChecked}");

                LogStep($"Enter Store Description: {StoreDesc}");
                _StorePage.EnterStoreDesc(StoreDesc);
                WaitForUIEffect();

                LogStep($"Enter Contact Person Information: {Contactperson}");
                _StorePage.EnterStoreContactPerson(Contactperson);
                WaitForUIEffect();

                LogStep($"Enter Address 1: {Address1}");
                _StorePage.EnterStore1Address(Address1);
                WaitForUIEffect();

                LogStep($"Enter Address 2: {Address2}");
                _StorePage.EnterStore2Address(Address2);
                WaitForUIEffect();

                LogStep($"Enter Address 3: {Address3}");
                _StorePage.EnterStore3Address(Address3);
                WaitForUIEffect();

                LogStep($"Enter Address 4: {Address4}");
                _StorePage.EnterStore4Address(Address4);
                WaitForUIEffect();
                
                LogStep($"Enter Post Code: {Postcode}");
                _StorePage.EnterStorePostCode(Postcode);
                WaitForUIEffect();

                LogStep($"Enter Email Address: {Email}");
                _StorePage.EnterStoreEmail(Email);
                WaitForUIEffect();

                LogStep($"Enter Phone Number: {PhoneNum}");
                _StorePage.EnterStorePhoneNumber(PhoneNum);
                WaitForUIEffect();

                LogStep($"Enter Eatmol merchant ID: {EatmolID}");
                _StorePage.EnterEatmolMerchantID(EatmolID);
                WaitForUIEffect();
                
                LogStep("Click 'General Info' tab.");
                var generalInfoBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/mat-tab-header/div/div/div/div[2]")));
                ScrollToElement(generalInfoBtn);
                generalInfoBtn.Click();
                WaitForUIEffect(1000);

                LogStep($"Select Country: {Storecountry}");
                var StoreCountrydropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[1]/div[1]/div[1]/select"));
                var selectCountry = new SelectElement(StoreCountrydropdown);
                selectCountry.SelectByText(Storecountry);
                WaitForUIEffect(1000);

                LogStep($"Select State: {Storestate}");
                var StoreStatedropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[1]/div[2]/div[1]/select"));
                var selectState = new SelectElement(StoreStatedropdown);
                selectState.SelectByText(Storestate);
                WaitForUIEffect(1000);

                LogStep($"Select City: {City}");
                var StoreCitydropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[1]/div[3]/div[1]/select"));
                var selectCity = new SelectElement(StoreCitydropdown);
                selectCity.SelectByText(City);
                WaitForUIEffect(1000);

                LogStep($"Select Price Level: {Pricelevel}");
                var PriceLeveldropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[2]/div[1]/div[1]/select"));
                var selectPriceLevel = new SelectElement(PriceLeveldropdown);
                selectPriceLevel.SelectByText(Pricelevel);
                WaitForUIEffect(1000);

                LogStep($"Select Store Group: {Storegroup}");
                var StoreGroupdropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[2]/div[2]/div[1]/select"));
                var selectStoreGroup = new SelectElement(StoreGroupdropdown);
                selectStoreGroup.SelectByText(Storegroup);
                WaitForUIEffect(1000);

                LogStep("Click 'Receipt Setting' tab.");
                var receiptSettingBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/mat-tab-header/div/div/div/div[3]")));
                ScrollToElement(receiptSettingBtn);
                receiptSettingBtn.Click();
                WaitForUIEffect(1000);


                bool isEReceiptChecked = eReceipt.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetEReceiptCheckboxState(isEReceiptChecked);
                WaitForUIEffect();
                LogStep($"E-Receipt Status Checkbox set to: {isEReceiptChecked}");


                bool isSendEmailChecked = eReceiptEmail.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetEReceiptEmailCheckboxState(isSendEmailChecked);
                WaitForUIEffect();
                LogStep($"E-Receipt Send Email Checkbox set to: {isSendEmailChecked}");


                bool isGeneratePDFChecked = eReceiptPDF.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetEReceiptGenerateEmailCheckboxState(isGeneratePDFChecked);
                WaitForUIEffect();
                LogStep($"E-Receipt Generate PDF Checkbox set to: {isGeneratePDFChecked}");


                LogStep($"Enter Receipt Footer Phone Number: {FooterPhoneNum}");
                _StorePage.EnterFooterPhoneNumber(FooterPhoneNum);
                WaitForUIEffect();

                LogStep($"Enter Receipt Footer Email: {FooterEmail}");
                _StorePage.EnterFooterEmail(FooterEmail);
                WaitForUIEffect();

                LogStep($"Enter Receipt Footer Message: {FooterMessage}");
                _StorePage.EnterFooterMessage(FooterMessage);
                WaitForUIEffect();

                LogStep("Click 'Save' button.");
                var saveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[1]/button[2]")));
                ScrollToElement(saveBtn);
                saveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Save' Confirmation button.");
                var confirmsaveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-confirm-dialog/div[2]/button[2]")));
                ScrollToElement(confirmsaveBtn);
                confirmsaveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Check for success modal.");
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"Modal Message: {message}");

                // Capture screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Validate modal message
                if (!message.ToLower().Contains("saved"))
                {
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                LogStep("✅ Store creation test completed successfully.");
            }
            catch (Exception ex)
            {
                // Save screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Log detailed error
                LogStep($"❌ Exception occurred: {ex.Message}");

                // Fail with full reason
                Assert.Fail($"Test failed due to exception: {ex.Message}");
            }
        }


        [Test]
        [Category("Store")]
        [Order(18)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Update")]
        [TestCaseSource(nameof(UpdateStoreTestData))]
        public void UpdateStore_AddTerminalInfo(string Storecode, string Storename, string Storestatus, string Storelogo, string StoreDesc,
                                string Contactperson, string Address1, string Address2, string Address3, string Address4,
                                string Postcode, string Email, string PhoneNum, string EatmolID, string Storecountry, string Storestate,
                                string City, string Pricelevel, string Storegroup, string eReceipt, string eReceiptEmail, string eReceiptPDF,
                                string FooterPhoneNum, string FooterEmail, string FooterMessage, string TerminalID, string TerminalDesc, string TerminalStatus)
        {
            try
            {
                // Step 0: Navigate to the Store page
                LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-setup");
                WaitForUIEffect();

                LogStep("Update Store State.");

                LogStep("Clicking 'Edit' button.");
                _StorePage.ClickEditStoreButton(Storecode);
                WaitForUIEffect();

                LogStep($"Enter Store Name: {Storename}");
                _StorePage.EnterStoreName(Storename);
                WaitForUIEffect();

                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                if (string.IsNullOrEmpty(Storelogo) || !File.Exists(Storelogo))
                {
                    LogStep($"❌ File not found at: {Storelogo}");
                    Assert.Fail("File not found: " + Storelogo);
                }
                LogStep($"📂 File found, ready to upload: {Storelogo}");

                var fileInput = wait.Until(ExpectedConditions.ElementExists(
                    By.CssSelector("app-store-dialog input[type='file']"))
                );

                fileInput.SendKeys(Storelogo);
                WaitForUIEffect();
                LogStep("📤 File upload initiated.");

                bool isStoreStatusChecked = Storestatus.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetCheckboxStoreState(isStoreStatusChecked);
                WaitForUIEffect();
                LogStep($"Group Status Checkbox set to: {isStoreStatusChecked}");

                LogStep($"Enter Store Description: {StoreDesc}");
                _StorePage.EnterStoreDesc(StoreDesc);
                WaitForUIEffect();

                LogStep($"Enter Contact Person Information: {Contactperson}");
                _StorePage.EnterStoreContactPerson(Contactperson);
                WaitForUIEffect();

                LogStep($"Enter Address 1: {Address1}");
                _StorePage.EnterStore1Address(Address1);
                WaitForUIEffect();

                LogStep($"Enter Address 2: {Address2}");
                _StorePage.EnterStore2Address(Address2);
                WaitForUIEffect();

                LogStep($"Enter Address 3: {Address3}");
                _StorePage.EnterStore3Address(Address3);
                WaitForUIEffect();

                LogStep($"Enter Address 4: {Address4}");
                _StorePage.EnterStore4Address(Address4);
                WaitForUIEffect();

                LogStep($"Enter Post Code: {Postcode}");
                _StorePage.EnterStorePostCode(Postcode);
                WaitForUIEffect();

                LogStep($"Enter Email Address: {Email}");
                _StorePage.EnterStoreEmail(Email);
                WaitForUIEffect();

                LogStep($"Enter Phone Number: {PhoneNum}");
                _StorePage.EnterStorePhoneNumber(PhoneNum);
                WaitForUIEffect();

                LogStep($"Enter Eatmol merchant ID: {EatmolID}");
                _StorePage.EnterEatmolMerchantID(EatmolID);
                WaitForUIEffect();
                //
                LogStep("Click 'General Info' tab.");
                var generalInfoBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/mat-tab-header/div/div/div/div[2]")));
                ScrollToElement(generalInfoBtn);
                generalInfoBtn.Click();
                WaitForUIEffect(1000);

                LogStep($"Select Country: {Storecountry}");
                var StoreCountrydropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[1]/div[1]/div[1]/select"));
                var selectCountry = new SelectElement(StoreCountrydropdown);
                selectCountry.SelectByText(Storecountry);
                WaitForUIEffect(1000);

                LogStep($"Select State: {Storestate}");
                var StoreStatedropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[1]/div[2]/div[1]/select"));
                var selectState = new SelectElement(StoreStatedropdown);
                selectState.SelectByText(Storestate);
                WaitForUIEffect(1000);

                LogStep($"Select City: {City}");
                var StoreCitydropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[1]/div[3]/div[1]/select"));
                var selectCity = new SelectElement(StoreCitydropdown);
                selectCity.SelectByText(City);
                WaitForUIEffect(1000);

                LogStep($"Select Price Level: {Pricelevel}");
                var PriceLeveldropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[2]/div[1]/div[1]/select"));
                var selectPriceLevel = new SelectElement(PriceLeveldropdown);
                selectPriceLevel.SelectByText(Pricelevel);
                WaitForUIEffect(1000);

                LogStep($"Select Store Group: {Storegroup}");
                var StoreGroupdropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[2]/div[2]/div[1]/select"));
                var selectStoreGroup = new SelectElement(StoreGroupdropdown);
                selectStoreGroup.SelectByText(Storegroup);
                WaitForUIEffect(1000);

                LogStep("Click 'Terminal Info' tab.");
                var terminalInfogBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/mat-tab-header/div/div/div/div[3]")));
                ScrollToElement(terminalInfogBtn);
                terminalInfogBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Add Terminal' button.");
                _StorePage.ClickNewTerminalButton();
                WaitForUIEffect();

                LogStep($"Enter Terminal ID: {TerminalID}");
                _StorePage.EnterTerminalID(TerminalID);
                WaitForUIEffect();

                LogStep($"Enter Terminal Description: {TerminalDesc}");
                _StorePage.EnterTerminalDesc(TerminalDesc);
                WaitForUIEffect();

                bool isTerminalStatusChecked = TerminalStatus.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetTerminalStatusCheckboxState(isTerminalStatusChecked);
                WaitForUIEffect();
                LogStep($"Terminal Status Checkbox set to: {isTerminalStatusChecked}");

                LogStep("Click 'Save' button.");
                var saveTerminalBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-terminal-dialog/div[2]/button[2]")));
                ScrollToElement(saveTerminalBtn);
                saveTerminalBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Receipt Setting' tab.");
                var receiptSettingBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/mat-tab-header/div/div/div/div[4]")));
                ScrollToElement(receiptSettingBtn);
                receiptSettingBtn.Click();
                WaitForUIEffect(1000);


                bool isEReceiptChecked = eReceipt.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetEReceiptCheckboxState(isEReceiptChecked);
                WaitForUIEffect();
                LogStep($"E-Receipt Status Checkbox set to: {isEReceiptChecked}");


                bool isSendEmailChecked = eReceiptEmail.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetEReceiptEmailCheckboxState(isSendEmailChecked);
                WaitForUIEffect();
                LogStep($"E-Receipt Send Email Checkbox set to: {isSendEmailChecked}");


                bool isGeneratePDFChecked = eReceiptPDF.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetEReceiptGenerateEmailCheckboxState(isGeneratePDFChecked);
                WaitForUIEffect();
                LogStep($"E-Receipt Generate PDF Checkbox set to: {isGeneratePDFChecked}");


                LogStep($"Enter Receipt Footer Phone Number: {FooterPhoneNum}");
                _StorePage.EnterFooterPhoneNumber(FooterPhoneNum);
                WaitForUIEffect();

                LogStep($"Enter Receipt Footer Email: {FooterEmail}");
                _StorePage.EnterFooterEmail(FooterEmail);
                WaitForUIEffect();

                LogStep($"Enter Receipt Footer Message: {FooterMessage}");
                _StorePage.EnterFooterMessage(FooterMessage);
                WaitForUIEffect();

                LogStep("Click 'Save' button.");
                var saveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[1]/button[2]")));
                ScrollToElement(saveBtn);
                saveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Save' Confirmation button.");
                var confirmsaveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-confirm-dialog/div[2]/button[2]")));
                ScrollToElement(confirmsaveBtn);
                confirmsaveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Check for success modal.");
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"Modal Message: {message}");

                // Capture screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Validate modal message
                if (!message.ToLower().Contains("saved"))
                {
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                LogStep("✅ Store creation test completed successfully.");
            }
            catch (Exception ex)
            {
                // Save screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Log detailed error
                LogStep($"❌ Exception occurred: {ex.Message}");

                // Fail with full reason
                Assert.Fail($"Test failed due to exception: {ex.Message}");
            }
        }


        [Test]
        [Category("Store")]
        [Order(19)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Update")]
        [TestCaseSource(nameof(UpdateStoreTerminalTestData))]
        public void UpdateStore_EditTerminalInfo(string Storecode, string Storename, string Storestatus, string Storelogo, string StoreDesc,
                                string Contactperson, string Address1, string Address2, string Address3, string Address4,
                                string Postcode, string Email, string PhoneNum, string EatmolID, string Storecountry, string Storestate,
                                string City, string Pricelevel, string Storegroup, string eReceipt, string eReceiptEmail, string eReceiptPDF,
                                string FooterPhoneNum, string FooterEmail, string FooterMessage, string TerminalID, string TerminalDesc, string TerminalStatus)
        {
            try
            {
                // Step 0: Navigate to the Store page
                LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-setup");
                WaitForUIEffect();

                LogStep("Update Store State.");

                LogStep("Clicking 'Edit' button.");
                _StorePage.ClickEditStoreButton(Storecode);
                WaitForUIEffect();

                LogStep($"Enter Store Name: {Storename}");
                _StorePage.EnterStoreName(Storename);
                WaitForUIEffect();

                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                if (string.IsNullOrEmpty(Storelogo) || !File.Exists(Storelogo))
                {
                    LogStep($"❌ File not found at: {Storelogo}");
                    Assert.Fail("File not found: " + Storelogo);
                }
                LogStep($"📂 File found, ready to upload: {Storelogo}");

                var fileInput = wait.Until(ExpectedConditions.ElementExists(
                    By.CssSelector("app-store-dialog input[type='file']"))
                );

                fileInput.SendKeys(Storelogo);
                WaitForUIEffect();
                LogStep("📤 File upload initiated.");

                bool isStoreStatusChecked = Storestatus.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetCheckboxStoreState(isStoreStatusChecked);
                WaitForUIEffect();
                LogStep($"Group Status Checkbox set to: {isStoreStatusChecked}");

                LogStep($"Enter Store Description: {StoreDesc}");
                _StorePage.EnterStoreDesc(StoreDesc);
                WaitForUIEffect();

                LogStep($"Enter Contact Person Information: {Contactperson}");
                _StorePage.EnterStoreContactPerson(Contactperson);
                WaitForUIEffect();

                LogStep($"Enter Address 1: {Address1}");
                _StorePage.EnterStore1Address(Address1);
                WaitForUIEffect();

                LogStep($"Enter Address 2: {Address2}");
                _StorePage.EnterStore2Address(Address2);
                WaitForUIEffect();

                LogStep($"Enter Address 3: {Address3}");
                _StorePage.EnterStore3Address(Address3);
                WaitForUIEffect();

                LogStep($"Enter Address 4: {Address4}");
                _StorePage.EnterStore4Address(Address4);
                WaitForUIEffect();

                LogStep($"Enter Post Code: {Postcode}");
                _StorePage.EnterStorePostCode(Postcode);
                WaitForUIEffect();

                LogStep($"Enter Email Address: {Email}");
                _StorePage.EnterStoreEmail(Email);
                WaitForUIEffect();

                LogStep($"Enter Phone Number: {PhoneNum}");
                _StorePage.EnterStorePhoneNumber(PhoneNum);
                WaitForUIEffect();

                LogStep($"Enter Eatmol merchant ID: {EatmolID}");
                _StorePage.EnterEatmolMerchantID(EatmolID);
                WaitForUIEffect();
                //
                LogStep("Click 'General Info' tab.");
                var generalInfoBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/mat-tab-header/div/div/div/div[2]")));
                ScrollToElement(generalInfoBtn);
                generalInfoBtn.Click();
                WaitForUIEffect(1000);

                LogStep($"Select Country: {Storecountry}");
                var StoreCountrydropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[1]/div[1]/div[1]/select"));
                var selectCountry = new SelectElement(StoreCountrydropdown);
                selectCountry.SelectByText(Storecountry);
                WaitForUIEffect(1000);

                LogStep($"Select State: {Storestate}");
                var StoreStatedropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[1]/div[2]/div[1]/select"));
                var selectState = new SelectElement(StoreStatedropdown);
                selectState.SelectByText(Storestate);
                WaitForUIEffect(1000);

                LogStep($"Select City: {City}");
                var StoreCitydropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[1]/div[3]/div[1]/select"));
                var selectCity = new SelectElement(StoreCitydropdown);
                selectCity.SelectByText(City);
                WaitForUIEffect(1000);

                LogStep($"Select Price Level: {Pricelevel}");
                var PriceLeveldropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[2]/div[1]/div[1]/select"));
                var selectPriceLevel = new SelectElement(PriceLeveldropdown);
                selectPriceLevel.SelectByText(Pricelevel);
                WaitForUIEffect(1000);

                LogStep($"Select Store Group: {Storegroup}");
                var StoreGroupdropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[2]/div[2]/div[1]/select"));
                var selectStoreGroup = new SelectElement(StoreGroupdropdown);
                selectStoreGroup.SelectByText(Storegroup);
                WaitForUIEffect(1000);

                LogStep("Click 'Terminal Info' tab.");
                var terminalInfogBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/mat-tab-header/div/div/div/div[3]")));
                ScrollToElement(terminalInfogBtn);
                terminalInfogBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Find all store rows in table.");
                var rows = _driver.FindElements(By.XPath("//app-store-dialog//p-table//tbody/tr"));

                bool recordFound = false;

                foreach (var row in rows)
                {
                    // Get first column text (td[1])
                    var firstCol = row.FindElement(By.XPath("./td[1]"));
                    string cellText = firstCol.Text.Trim();

                    if (cellText.Equals(TerminalID, StringComparison.OrdinalIgnoreCase))
                    {
                        LogStep($"🎯 Found target store: {cellText}");

                        // Find edit button in td[4] of this row
                        var editBtn = row.FindElement(By.XPath("./td[4]//div/div[1]"));
                        ScrollToElement(editBtn);
                        editBtn.Click();
                        WaitForUIEffect(1000);
                        LogStep("✏️ Edit button clicked successfully.");

                        recordFound = true;
                        break;
                    }
                }

                if (!recordFound)
                {
                    LogStep($"❌ Target store '{TerminalID}' not found in table.");
                    Assert.Fail("Record not found: " + TerminalID);
                }


                LogStep($"Enter Terminal Description: {TerminalDesc}");
                _StorePage.EnterTerminalDesc(TerminalDesc);
                WaitForUIEffect();

                bool isTerminalStatusChecked = TerminalStatus.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetTerminalStatusCheckboxState(isTerminalStatusChecked);
                WaitForUIEffect();
                LogStep($"Terminal Status Checkbox set to: {isTerminalStatusChecked}");

                LogStep("Click 'Save' button.");
                var saveTerminalBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-terminal-dialog/div[2]/button[2]")));
                ScrollToElement(saveTerminalBtn);
                saveTerminalBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Receipt Setting' tab.");
                var receiptSettingBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/mat-tab-header/div/div/div/div[4]")));
                ScrollToElement(receiptSettingBtn);
                receiptSettingBtn.Click();
                WaitForUIEffect(1000);


                bool isEReceiptChecked = eReceipt.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetEReceiptCheckboxState(isEReceiptChecked);
                WaitForUIEffect();
                LogStep($"E-Receipt Status Checkbox set to: {isEReceiptChecked}");


                bool isSendEmailChecked = eReceiptEmail.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetEReceiptEmailCheckboxState(isSendEmailChecked);
                WaitForUIEffect();
                LogStep($"E-Receipt Send Email Checkbox set to: {isSendEmailChecked}");


                bool isGeneratePDFChecked = eReceiptPDF.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetEReceiptGenerateEmailCheckboxState(isGeneratePDFChecked);
                WaitForUIEffect();
                LogStep($"E-Receipt Generate PDF Checkbox set to: {isGeneratePDFChecked}");


                LogStep($"Enter Receipt Footer Phone Number: {FooterPhoneNum}");
                _StorePage.EnterFooterPhoneNumber(FooterPhoneNum);
                WaitForUIEffect();

                LogStep($"Enter Receipt Footer Email: {FooterEmail}");
                _StorePage.EnterFooterEmail(FooterEmail);
                WaitForUIEffect();

                LogStep($"Enter Receipt Footer Message: {FooterMessage}");
                _StorePage.EnterFooterMessage(FooterMessage);
                WaitForUIEffect();

                LogStep("Click 'Save' button.");
                var saveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[1]/button[2]")));
                ScrollToElement(saveBtn);
                saveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Save' Confirmation button.");
                var confirmsaveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-confirm-dialog/div[2]/button[2]")));
                ScrollToElement(confirmsaveBtn);
                confirmsaveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Check for success modal.");
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"Modal Message: {message}");

                // Capture screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Validate modal message
                if (!message.ToLower().Contains("saved"))
                {
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                LogStep("✅ Store creation test completed successfully.");
            }
            catch (Exception ex)
            {
                // Save screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Log detailed error
                LogStep($"❌ Exception occurred: {ex.Message}");

                // Fail with full reason
                Assert.Fail($"Test failed due to exception: {ex.Message}");
            }
        }


        [Test]
        [Category("Store")]
        [Order(20)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Update")]
        [TestCaseSource(nameof(UpdateStoreTestData))]
        public void UpdateStore_DeleteTerminalInfo(string Storecode, string Storename, string Storestatus, string Storelogo, string StoreDesc,
                        string Contactperson, string Address1, string Address2, string Address3, string Address4,
                        string Postcode, string Email, string PhoneNum, string EatmolID, string Storecountry, string Storestate,
                        string City, string Pricelevel, string Storegroup, string eReceipt, string eReceiptEmail, string eReceiptPDF,
                        string FooterPhoneNum, string FooterEmail, string FooterMessage, string TerminalID, string TerminalDesc, string TerminalStatus)
        {
            try
            {
                // Step 0: Navigate to the Store page
                LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-setup");
                WaitForUIEffect();

                LogStep("Update Store State.");

                LogStep("Clicking 'Edit' button.");
                _StorePage.ClickEditStoreButton(Storecode);
                WaitForUIEffect();

                LogStep($"Enter Store Name: {Storename}");
                _StorePage.EnterStoreName(Storename);
                WaitForUIEffect();

                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                if (string.IsNullOrEmpty(Storelogo) || !File.Exists(Storelogo))
                {
                    LogStep($"❌ File not found at: {Storelogo}");
                    Assert.Fail("File not found: " + Storelogo);
                }
                LogStep($"📂 File found, ready to upload: {Storelogo}");

                var fileInput = wait.Until(ExpectedConditions.ElementExists(
                    By.CssSelector("app-store-dialog input[type='file']"))
                );

                fileInput.SendKeys(Storelogo);
                WaitForUIEffect();
                LogStep("📤 File upload initiated.");

                bool isStoreStatusChecked = Storestatus.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetCheckboxStoreState(isStoreStatusChecked);
                WaitForUIEffect();
                LogStep($"Group Status Checkbox set to: {isStoreStatusChecked}");

                LogStep($"Enter Store Description: {StoreDesc}");
                _StorePage.EnterStoreDesc(StoreDesc);
                WaitForUIEffect();

                LogStep($"Enter Contact Person Information: {Contactperson}");
                _StorePage.EnterStoreContactPerson(Contactperson);
                WaitForUIEffect();

                LogStep($"Enter Address 1: {Address1}");
                _StorePage.EnterStore1Address(Address1);
                WaitForUIEffect();

                LogStep($"Enter Address 2: {Address2}");
                _StorePage.EnterStore2Address(Address2);
                WaitForUIEffect();

                LogStep($"Enter Address 3: {Address3}");
                _StorePage.EnterStore3Address(Address3);
                WaitForUIEffect();

                LogStep($"Enter Address 4: {Address4}");
                _StorePage.EnterStore4Address(Address4);
                WaitForUIEffect();

                LogStep($"Enter Post Code: {Postcode}");
                _StorePage.EnterStorePostCode(Postcode);
                WaitForUIEffect();

                LogStep($"Enter Email Address: {Email}");
                _StorePage.EnterStoreEmail(Email);
                WaitForUIEffect();

                LogStep($"Enter Phone Number: {PhoneNum}");
                _StorePage.EnterStorePhoneNumber(PhoneNum);
                WaitForUIEffect();

                LogStep($"Enter Eatmol merchant ID: {EatmolID}");
                _StorePage.EnterEatmolMerchantID(EatmolID);
                WaitForUIEffect();
                //
                LogStep("Click 'General Info' tab.");
                var generalInfoBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/mat-tab-header/div/div/div/div[2]")));
                ScrollToElement(generalInfoBtn);
                generalInfoBtn.Click();
                WaitForUIEffect(1000);

                LogStep($"Select Country: {Storecountry}");
                var StoreCountrydropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[1]/div[1]/div[1]/select"));
                var selectCountry = new SelectElement(StoreCountrydropdown);
                selectCountry.SelectByText(Storecountry);
                WaitForUIEffect(1000);

                LogStep($"Select State: {Storestate}");
                var StoreStatedropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[1]/div[2]/div[1]/select"));
                var selectState = new SelectElement(StoreStatedropdown);
                selectState.SelectByText(Storestate);
                WaitForUIEffect(1000);

                LogStep($"Select City: {City}");
                var StoreCitydropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[1]/div[3]/div[1]/select"));
                var selectCity = new SelectElement(StoreCitydropdown);
                selectCity.SelectByText(City);
                WaitForUIEffect(1000);

                LogStep($"Select Price Level: {Pricelevel}");
                var PriceLeveldropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[2]/div[1]/div[1]/select"));
                var selectPriceLevel = new SelectElement(PriceLeveldropdown);
                selectPriceLevel.SelectByText(Pricelevel);
                WaitForUIEffect(1000);

                LogStep($"Select Store Group: {Storegroup}");
                var StoreGroupdropdown = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/div/mat-tab-body[2]/div/div/form/div/div[2]/div[2]/div[1]/select"));
                var selectStoreGroup = new SelectElement(StoreGroupdropdown);
                selectStoreGroup.SelectByText(Storegroup);
                WaitForUIEffect(1000);

                LogStep("Click 'Terminal Info' tab.");
                var terminalInfogBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/mat-tab-header/div/div/div/div[3]")));
                ScrollToElement(terminalInfogBtn);
                terminalInfogBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Find all store rows in table.");
                var rows = _driver.FindElements(By.XPath("//app-store-dialog//p-table//tbody/tr"));

                bool recordFound = false;

                foreach (var row in rows)
                {
                    // Get first column text (td[1])
                    var firstCol = row.FindElement(By.XPath("./td[1]"));
                    string cellText = firstCol.Text.Trim();

                    if (cellText.Equals(TerminalID, StringComparison.OrdinalIgnoreCase))
                    {
                        LogStep($"🎯 Found target store: {cellText}");

                        // Find delete button in td[5] of this row
                        var deleteBtn = row.FindElement(By.XPath("./td[4]//div/div[2]"));
                        ScrollToElement(deleteBtn);
                        deleteBtn.Click();
                        WaitForUIEffect(1000);
                        LogStep("✏️ Delete button clicked successfully.");

                        recordFound = true;
                        break;
                    }
                }

                if (!recordFound)
                {
                    LogStep($"❌ Target store '{TerminalID}' not found in table.");
                    Assert.Fail("Record not found: " + TerminalID);
                }

                LogStep("Click 'Yes' Confirmation button.");
                var confirmDeleteBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-confirm-dialog/div[2]/button[2]")));
                ScrollToElement(confirmDeleteBtn);
                confirmDeleteBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Check for success modal.");
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]")));
                var message1 = modal.Text.Trim();
                LogStep($"Modal Message: {message1}");

                // Step 4: Capture screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Step 5: Validate modal message (delete confirmation)
                if (!message1.ToLower().Contains("deleted"))
                {
                    Assert.Fail($"❌ Expected delete success message but got: {message1}");
                }

                LogStep("✅ Store deletion completed successfully.");
                recordFound = true;


                LogStep("Click 'Receipt Setting' tab.");
                var receiptSettingBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[3]/div/mat-tab-group/mat-tab-header/div/div/div/div[4]")));
                ScrollToElement(receiptSettingBtn);
                receiptSettingBtn.Click();
                WaitForUIEffect(1000);


                bool isEReceiptChecked = eReceipt.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetEReceiptCheckboxState(isEReceiptChecked);
                WaitForUIEffect();
                LogStep($"E-Receipt Status Checkbox set to: {isEReceiptChecked}");


                bool isSendEmailChecked = eReceiptEmail.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetEReceiptEmailCheckboxState(isSendEmailChecked);
                WaitForUIEffect();
                LogStep($"E-Receipt Send Email Checkbox set to: {isSendEmailChecked}");


                bool isGeneratePDFChecked = eReceiptPDF.Equals("Active", StringComparison.OrdinalIgnoreCase);
                _StorePage.SetEReceiptGenerateEmailCheckboxState(isGeneratePDFChecked);
                WaitForUIEffect();
                LogStep($"E-Receipt Generate PDF Checkbox set to: {isGeneratePDFChecked}");


                LogStep($"Enter Receipt Footer Phone Number: {FooterPhoneNum}");
                _StorePage.EnterFooterPhoneNumber(FooterPhoneNum);
                WaitForUIEffect();

                LogStep($"Enter Receipt Footer Email: {FooterEmail}");
                _StorePage.EnterFooterEmail(FooterEmail);
                WaitForUIEffect();

                LogStep($"Enter Receipt Footer Message: {FooterMessage}");
                _StorePage.EnterFooterMessage(FooterMessage);
                WaitForUIEffect();

                LogStep("Click 'Save' button.");
                var saveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-dialog/div[1]/button[2]")));
                ScrollToElement(saveBtn);
                saveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Click 'Save' Confirmation button.");
                var confirmsaveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div[2]/div[2]/div/mat-dialog-container/app-confirm-dialog/div[2]/button[2]")));
                ScrollToElement(confirmsaveBtn);
                confirmsaveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Check for success modal.");
                var modal1 = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"Modal Message: {message}");

                // Validate modal message
                if (!(message.ToLower().Contains("saved") || message.ToLower().Contains("deleted")))
                {
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                LogStep("✅ Store creation test completed successfully.");
            }
            catch (Exception ex)
            {
                // Save screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Log detailed error
                LogStep($"❌ Exception occurred: {ex.Message}");

                // Fail with full reason
                Assert.Fail($"Test failed due to exception: {ex.Message}");
            }
        }


        [Test]
        [Category("Store")]
        [Order(21)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Search - General Match (Partial Match Accepted)")]
        [TestCaseSource(nameof(SearchStoreTestData))]
        public void Search_Store(string searchText)
        {

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-setup");
            WaitForUIEffect();

            LogStep($"🔍 Starting search for: {searchText}");
            _StorePage.SearchStore_Detail(searchText);
            helperFunction.WaitForStoreTableToLoad(_wait);
            WaitForUIEffect();

            bool isMatchFound = false;

            while (true)
            {
                WaitForUIEffect(800);

                var rows = _driver.FindElements(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-setup/div[2]/p-table/div/div/table/tbody/tr"));
                LogStep($"📄 Rows found in current page: {rows.Count}");

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));

                    foreach (var cell in cells)
                    {
                        string cellText;
                        try
                        {
                            cellText = cell.FindElement(By.TagName("span")).Text.Trim();
                        }
                        catch
                        {
                            cellText = cell.Text.Trim();
                        }

                        LogStep($"🔎 Checking cell: '{cellText}' vs '{searchText}'");

                        if (cellText.Replace(" ", "").Contains(searchText.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                        {
                            isMatchFound = true;
                            LogStep($"✅ Match found for '{searchText}' in cell: '{cellText}'");
                            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                            break;
                        }
                    }

                    if (isMatchFound) break;
                }

                if (isMatchFound) break;

                try
                {
                    var nextButton = _driver.FindElement(By.XPath("/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-setup/div[2]/p-table/div/p-paginator/div/button[3]"));

                    if (!nextButton.GetAttribute("class").Contains("disabled"))
                    {
                        LogStep("⏭ Going to next page...");
                        nextButton.Click();
                        helperFunction.WaitForStoreTableToLoad(_wait);
                        WaitForUIEffect(500);
                    }
                    else
                    {
                        _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                        var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                        File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                        LogStep("🛑 Reached last page. No more data.");
                        break;
                    }
                }
                catch (NoSuchElementException)
                {
                    LogStep("❌ Pagination not found. Ending search.");
                    break;
                }
            }

            WaitForUIEffect();
            LogStep($"Final match result for '{searchText}': {isMatchFound}");
            Assert.IsTrue(isMatchFound, $"❌ Match not found for '{searchText}' in any table cell.");
        }



        [Test]
        [Category("Store")]
        [Order(22)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create - Export Store City Report")]
        public void ExportStoreDetailReport()
        {
            string downloadPath = AppConfig.DownloadPath;
            string filePrefix = "Store Setup";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-setup");
            WaitForUIEffect(1000);

            LogStep("Clicking 'Export' button...");
            helperFunction.WaitForElementToBeClickable(_wait,
                By.CssSelector("body > app-management > div > mat-sidenav-container > mat-sidenav-content > div:nth-child(2) > app-store > div > div > app-store-setup > div.footerMarginTop > button"));
            _StorePage.ClickExportStoreButton();


            LogStep("📄 Waiting for downloaded file to appear...");
            bool fileDownloaded = _StorePage.WaitForFileDownload(downloadPath, filePrefix, TimeSpan.FromSeconds(15));
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            Assert.IsTrue(fileDownloaded, "❌ No new download detected.");
            LogStep("✅ Export file downloaded successfully.");
        }


        [TearDown]
        public void TearDown()
        {
            try
            {
                _recorder?.Stop();
                _recordingCompletedEvent.WaitOne(TimeSpan.FromSeconds(30));

                var context = NUnit.Framework.TestContext.CurrentContext;
                string testName = context.Test.MethodName;
                string result = context.Result.Outcome.Status.ToString();

                string inputParams = "";
                var testMethod = GetType().GetMethod(testName);
                var paramInfos = testMethod?.GetParameters();

                if (paramInfos != null && context.Test.Arguments.Length == paramInfos.Length)
                {
                    var formattedParams = new List<string>();
                    for (int i = 0; i < paramInfos.Length; i++)
                    {
                        string name = paramInfos[i].Name ?? $"Param{i + 1}";
                        string value = context.Test.Arguments[i]?.ToString() ?? "null";
                        formattedParams.Add($"{name} = {value}");
                    }
                    inputParams = string.Join(", ", formattedParams);
                }
                else
                {
                    inputParams = string.Join(", ", context.Test.Arguments.Select(arg => arg?.ToString() ?? "null"));
                }

                string message = CleanMessage(string.Join(" | ", _logMessages));
                DateTime time = DateTime.Now;

                ExportTestResultToExcel(testName, inputParams, result, message, time, _lastScreenshotPath);

            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error in TearDown: " + ex.Message);
            }
        }


        private string _lastModuleName = string.Empty;
        private int _testCaseCounter = 1;
        private string _lastScreenshotPath = null;
        private string _exportFilePath; // add class-level field
        private void ExportTestResultToExcel(string testName, string inputParams, string result, string message, DateTime time, string screenshotPath = null)
        {
            try
            {
                string testerName = AppConfig.TesterName;
                string developerName = AppConfig.FEDeveloperName + "\n" + AppConfig.BEDeveloperName;
                string managerName = AppConfig.ManagerName;
                string clientName = AppConfig.ClientName;
                string changeDesc = AppConfig.ChangeDesc;

                // Build export file path if not yet set
                if (string.IsNullOrEmpty(_exportFilePath))
                {
                    string today = DateTime.Now.ToString("yyyy-MM-dd");
                    string moduleName = _moduleName.Replace(" ", "_");
                    string folderWithModule = Path.Combine(AppConfig.CsvExportFolder, _moduleName, today);
                    Directory.CreateDirectory(folderWithModule);

                    string baseFileName = $"TestResults_{moduleName}_{today}.xlsx";
                    _exportFilePath = Path.Combine(folderWithModule, baseFileName);
                }

                // If not exist, copy from template
                if (!File.Exists(_exportFilePath))
                {
                    var templatePath = AppConfig.TestCaseFile;
                    File.Copy(templatePath, _exportFilePath);
                }

                var file = new FileInfo(_exportFilePath);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(file))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    // ✅ Keep header & footer design from template
                    worksheet.Cells["D5"].Value = changeDesc;
                    worksheet.Cells["D7"].Value = _footerValue;
                    worksheet.Cells["F2"].Value = testerName;
                    worksheet.Cells["F4"].Value = developerName;
                    worksheet.Cells["F6"].Value = managerName;
                    worksheet.Cells["F8"].Value = clientName;
                    worksheet.Cells["C13"].Value = testerName;
                    worksheet.Cells["D2"].Value = _moduleName;
                    worksheet.Cells["B13"].Value = DateTime.Now.ToString("yyyy-MM-dd");
                    worksheet.Cells["H2"].Value = DateTime.Now.ToString("yyyy-MM-dd");

                    int startRow = 19;
                    int row = startRow;

                    // Find next empty row (before footer starts)
                    while (!string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text))
                    {
                        row++;
                    }

                    // Reset counter if new module
                    if (_moduleName != _lastModuleName)
                    {
                        _testCaseCounter = 1;
                        _lastModuleName = _moduleName;
                    }

                    // Format test steps
                    string[] steps = message.Split(new[] { '\n', '•', '|' }, StringSplitOptions.RemoveEmptyEntries);
                    string formattedSteps = string.Join("\n", steps.Select((s, i) => $"{i + 1}. {s.Trim()}"));

                    // Extract expected result if passed
                    string expectedResult = "";
                    if (result.Equals("Passed", StringComparison.OrdinalIgnoreCase))
                    {
                        var modalLine = steps.FirstOrDefault(s => s.Trim().StartsWith("Modal:", StringComparison.OrdinalIgnoreCase));
                        if (!string.IsNullOrEmpty(modalLine))
                        {
                            expectedResult = modalLine.Substring(modalLine.IndexOf(':') + 1).Trim().Trim('"');
                        }
                        else
                        {
                            foreach (string s in steps.Reverse<string>())
                            {
                                string trimmed = s.Trim();
                                string lower = trimmed.ToLowerInvariant();
                                if (lower.Contains("successfully") || lower.Contains("has been") || lower.Contains("was saved")
                                    || lower.Contains("updated successfully") || lower.Contains("created") || lower.Contains("deleted")
                                    || lower.Contains("duplicate") || lower.Contains("success") || lower.Contains("match found")
                                    || lower.Contains("found") || lower.Contains("completed") || lower.Contains("download")
                                    || lower.Contains("processing") || lower.Contains("succeeded"))
                                {
                                    expectedResult = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(trimmed.TrimEnd('.'));
                                    break;
                                }
                            }
                        }
                    }

                    // Format input params
                    string formattedInputParams = string.Join(
                        Environment.NewLine,
                        (inputParams ?? string.Empty)
                            .Split(',')
                            .Select(p => p.Trim())
                    );

                    // ✅ Write to main test case table (old design kept)
                    worksheet.Cells[row, 1].Value = _testCaseCounter;
                    worksheet.Cells[row, 2].Value = _moduleName;
                    worksheet.Cells[row, 3].Value = testName;
                    worksheet.Cells[row, 4].Value = formattedSteps;
                    worksheet.Cells[row, 5].Value = expectedResult;
                    worksheet.Cells[row, 6].Value = formattedInputParams;
                    worksheet.Cells[row, 6].Style.WrapText = true;
                    worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[row, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[row, 7].Value = result;
                    worksheet.Cells[row, 8].Value = time.ToString("yyyy-MM-dd HH:mm:ss");

                    // ✅ Add color coding for result column
                    var statusCell = worksheet.Cells[row, 7];
                    statusCell.Style.Fill.PatternType = ExcelFillStyle.Solid;

                    if (result.Equals("Passed", StringComparison.OrdinalIgnoreCase))
                        statusCell.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                    else if (result.Equals("Failed", StringComparison.OrdinalIgnoreCase))
                        statusCell.Style.Fill.BackgroundColor.SetColor(Color.LightPink);

                    // ✅ Insert Screenshot (Screenshots sheet)
                    try
                    {
                        if (!string.IsNullOrEmpty(screenshotPath) && File.Exists(screenshotPath))
                        {
                            var screenshotSheet = package.Workbook.Worksheets["Screenshots"];
                            if (screenshotSheet == null)
                                screenshotSheet = package.Workbook.Worksheets.Add("Screenshots");

                            int imgRow = 2;
                            while (!string.IsNullOrWhiteSpace(screenshotSheet.Cells[imgRow, 1].Text))
                            {
                                imgRow += 28; // space between screenshots
                            }

                            int mergeWidth = 4;
                            screenshotSheet.Cells[imgRow, 1, imgRow, mergeWidth].Merge = true;
                            screenshotSheet.Cells[imgRow + 1, 1, imgRow + 1, mergeWidth].Merge = true;

                            var labelCell1 = screenshotSheet.Cells[imgRow, 1];
                            labelCell1.Value = $"🧪 Test Case {_testCaseCounter} : {testName}";
                            labelCell1.Style.Font.Bold = true;
                            labelCell1.Style.Font.Size = 12;
                            labelCell1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            labelCell1.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                            labelCell1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            var labelCell2 = screenshotSheet.Cells[imgRow + 1, 1];
                            labelCell2.Value = $"🕒 Timestamp: {time:yyyy-MM-dd HH:mm:ss}";
                            labelCell2.Style.Font.Italic = true;
                            labelCell2.Style.Font.Size = 11;
                            labelCell2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            labelCell2.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

                            for (int col = 1; col <= mergeWidth; col++)
                            {
                                screenshotSheet.Column(col).Width = 30;
                            }

                            var image = Image.FromFile(screenshotPath);
                            var excelImage = screenshotSheet.Drawings.AddPicture($"Screenshot_{testName}_{imgRow}", image);
                            excelImage.SetPosition(imgRow + 2, 5, 0, 0);
                            excelImage.SetSize(640, 360);

                            Console.WriteLine($"🖼️ Screenshot inserted successfully for test: {testName} at row {imgRow}.");
                        }
                    }
                    catch (Exception imgEx)
                    {
                        Console.WriteLine("⚠️ Failed to insert screenshot: " + imgEx.Message);
                    }

                    package.Save();
                    _testCaseCounter++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error in ExportTestResultToExcel: " + ex.Message);
            }
        }


        private string _footerValue = string.Empty;
        public void CaptureFooterBeforeLogin()
        {
            try
            {
                var footerElement = _wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("/html/body/app-root/body/app-login/div/div[1]/div[2]/app-footer/div")
                ));

                _footerValue = footerElement.Text.Trim();
                Console.WriteLine($"📄 Footer captured on login page: {_footerValue}");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("⚠️ Footer not found on login page.");
                _footerValue = string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Failed to capture footer on login page: {ex.Message}");
                _footerValue = string.Empty;
            }
        }


        private void LogStep(string message)
        {
            Console.WriteLine(message);
            _logMessages.Add(CleanMessage(message));
        }

        private string CleanMessage(string raw)
        {
            return raw?
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("\"", "'")
                .Replace("✅", "")
                .Replace("❌", "")
                .Replace("📤", "")
                .Replace("💾", "")
                .Replace("🖼️", "")
                .Replace("📢", "")
                .Replace("🔍", "")
                .Replace("⛔", "")
                .Replace("🟡", "")
                .Replace("🟢", "")
                .Replace("🔴", "")
                .Replace("📂", "")
                .Replace("🎉", "")
                .Replace("⏳", "")
                .Replace("⚠️", "")
                .Replace("📌", "")
                .Replace("📁", "")
                .Replace("📸", "")
                .Replace("📄", "")
                .Replace("🔎", "")
                .Replace("ℹ️", "")
                .Replace("🧭", "")
                .Replace("🆕", "")
                .Replace("⌨️", "")
                .Replace("📝", "")
                .Replace("🎨", "")
                .Replace("🎯", "")
                .Replace("🛠️", "")
                .Replace("☑️", "")
                .Replace("📜", "")
                .Replace("🔘", "")
                .Trim();
        }

        private void WaitForUIEffect(int ms = 2000)
        {
            Thread.Sleep(ms); // adjustable UI pause for better video capture
        }

        private void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", element);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            try
            {
                _driver?.Quit();
                _driver?.Dispose();
            }
            catch { }

            try
            {
                SystemSounds.Exclamation.Play();
            }
            catch { }
        }

    }
}
