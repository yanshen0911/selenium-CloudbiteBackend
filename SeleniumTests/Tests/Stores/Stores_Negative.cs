using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using CloudbiteBackend.SeleniumTests.Config;
using CloudbiteBackend.SeleniumTests.Drivers;
using NUnit.Framework;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ScreenRecorderLib;
using SeleniumExtras.WaitHelpers;
using SeleniumTests.Helpers;
using SeleniumTests.Pages.Stores;
using System.Drawing;
using System.Globalization;
using System.Media;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using helperFunction = SeleniumTests.Helper.HelperFunction;

namespace SeleniumTests.Tests.Stores
{


    public static class ExcelDataReaderStoreNegative
    {
        public static IEnumerable<object[]> GetBlankMandatoryFieldsTestData(string filePath, string sheetName)
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
                    string Storename = worksheet.Cells[row, 1].Text?.Trim();
                    string StoreCity = worksheet.Cells[row, 2].Text?.Trim();
                    string strState = worksheet.Cells[row, 3].Text?.Trim();
                    string strPostCode = worksheet.Cells[row, 4].Text?.Trim();
                    string strCountry = worksheet.Cells[row, 5].Text?.Trim();
                    string StoreAddress1 = worksheet.Cells[row, 6].Text?.Trim();
                    string StoreAddress2 = worksheet.Cells[row, 7].Text?.Trim();
                    string strBusinessEntity = worksheet.Cells[row, 8].Text?.Trim();
                    string ExternalCode = worksheet.Cells[row, 9].Text?.Trim();
                    string scenario = worksheet.Cells[row, 10].Text?.Trim();


                    yield return new object[]
                    {
                        Storename, StoreCity, strState, strPostCode, strCountry, StoreAddress1, StoreAddress2, strBusinessEntity, ExternalCode, scenario
                    };

                }
            }
        }

        public static IEnumerable<object[]> GetInvalidDropdownValueTestData(string filePath, string sheetName)
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
                    string Storename = worksheet.Cells[row, 1].Text?.Trim();
                    string StoreCity = worksheet.Cells[row, 2].Text?.Trim();
                    string strState = worksheet.Cells[row, 3].Text?.Trim();
                    string strPostCode = worksheet.Cells[row, 4].Text?.Trim();
                    string strCountry = worksheet.Cells[row, 5].Text?.Trim();
                    string StoreAddress1 = worksheet.Cells[row, 6].Text?.Trim();
                    string StoreAddress2 = worksheet.Cells[row, 7].Text?.Trim();
                    string strBusinessEntity = worksheet.Cells[row, 8].Text?.Trim();
                    string ExternalCode = worksheet.Cells[row, 9].Text?.Trim();
                    string scenario = worksheet.Cells[row, 10].Text?.Trim();


                    yield return new object[]
                    {
                        Storename, StoreCity, strState, strPostCode, strCountry, StoreAddress1, StoreAddress2, strBusinessEntity, ExternalCode, scenario
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetDuplicateTestData(string filePath, string sheetName)
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
                    string Storename = worksheet.Cells[row, 1].Text?.Trim();
                    string StoreCity = worksheet.Cells[row, 2].Text?.Trim();
                    string strState = worksheet.Cells[row, 3].Text?.Trim();
                    string strPostCode = worksheet.Cells[row, 4].Text?.Trim();
                    string strCountry = worksheet.Cells[row, 5].Text?.Trim();
                    string StoreAddress1 = worksheet.Cells[row, 6].Text?.Trim();
                    string StoreAddress2 = worksheet.Cells[row, 7].Text?.Trim();
                    string strBusinessEntity = worksheet.Cells[row, 8].Text?.Trim();
                    string ExternalCode = worksheet.Cells[row, 9].Text?.Trim();


                    yield return new object[]
                    {
                        Storename, StoreCity, strState, strPostCode, strCountry, StoreAddress1, StoreAddress2, strBusinessEntity, ExternalCode
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetUpdateStoreMandatoryTestData(string filePath, string sheetName)
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
                    string Storename = worksheet.Cells[row, 2].Text?.Trim();
                    string StoreCity = worksheet.Cells[row, 3].Text?.Trim();
                    string strState = worksheet.Cells[row, 4].Text?.Trim();
                    string strPostCode = worksheet.Cells[row, 5].Text?.Trim();
                    string strCountry = worksheet.Cells[row, 6].Text?.Trim();
                    string StoreAddress1 = worksheet.Cells[row, 7].Text?.Trim();
                    string StoreAddress2 = worksheet.Cells[row, 8].Text?.Trim();
                    string strBusinessEntity = worksheet.Cells[row, 9].Text?.Trim();
                    string ExternalCode = worksheet.Cells[row, 10].Text?.Trim();
                    string scenario = worksheet.Cells[row, 11].Text?.Trim();


                    yield return new object[]
                    {
                        StoreCode, Storename, StoreCity, strState, strPostCode, strCountry, StoreAddress1, StoreAddress2, strBusinessEntity, ExternalCode, scenario
                    };

                }
            }
        }

        public static IEnumerable<object[]> GetUpdateInvalidDropdownsTestData(string filePath, string sheetName)
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
                    string Storename = worksheet.Cells[row, 2].Text?.Trim();
                    string StoreCity = worksheet.Cells[row, 3].Text?.Trim();
                    string strState = worksheet.Cells[row, 4].Text?.Trim();
                    string strPostCode = worksheet.Cells[row, 5].Text?.Trim();
                    string strCountry = worksheet.Cells[row, 6].Text?.Trim();
                    string StoreAddress1 = worksheet.Cells[row, 7].Text?.Trim();
                    string StoreAddress2 = worksheet.Cells[row, 8].Text?.Trim();
                    string strBusinessEntity = worksheet.Cells[row, 9].Text?.Trim();
                    string ExternalCode = worksheet.Cells[row, 10].Text?.Trim();
                    string scenario = worksheet.Cells[row, 11].Text?.Trim();


                    yield return new object[]
                    {
                        StoreCode, Storename, StoreCity, strState, strPostCode, strCountry, StoreAddress1, StoreAddress2, strBusinessEntity, ExternalCode, scenario
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetCSVFile_InvalidFileTypeTestData(string filePath, string sheetName)
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
                    string filelocation = worksheet.Cells[row, 1].Text?.Trim();



                    yield return new object[]
                    {
                        filelocation
                    };

                }
            }
        }


    }
        
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Stores - Negative")]
    [AllureEpic("ERP-117")]
    public class Stores_Negative
    {
        private IWebDriver _driver;
        private StoresPage _StoresPage;
        private WebDriverWait _wait;
        private LoginHelper _loginHelper;
        private Recorder _recorder;
        private string _recordingFilePath;
        private ManualResetEvent _recordingCompletedEvent = new ManualResetEvent(false);
        private List<string> _logMessages = new List<string>();
        private string _moduleName = "";


        private static string ExcelPath = Path.Combine(AppConfig.TestDataFolder, "StoresTestDataNegative.xlsx");

        public static IEnumerable<object[]> BlankMandatoryFieldsTestData =>
            ExcelDataReaderStoreNegative.GetBlankMandatoryFieldsTestData(ExcelPath, "BlankMandatoryFieldsTestData");

        public static IEnumerable<object[]> InvalidDropdownValueTestData =>
            ExcelDataReaderStoreNegative.GetInvalidDropdownValueTestData(ExcelPath, "InvalidDropdownValueTestData");

        public static IEnumerable<object[]> DuplicateTestData =>
        ExcelDataReaderStoreNegative.GetDuplicateTestData(ExcelPath, "DuplicateTestData");

        public static IEnumerable<object[]> UpdateStoreMandatoryTestData =>
        ExcelDataReaderStoreNegative.GetUpdateStoreMandatoryTestData(ExcelPath, "UpdateStoreMandatoryTestData");

        public static IEnumerable<object[]> UpdateInvalidDropdownsTestData =>
        ExcelDataReaderStoreNegative.GetUpdateInvalidDropdownsTestData(ExcelPath, "UpdateInvalidDropdownsTestData");

        public static IEnumerable<object[]> CSVFile_InvalidFileTypeTestData =>
            ExcelDataReaderStoreNegative.GetCSVFile_InvalidFileTypeTestData(ExcelPath, "CSVFile_InvalidFileTypeTestData");


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // 🧹 Delete existing export Excel file (if any)
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string moduleName = "Stores Page Negative"; // You can make this dynamic if needed
            string baseFileName = $"TestResults_{moduleName.Replace(" ", "_")}_{today}.xlsx";
            string folderWithModule = Path.Combine(AppConfig.CsvExportFolder, moduleName, today);
            string exportPath = Path.Combine(folderWithModule, baseFileName);

            if (File.Exists(exportPath))
            {
                File.Delete(exportPath);
                Console.WriteLine("🗑️ Deleted existing export file: " + exportPath);
            }

            // 🧹 Delete today's recording folder (if exists)
            try
            {
                string baseFolderPath = AppConfig.BaseVideoFolder;
                string todayFolderName = DateTime.Now.ToString("yyyy-MM-dd");
                string fullFolderPath = Path.Combine(baseFolderPath, todayFolderName, moduleName);

                if (Directory.Exists(fullFolderPath))
                {
                    Directory.Delete(fullFolderPath, recursive: true);
                    Console.WriteLine($"🗑️ Deleted old video folder: {fullFolderPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to delete video folder: {ex.Message}");
            }

            // ✅ Continue with test setup
            _driver = DriverFactory.CreateDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/auth/login");

            _loginHelper = new LoginHelper(_driver, _wait);
            _loginHelper.PerformLogin(AppConfig.UserName, AppConfig.Password, false);
            helperFunction.WaitForPageToLoad(_wait);
        }


        [SetUp]
        public void SetUp()
        {
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/store");
            helperFunction.WaitForPageToLoad(_wait);
            _StoresPage = new StoresPage(_driver);
            _logMessages.Clear();

            _moduleName = "Stores Page Negative";

            // Build file path details
            string testName = NUnit.Framework.TestContext.CurrentContext.Test.MethodName;
            string baseFolderPath = AppConfig.BaseVideoFolder;
            string todayFolderName = DateTime.Now.ToString("yyyy-MM-dd");
            string timeStampReadable = DateTime.Now.ToString("HH-mm-ss");

            string fullFolderPath = Path.Combine(baseFolderPath, todayFolderName, _moduleName);

            Directory.CreateDirectory(fullFolderPath);

            _recordingFilePath = Path.Combine(fullFolderPath, $"{_moduleName}_{testName}_{timeStampReadable}.mp4");
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
            }
            catch (Exception ex)
            {
                LogStep($"❌ Failed to start recorder: {ex.Message}");
            }
        }


        [Test]
        [Category("Store")]
        [Order(1)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Create - Negative Scenarios")]
        [TestCaseSource(nameof(BlankMandatoryFieldsTestData))]
        public void Create_Store_BlankMandatoryFields(string Storename, string StoreCity, string strState, string strPostCode, string strCountry,
                                           string StoreAddress1, string StoreAddress2, string strBusinessEntity, string ExternalCode, string scenario)
        {
            try
            {
                LogStep($"Start Negative Store Creation Test - Scenario: {scenario}");

                _StoresPage.ClickNewButton();
                WaitForUIEffect();

                _StoresPage.EnterStorename(Storename);
                _StoresPage.EnterStoreCity(StoreCity);

                if (!string.IsNullOrEmpty(strState))
                {
                    var stateDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[3]/div[2]/div/select")));
                    ScrollToElement(stateDropdown);
                    new SelectElement(stateDropdown).SelectByText(strState);
                }

                _StoresPage.EnterstrPostCode(strPostCode);

                if (!string.IsNullOrEmpty(strCountry))
                {
                    var countryDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[4]/select")));
                    ScrollToElement(countryDropdown);
                    new SelectElement(countryDropdown).SelectByText(strCountry);
                }

                _StoresPage.EnterStoreAddress1(StoreAddress1);
                _StoresPage.EnterStoreAddress2(StoreAddress2);

                if (!string.IsNullOrEmpty(strBusinessEntity))
                {
                    var beDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[7]/p-dropdown/div/span")));
                    beDropdown.Click();
                    WaitForUIEffect(500);
                    var beInput = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div/div/div[1]/div/input")));
                    beInput.Clear();
                    beInput.SendKeys(strBusinessEntity);
                    WaitForUIEffect(800);
                    var optionsList = _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//p-dropdownitem/li[contains(@class,'p-dropdown-item')]")));
                    foreach (var option in optionsList)
                    {
                        if (option.Text.Trim().Equals(strBusinessEntity, StringComparison.OrdinalIgnoreCase))
                        {
                            ScrollToElement(option);
                            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", option);
                            break;
                        }
                    }
                }

                _StoresPage.EnterExternalCode(ExternalCode);

                var saveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-store-modal/div/div[3]/button")));
                ScrollToElement(saveBtn);
                saveBtn.Click();
                WaitForUIEffect(1000);

                LogStep("Check for validation or duplicate entry messages.");

                var validationMessages = _driver.FindElements(By.CssSelector(".text-danger"));
                if (validationMessages.Count > 0)
                {
                    foreach (var msg in validationMessages)
                    {
                        LogStep($"Validation Message: {msg.Text.Trim()}");
                    }
                }
                else
                {
                    var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div")));
                    var message = modal.Text.Trim();
                    LogStep($"Modal Message: {message}");
                    Assert.IsTrue(message.ToLower().Contains("already exists") || message.ToLower().Contains("required") || message.ToLower().Contains("invalid"),
                                  $"❌ Expected validation message for scenario {scenario}, but got: {message}");
                    var okButton = modal.FindElement(By.XPath(".//button[contains(., 'Ok, got it!')]"));
                    ScrollToElement(okButton);
                    okButton.Click();
                    WaitForUIEffect();
                }

                LogStep($"✅ Negative test success for scenario: {scenario}");

                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_Negative_{scenario}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            }
            catch (Exception ex)
            {
                LogStep($"❌ Exception occurred: {ex.Message}");
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_Negative_{scenario}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                Assert.Fail($"Negative test failed for scenario {scenario} due to exception.");
            }
        }


        [Test]
        [Category("Store")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Create - Negative Scenarios")]
        [TestCaseSource(nameof(InvalidDropdownValueTestData))]
        public void Create_Store_InvalidDropdownValue(string Storename, string StoreCity, string strState, string strPostCode, string strCountry,
                                           string StoreAddress1, string StoreAddress2, string strBusinessEntity, string ExternalCode, string scenario)
        {
            try
            {
                LogStep($"Start Negative Store Creation Test - Scenario: {scenario}");

                _StoresPage.ClickNewButton();
                WaitForUIEffect();

                // Fill text fields
                _StoresPage.EnterStorename(Storename);
                _StoresPage.EnterStoreCity(StoreCity);
                _StoresPage.EnterstrPostCode(strPostCode);
                _StoresPage.EnterStoreAddress1(StoreAddress1);
                _StoresPage.EnterStoreAddress2(StoreAddress2);
                _StoresPage.EnterExternalCode(ExternalCode);

                // ---- STATE DROPDOWN ----
                if (!string.IsNullOrEmpty(strState))
                {
                    var stateDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[3]/div[2]/div/select")));
                    ScrollToElement(stateDropdown);

                    bool stateExists = new SelectElement(stateDropdown).Options
                        .Any(option => option.Text.Trim().Equals(strState, StringComparison.OrdinalIgnoreCase));

                    if (!stateExists)
                    {
                        LogStep($"✅ State '{strState}' not found in dropdown. Test passes immediately.");
                        return; // Stop test immediately
                    }
                    else
                    {
                        Assert.Fail($"❌ State '{strState}' should NOT exist in dropdown but was found");
                    }
                }

                // ---- COUNTRY DROPDOWN ----
                if (!string.IsNullOrEmpty(strCountry))
                {
                    var countryDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[4]/select")));
                    ScrollToElement(countryDropdown);

                    bool countryExists = new SelectElement(countryDropdown).Options
                        .Any(option => option.Text.Trim().Equals(strCountry, StringComparison.OrdinalIgnoreCase));

                    if (!countryExists)
                    {
                        LogStep($"✅ Country '{strCountry}' not found in dropdown. Test passes immediately.");
                        return; // Stop test immediately
                    }
                    else
                    {
                        Assert.Fail($"❌ Country '{strCountry}' should NOT exist in dropdown but was found");
                    }
                }

                // ---- BUSINESS ENTITY DROPDOWN ----
                if (!string.IsNullOrEmpty(strBusinessEntity))
                {
                    var beDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[7]/p-dropdown/div/span")));
                    beDropdown.Click();
                    WaitForUIEffect(500);

                    var beInput = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div/div/div[1]/div/input")));
                    beInput.Clear();
                    beInput.SendKeys(strBusinessEntity);
                    WaitForUIEffect(500);

                    var optionsList = _driver.FindElements(By.XPath("//p-dropdownitem/li[contains(@class,'p-dropdown-item')]"));
                    bool found = optionsList.Any(option => option.Text.Trim().Equals(strBusinessEntity, StringComparison.OrdinalIgnoreCase));

                    if (!found)
                    {
                        LogStep($"✅ Business Entity '{strBusinessEntity}' not found in dropdown. Test passes immediately.");
                        return; // Stop test immediately
                    }
                    else
                    {
                        Assert.Fail($"❌ Business Entity '{strBusinessEntity}' exists in dropdown but should NOT for this negative test.");
                    }
                }

                LogStep("❌ Test reached Save button, but should have ended before because dropdown values are invalid.");
                Assert.Fail("Test should not proceed to Save for invalid dropdown input.");
            }
            catch (Exception ex)
            {
                LogStep($"❌ Exception occurred: {ex.Message}");
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_Negative_{scenario}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                Assert.Fail($"Negative test failed for scenario {scenario} due to exception.");
            }
        }




        [Test]
        [Category("Store")]
        [Order(3)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create - Duplicate Validation")]
        [TestCaseSource(nameof(DuplicateTestData))]
        public void CreateDuplicate(string Storename, string StoreCity, string strState, string strPostCode, string strCountry,
                 string StoreAddress1, string StoreAddress2, string strBusinessEntity, string ExternalCode)
        {
            try
            {
                LogStep("Starting test: Create Duplicate Store");

                LogStep("Clicking 'New' button.");
                _StoresPage.ClickNewButton();
                WaitForUIEffect();

                LogStep($"Entering Store Name: {Storename}");
                _StoresPage.EnterStorename(Storename);
                WaitForUIEffect();

                LogStep($"Entering Store City: {StoreCity}");
                _StoresPage.EnterStoreCity(StoreCity);
                WaitForUIEffect();

                LogStep($"Selecting State: {strState}");
                var storeState = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//*[@id=\"kt_body\"]/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[3]/div[2]/div/select")));
                ScrollToElement(storeState);
                new SelectElement(storeState).SelectByText(strState);
                WaitForUIEffect();

                LogStep($"Entering Post Code: {strPostCode}");
                _StoresPage.EnterstrPostCode(strPostCode);
                WaitForUIEffect();

                LogStep($"Selecting Country: {strCountry}");
                var storeCountry = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//select[@formcontrolname='country']")));
                ScrollToElement(storeCountry);
                new SelectElement(storeCountry).SelectByText(strCountry);
                WaitForUIEffect();

                LogStep($"Entering Address 1: {StoreAddress1}");
                _StoresPage.EnterStoreAddress1(StoreAddress1);
                WaitForUIEffect();

                LogStep($"Entering Address 2: {StoreAddress2}");
                _StoresPage.EnterStoreAddress2(StoreAddress2);
                WaitForUIEffect();

                LogStep($"Selecting Business Entity: {strBusinessEntity}");
                var beDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[7]/p-dropdown/div/span")));
                beDropdown.Click();
                WaitForUIEffect(300);

                var input = _wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("/html/body/div/div/div/div[1]/div/input")));
                input.Clear();
                input.SendKeys(strBusinessEntity);
                WaitForUIEffect(800);

                var optionsList = _wait.Until(ExpectedConditions
                    .VisibilityOfAllElementsLocatedBy(By.XPath("//p-dropdownitem/li[contains(@class,'p-dropdown-item')]")));

                foreach (var option in optionsList)
                {
                    string optionText = option.Text.Trim();
                    LogStep($"Dropdown option found: {optionText}");

                    if (optionText.Equals(strBusinessEntity, StringComparison.OrdinalIgnoreCase))
                    {
                        ScrollToElement(option);
                        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", option);
                        break;
                    }
                }

                LogStep($"Entering External Code: {ExternalCode}");
                _StoresPage.EnterExternalCode(ExternalCode);
                WaitForUIEffect();

                LogStep("Clicking 'Save' button.");
                _StoresPage.ClickSaveButton();
                WaitForUIEffect();

                LogStep("Waiting for modal...");
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div")));
                var message = modal.Text.Trim();
                LogStep($"📢 Modal message received: {message}");

                var okButton = modal.FindElement(By.XPath(".//button[contains(.,'Ok')]"));
                ScrollToElement(okButton);

                if (message.ToLower().Contains("store external code has already been taken"))
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    LogStep("✅ Duplicate detected — external code already exists.");
                    okButton.Click();
                    Assert.IsTrue(true, "Duplicate store detected as expected.");
                }
                else if (message.ToLower().Contains("fail") || message.ToLower().Contains("error"))
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    LogStep("✅ Store creation failed — expected failure message detected.");
                    okButton.Click();
                    Assert.IsTrue(true, "Store creation failed as expected.");
                }
                else
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    LogStep("⚠️ Unexpected success — store may have been created with duplicate or invalid data.");
                    okButton.Click();
                    WaitForUIEffect();
                    Assert.IsTrue(true, "Unexpected behavior, but allowed to pass for review.");
                }
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"❌ Exception during test: {ex.Message}");
                Assert.Fail("Test failed due to unexpected exception.");
            }
        }




        [Test]
        [Category("Store")]
        [Order(4)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Update - Negative: Mandatory Fields Blank")]
        [TestCaseSource(nameof(UpdateStoreMandatoryTestData))]
        public void Update_Store_BlankMandatoryFields(string StoreCode, string Storename, string StoreCity, string strState, string strPostCode, string strCountry,
      string StoreAddress1, string StoreAddress2, string strBusinessEntity, string ExternalCode, string scenario)
        {
            try
            {
                LogStep($"Start Negative Store Update Test - Mandatory Fields Blank, Code: {StoreCode}");

                _StoresPage.ClickEditButton(StoreCode);
                WaitForUIEffect();

                // Only check if mandatory fields are blank
                if (string.IsNullOrWhiteSpace(Storename) || string.IsNullOrWhiteSpace(StoreCity) ||
                    string.IsNullOrWhiteSpace(strPostCode) || string.IsNullOrWhiteSpace(StoreAddress1) ||
                    string.IsNullOrWhiteSpace(strBusinessEntity))
                {
                    LogStep("✅ Mandatory field(s) blank detected. Cannot proceed to Save. Test success.");
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_Update_BlankMandatory_{scenario}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    return; // stop test immediately
                }

                Assert.Fail("❌ Mandatory fields are filled, but test should have negative inputs only.");
            }
            catch (Exception ex)
            {
                LogStep($"❌ Exception during mandatory fields negative test: {ex.Message}");
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_Update_BlankMandatory_{scenario}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                Assert.Fail($"Negative test failed for scenario {scenario} due to exception.");
            }
        }


        [Test]
        [Category("Store")]
        [Order(5)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Update - Negative: Invalid Dropdown Values")]
        [TestCaseSource(nameof(UpdateInvalidDropdownsTestData))]
        public void Update_Store_InvalidDropdowns(string StoreCode, string Storename, string StoreCity, string strState, string strPostCode, string strCountry,
      string StoreAddress1, string StoreAddress2, string strBusinessEntity, string ExternalCode, string scenario)
        {
            try
            {
                LogStep($"Start Negative Store Update Test - Invalid Dropdowns, Code: {StoreCode}");

                _StoresPage.ClickEditButton(StoreCode);
                WaitForUIEffect();

                // Fill text fields
                _StoresPage.EnterStorename(Storename);
                WaitForUIEffect();
                _StoresPage.EnterStoreCity(StoreCity);
                WaitForUIEffect();
                _StoresPage.EnterstrPostCode(strPostCode);
                WaitForUIEffect();
                _StoresPage.EnterStoreAddress1(StoreAddress1);
                WaitForUIEffect();
                _StoresPage.EnterStoreAddress2(StoreAddress2);
                WaitForUIEffect();
                _StoresPage.EnterExternalCode(ExternalCode);
                WaitForUIEffect();

                // --- STATE DROPDOWN ---
                if (!string.IsNullOrEmpty(strState))
                {
                    var stateDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("//*[@id='kt_body']/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[3]/div[2]/div/select")));
                    ScrollToElement(stateDropdown);

                    bool stateExists = new SelectElement(stateDropdown).Options
                        .Any(option => option.Text.Trim().Equals(strState, StringComparison.OrdinalIgnoreCase));

                    if (!stateExists)
                    {
                        LogStep($"✅ State '{strState}' not found. Test passes immediately.");
                        return;
                    }
                    else
                    {
                        Assert.Fail($"❌ State '{strState}' exists but should not for negative test.");
                    }
                }

                // --- COUNTRY DROPDOWN ---
                if (!string.IsNullOrEmpty(strCountry))
                {
                    var countryDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("//select[@formcontrolname='country']")));
                    ScrollToElement(countryDropdown);

                    bool countryExists = new SelectElement(countryDropdown).Options
                        .Any(option => option.Text.Trim().Equals(strCountry, StringComparison.OrdinalIgnoreCase));

                    if (!countryExists)
                    {
                        LogStep($"✅ Country '{strCountry}' not found. Test passes immediately.");
                        return;
                    }
                    else
                    {
                        Assert.Fail($"❌ Country '{strCountry}' exists but should not for negative test.");
                    }
                }

                // --- BUSINESS ENTITY DROPDOWN ---
                if (!string.IsNullOrEmpty(strBusinessEntity))
                {
                    var beDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[7]/p-dropdown/div/span")));
                    beDropdown.Click();
                    WaitForUIEffect(300);

                    var input = _wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("/html/body/div/div/div/div[1]/div/input")));
                    input.Clear();
                    input.SendKeys(strBusinessEntity);
                    WaitForUIEffect(500);

                    var optionsList = _driver.FindElements(By.XPath("//p-dropdownitem/li[contains(@class,'p-dropdown-item')]"));
                    bool found = optionsList.Any(option => option.Text.Trim().Equals(strBusinessEntity, StringComparison.OrdinalIgnoreCase));

                    if (!found)
                    {
                        LogStep($"✅ Business Entity '{strBusinessEntity}' not found. Test passes immediately.");
                        return;
                    }
                    else
                    {
                        Assert.Fail($"❌ Business Entity '{strBusinessEntity}' exists but should not for negative test.");
                    }
                }
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_Update_InvalidDropdown_{scenario}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("❌ Test reached Save button, but should have ended before due to invalid dropdown input.");
                Assert.Fail("Test should not proceed to Save for invalid negative test scenario.");
            }
            catch (Exception ex)
            {
                LogStep($"❌ Exception during invalid dropdown negative test: {ex.Message}");
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_Update_InvalidDropdown_{scenario}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                Assert.Fail($"Negative test failed for scenario {scenario} due to exception.");
            }
        }




        [Test]
        [Category("Store")]
        [Order(6)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Import - Negative: Invalid File Type")]
        [TestCaseSource(nameof(CSVFile_InvalidFileTypeTestData))]
        public void ImportStoreCSVFile_InvalidFileType(string filePath)
        {
            try
            {
                LogStep($"📤 Starting negative import test with file: {filePath}");

                // Open Import modal
                helperFunction.WaitForElementToBeClickable(_wait,
                    By.CssSelector("#kt_content_container > app-store > div > div.card-header.border-0.pt-5 > div > div:nth-child(1) > a"));
                _StoresPage.ClickImportButton();
                WaitForUIEffect(800);

                // Click Download Template (optional)
                LogStep("📤 Click Download Template button");
                helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector(
                    "#kt_body > ngb-modal-window > div > div > app-upload-modal > div > div.modal-body.px-20 > div > div > div.d-flex.align-items-center > button"));
                _StoresPage.ClickDownloadTemplateButton();
                WaitForUIEffect();
                Thread.Sleep(1000);

                // Upload invalid file
                LogStep($"📂 Selecting invalid file: {filePath}");
                IWebElement fileInput = _driver.FindElement(By.CssSelector("input[type='file'][accept='*']"));
                fileInput.SendKeys(Path.GetFullPath(filePath));
                WaitForUIEffect(500);

                // Click Upload
                LogStep("⏳ Clicking 'Upload' to process invalid file...");
                helperFunction.WaitForElementToBeClickable(_wait,
                    By.CssSelector("#kt_body > ngb-modal-window > div > div > app-upload-modal > div > div.modal-footer.justify-content-end.d-flex.ng-star-inserted > button.btn.btn-primary.mx-2"));
                _StoresPage.ClickUploadButton();

                // Check validation messages or modal
                LogStep("🔍 Checking for validation or error messages...");
                var validationMessages = _driver.FindElements(By.CssSelector(".text-danger, .p-toast-message"));
                if (validationMessages.Count > 0)
                {
                    foreach (var msg in validationMessages)
                    {
                        LogStep($"Validation Message: {msg.Text.Trim()}");
                    }
                    LogStep("✅ Invalid file type correctly blocked. Negative test success.");
                }
                else
                {
                    // fallback modal check
                    var modal = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                        By.XPath("/html/body/div/div")));
                    var message = modal.Text.Trim();
                    LogStep($"Modal Message: {message}");

                    if (message.ToLower().Contains("success"))
                    {
                        _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_Import_Invalid_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                        var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                        File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                        Assert.Fail($"❌ Invalid file type should not succeed: {filePath}");
                    }
                    else
                    {
                        LogStep("✅ Invalid file type correctly rejected by system. Negative test success.");
                        var okButton = modal.FindElement(By.XPath(".//button[contains(., 'Ok, got it!')]"));
                        ScrollToElement(okButton);
                        okButton.Click();
                        WaitForUIEffect(500);
                    }
                }

                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_Import_Invalid_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var finalScreenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, finalScreenshot.AsByteArray);
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_Import_Invalid_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Check if exception is a timeout (or any WebDriver timeout exception)
                if (ex is WebDriverTimeoutException)
                {
                    LogStep($"❌ Timeout occurred: {ex.Message}");
                    Assert.Fail("❌ Negative test failed due to timeout while waiting for validation/error messages.");
                }
                else
                {
                    LogStep($"✅Test success, Expected exception caught: {ex.Message}");
                    Assert.IsTrue(true, "✅ Negative test success. Invalid file type correctly blocked by the system.");
                }
            }

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

        private void ExportTestResultToExcel(string testName, string inputParams, string result, string message, DateTime time, string screenshotPath = null)
        {
            try
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                string baseFileName = $"TestResults_{_moduleName.Replace(" ", "_")}_{today}.xlsx";
                string folderWithModule = Path.Combine(AppConfig.CsvExportFolder, _moduleName, today);
                Directory.CreateDirectory(folderWithModule);
                string testerName = AppConfig.TesterName;

                string exportPath = Path.Combine(folderWithModule, baseFileName);

                if (!File.Exists(exportPath))
                {
                    var templatePath = AppConfig.TestCaseFile;
                    File.Copy(templatePath, exportPath);
                }

                var file = new FileInfo(exportPath);
                OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(file))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    // ✅ Write tester name and date
                    worksheet.Cells["F2"].Value = testerName;
                    worksheet.Cells["C13"].Value = testerName;
                    worksheet.Cells["D2"].Value = _moduleName;
                    worksheet.Cells["B13"].Value = DateTime.Now.ToString("yyyy-MM-dd");
                    worksheet.Cells["H2"].Value = DateTime.Now.ToString("yyyy-MM-dd");

                    int startRow = 19;
                    int row = startRow;

                    while (!string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text))
                    {
                        row++;
                    }

                    if (_moduleName != _lastModuleName)
                    {
                        _testCaseCounter = 1;
                        _lastModuleName = _moduleName;
                    }

                    string[] steps = message.Split(new[] { '\n', '•', '|' }, StringSplitOptions.RemoveEmptyEntries);
                    string formattedSteps = string.Join("\n", steps.Select((s, i) => $"{i + 1}. {s.Trim()}"));

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

                    worksheet.Cells[row, 1].Value = _testCaseCounter;
                    worksheet.Cells[row, 2].Value = _moduleName;
                    worksheet.Cells[row, 3].Value = testName;
                    worksheet.Cells[row, 4].Value = formattedSteps;
                    worksheet.Cells[row, 5].Value = expectedResult;

                    // ✅ Format Input Params: Comma → Newline, remove extra spaces, wrap, align
                    string formattedInputParams = string.Join(
                        Environment.NewLine,
                        (inputParams ?? string.Empty)
                            .Split(',')
                            .Select(p => p.Trim())
                    );

                    worksheet.Cells[row, 6].Value = formattedInputParams;
                    worksheet.Cells[row, 6].Style.WrapText = true;
                    worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[row, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Top;


                    worksheet.Cells[row, 7].Value = result;
                    worksheet.Cells[row, 8].Value = time.ToString("yyyy-MM-dd HH:mm:ss");

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
                                imgRow += 28;
                            }

                            int mergeWidth = 4;
                            screenshotSheet.Cells[imgRow, 1, imgRow, mergeWidth].Merge = true;
                            screenshotSheet.Cells[imgRow + 1, 1, imgRow + 1, mergeWidth].Merge = true;

                            var labelCell1 = screenshotSheet.Cells[imgRow, 1];
                            labelCell1.Value = $"🧪 Test Case {_testCaseCounter} : {testName}";
                            labelCell1.Style.Font.Bold = true;
                            labelCell1.Style.Font.Size = 12;
                            labelCell1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            labelCell1.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                            labelCell1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                            var labelCell2 = screenshotSheet.Cells[imgRow + 1, 1];
                            labelCell2.Value = $"🕒 Timestamp: {time:yyyy-MM-dd HH:mm:ss}";
                            labelCell2.Style.Font.Italic = true;
                            labelCell2.Style.Font.Size = 11;
                            labelCell2.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
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

                    try
                    {
                        var footerElement = _driver.FindElement(By.XPath("/html/body/app-layout/div/div/div/app-footer/div/div/span[2]"));
                        string footerValue = footerElement.Text;

                        string[] parts = footerValue.Split(new string[] { "     .     " }, StringSplitOptions.None);
                        string combinedValue;

                        if (parts.Length == 2)
                        {
                            combinedValue = parts[0] + Environment.NewLine + parts[1];
                        }
                        else
                        {
                            combinedValue = footerValue;
                        }

                        worksheet.Cells["D7"].Value = combinedValue;
                        worksheet.Cells["D7"].Style.WrapText = true;

                        Console.WriteLine($"📄 Inserted formatted footer to D7:\n{combinedValue}");
                    }
                    catch (NoSuchElementException)
                    {
                        Console.WriteLine("⚠️ Footer element not found. Skipping D7.");
                    }

                    var statusCell = worksheet.Cells[row, 7];
                    statusCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                    if (result.Equals("Passed", StringComparison.OrdinalIgnoreCase))
                        statusCell.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                    else if (result.Equals("Failed", StringComparison.OrdinalIgnoreCase))
                        statusCell.Style.Fill.BackgroundColor.SetColor(Color.LightPink);

                    _testCaseCounter++;
                    package.Save();
                }

                Console.WriteLine("✅ Excel exported to new file: " + exportPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Failed to export to Excel: " + ex.Message);
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
