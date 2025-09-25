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
using SeleniumTests.Pages.Store;
using System.Drawing;
using System.Globalization;
using System.Media;
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
                    string Storename = worksheet.Cells[row, 2].Text?.Trim();
                    string StoreCity = worksheet.Cells[row, 3].Text?.Trim();
                    string strState = worksheet.Cells[row, 4].Text?.Trim();
                    string strPostCode = worksheet.Cells[row, 5].Text?.Trim();
                    string strCountry = worksheet.Cells[row, 6].Text?.Trim();
                    string StoreAddress1 = worksheet.Cells[row, 7].Text?.Trim();
                    string StoreAddress2 = worksheet.Cells[row, 8].Text?.Trim();
                    string strBusinessEntity = worksheet.Cells[row, 9].Text?.Trim();
                    string ExternalCode = worksheet.Cells[row, 10].Text?.Trim();

                    yield return new object[]
                    {
                        StoreCode, Storename, StoreCity, strState, strPostCode, strCountry, StoreAddress1, StoreAddress2, strBusinessEntity, ExternalCode
                    };

                }
            }
        }

        public static IEnumerable<object[]> GetFilterCategoryTestData(string filePath, string sheetName)
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
                    string category = worksheet.Cells[row, 1].Text?.Trim();


                    yield return new object[]
                    {
                        category
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

        public static IEnumerable<object[]> UpdateStoreTestData =>
        ExcelDataReaderStoreValid.GetUpdateStoreTestData(ExcelPath, "UpdateStoreTestData");

        public static IEnumerable<object[]> FilterCategoryTestData =>
        ExcelDataReaderStoreValid.GetFilterCategoryTestData(ExcelPath, "FilterCategoryTestData");



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
                _StorePage.ClickNewButton();
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
                Assert.Fail($"Test failed due to exception: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}");
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
                Assert.Fail($"Test failed due to exception: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}");
            }
        }




        [Test]
        [Category("Store")]
        [Order(3)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Search - General Match (Partial Match Accepted)")]
        [TestCaseSource(nameof(SearchStoreGroupTestData))]
        public void Search_Store(string searchText)
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
                    worksheet.Cells["F2"].Value = testerName;
                    worksheet.Cells["C13"].Value = testerName;
                    worksheet.Cells["D2"].Value = _moduleName;
                    worksheet.Cells["D7"].Value = _footerValue;
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
                    string[] steps = message.Split(new[] { '\n', '-', '•', '|' }, StringSplitOptions.RemoveEmptyEntries);
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
                                if (lower.Contains("successfully") || lower.Contains("saved") || lower.Contains("updated") ||
                                    lower.Contains("created") || lower.Contains("deleted") || lower.Contains("download") ||
                                    lower.Contains("completed") || lower.Contains("match found"))
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
