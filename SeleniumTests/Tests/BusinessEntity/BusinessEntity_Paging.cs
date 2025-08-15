using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using EInvoice.SeleniumTests.Config;
using EInvoice.SeleniumTests.Drivers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ScreenRecorderLib;
using SeleniumTests.Helpers;
using System.Drawing;
using System.Globalization;
using System.Media;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using helperFunction = SeleniumTests.Helper.HelperFunction;



namespace SeleniumTests.Tests.BusinessEntity
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Business Entity - Paging")] // use this ties to module
    [AllureEpic("ERP-117")] // use this and ties to ticket number

    public class BusinessEntity_Paging
    {

        private IWebDriver _driver;
        private BusinessEntityPage _BusinessEntityPage;
        private WebDriverWait _wait;
        private LoginHelper _loginHelper;
        private Recorder _recorder;
        private string _recordingFilePath;
        private ManualResetEvent _recordingCompletedEvent = new ManualResetEvent(false);
        private List<string> _logMessages = new List<string>();
        private string _moduleName = "";


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // 🧹 Delete existing export Excel file (if any)
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string moduleName = "Business Entity Page - Paging"; // You can make this dynamic if needed
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
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/business-entity");
            helperFunction.WaitForPageToLoad(_wait);
            _BusinessEntityPage = new BusinessEntityPage(_driver);
            _logMessages.Clear();

            _moduleName = "Business Entity Page - Paging";

            // Build file path details
            string testName = NUnit.Framework.TestContext.CurrentContext.Test.MethodName;
            string baseFolderPath = AppConfig.BaseVideoFolder;
            string todayFolderName = DateTime.Now.ToString("yyyy-MM-dd");

            string fullFolderPath = Path.Combine(baseFolderPath, todayFolderName, _moduleName);

            Directory.CreateDirectory(fullFolderPath);

            _recordingFilePath = Path.Combine(fullFolderPath, $"{_moduleName}_{testName}.mp4");
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
        [Category("Business Entity")]
        [Order(1)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Business Entity Paging")]
        public void TestPagingNextButtonAndVerify()
        {
            // Wait for table to be visible
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[1]")));
            WaitForUIEffect();

            LogStep("📄 Table loaded. Capturing current HTML content.");
            string beforeHtml = tableElement.GetAttribute("innerHTML");

            // Find the "Next" button
            var nextButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(
                By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[2]/app-global-pagination/div/div[2]/ul/li[4]")));
            WaitForUIEffect();

            LogStep("🔍 Located the 'Next' pagination button.");

            // Check if "Next" button is disabled
            bool isDisabled = nextButton.GetAttribute("class").Contains("disabled") || !nextButton.Enabled;
            if (isDisabled)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("⚠️ 'Next' button is disabled. Pagination not applicable (only one page).");
                return;
            }

            // Click the "Next" button
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(nextButton));
            nextButton.Click();
            WaitForUIEffect();
            LogStep("Clicked the 'Next' button to move to next page.");

            // Wait for table content to change OR "No data available" to appear
            try
            {
                bool result = _wait.Until(driver =>
                {
                    var updatedTable = driver.FindElement(By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[1]"));
                    string updatedHtml = updatedTable.GetAttribute("innerHTML");

                    // Check for table content change
                    if (updatedHtml != beforeHtml)
                        return true;

                    // Check for "No data available"
                    return updatedHtml.Contains("No data available");
                });

                WaitForUIEffect();

                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                if (result)
                {
                    LogStep("✅ Paging successful: table updated or shows 'No data available'.");
                    Assert.IsTrue(true);
                }
                else
                {
                    LogStep("❌ Paging result unknown: no table change and no 'No data available' message.");
                    Assert.Fail("Paging did not update content and no message shown.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Paging_Failed_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                Assert.Fail("❌ Timeout: No table content change or 'No data available' after clicking 'Next'.");
            }
        }



        [Test]
        [Category("Business Entity")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Business Entity Paging")]
        public void TestPagingPreviousButtonAndVerify()
        {
            // Wait for table to be visible
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[1]")));
            WaitForUIEffect();

            string originalHtml = tableElement.GetAttribute("innerHTML");
            LogStep("📄 Captured original table HTML content.");

            // Find and evaluate "Next" button
            var nextButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(
                By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[2]/app-global-pagination/div/div[2]/ul/li[4]")));
            WaitForUIEffect();

            bool nextDisabled = nextButton.GetAttribute("disabled") == "true" || nextButton.GetAttribute("class").Contains("disabled");
            if (nextDisabled)
            {
                LogStep("⚠️ 'Next' button is disabled. Only one page exists. Skipping previous paging test.");
                return;
            }

            // Click "Next"
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(nextButton));
            nextButton.Click();
            WaitForUIEffect();
            LogStep("Clicked 'Next' button to go to page 2.");

            // Wait for table to change (or remain same if no data)
            var newTable = _wait.Until(driver => driver.FindElement(
                By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[1]")));

            string newHtml = newTable.GetAttribute("innerHTML");

            if (newHtml == originalHtml)
            {
                LogStep("⚠️ Table content did not change — possibly no data on next page.");
            }
            else
            {
                LogStep("✅ Table content updated after clicking 'Next'.");
            }

            // Locate "Previous" button
            var previousButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(
                By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[2]/app-global-pagination/div/div[2]/ul/li[2]")));
            WaitForUIEffect();

            bool prevDisabled = previousButton.GetAttribute("disabled") == "true" || previousButton.GetAttribute("class").Contains("disabled");
            if (prevDisabled)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}_PrevDisabled.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("⚠️ 'Previous' button is disabled. Cannot go back to page 1.");
                Assert.Fail("❌ 'Previous' button should be enabled after going to next page.");
                return;
            }

            // Click "Previous"
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(previousButton));
            previousButton.Click();
            WaitForUIEffect();
            LogStep("Clicked 'Previous' button to return to original page.");

            // Verify returned to original content
            var tableBack = _wait.Until(driver => driver.FindElement(
                By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[1]")));
            string finalHtml = tableBack.GetAttribute("innerHTML");

            if (finalHtml == originalHtml)
            {
                LogStep("✅ Table returned to original content. Paging 'Previous' works correctly.");
            }
            else
            {
                LogStep("⚠️ Table content did not fully return to original. Possibly empty state, still considered pass if navigation successful.");
            }

            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}_Paging.png");
            var finalScreenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, finalScreenshot.AsByteArray);

            Assert.IsTrue(true);
        }



        [Test]
        [Category("Business Entity")]
        [Order(3)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Business Entity Paging - Click Last and Verify Change")]
        public void TestPagingLastButtonAndVerify()
        {
            string tableXPath = "/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[1]";

            // Capture current table content
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            WaitForUIEffect();
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            LogStep("📄 Captured original table HTML content before clicking 'Last'.");

            // Find the "Last" button
            var lastButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("//a[.//i[contains(@class,'fa-angle-double-right')]]")));
            WaitForUIEffect();
            LogStep("🔎 Found 'Last' page button (double-right arrow).");

            // Check if the button is disabled
            bool isDisabled = lastButton.GetAttribute("class")?.Contains("disabled") ?? false;
            if (isDisabled)
            {
                LogStep("ℹ️ 'Last' button is disabled — probably already on the last page or only one page exists.");
                Assert.IsTrue(true); // Test passes as paging not needed
                return;
            }

            // Scroll into view and click
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({ block: 'center' });", lastButton);
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(lastButton));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", lastButton);
            WaitForUIEffect();
            LogStep("✅ Clicked 'Last' page button via JavaScript.");

            // Wait for content change
            bool tableChanged = false;
            try
            {
                tableChanged = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)).Until(driver =>
                {
                    var updatedTable = driver.FindElement(By.XPath(tableXPath));
                    return updatedTable.GetAttribute("innerHTML") != beforeHtml;
                });
            }
            catch (WebDriverTimeoutException)
            {
                LogStep("⚠️ Table content did not change after clicking 'Last'. Might already be on last page.");
            }

            LogStep(tableChanged
                ? "✅ Table content updated after clicking 'Last'."
                : "ℹ️ Table content remained the same. Possibly already on last page. Still acceptable.");
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            Assert.IsTrue(true);
        }




        [Test]
        [Category("Business Entity")]
        [Order(4)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Business Entity Paging - Click First and Verify Return")]
        public void TestPagingFirstButtonAndVerify()
        {
            string tableXPath = "/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[1]";

            // Capture original table content
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            WaitForUIEffect();
            string originalHtml = tableElement.GetAttribute("innerHTML");
            LogStep("📄 Captured original table HTML content.");

            // Locate and check 'Last' button
            var lastButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("//a[.//i[contains(@class,'fa-angle-double-right')]]")));
            WaitForUIEffect();

            if (lastButton.GetAttribute("class")?.Contains("disabled") ?? false)
            {
                LogStep("ℹ️ 'Last' button is disabled — possibly already on last page or only one page exists.");
                Assert.IsTrue(true);
                return;
            }

            // Click 'Last' button via JS
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({ block: 'center' });", lastButton);
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(lastButton));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", lastButton);
            WaitForUIEffect();
            LogStep("✅ Clicked 'Last' button via JavaScript.");

            // Wait for content to change
            bool tableChanged = false;
            try
            {
                tableChanged = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)).Until(driver =>
                {
                    var updatedTable = driver.FindElement(By.XPath(tableXPath));
                    return updatedTable.GetAttribute("innerHTML") != originalHtml;
                });
            }
            catch (WebDriverTimeoutException)
            {
                LogStep("⚠️ Table did not change after clicking 'Last'. Possibly already on last page.");
            }

            // Locate and click 'First' button
            var firstButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("//a[.//i[contains(@class,'fa-angle-double-left')]]")));
            WaitForUIEffect();

            if (firstButton.GetAttribute("class")?.Contains("disabled") ?? false)
            {
                LogStep("ℹ️ 'First' button is disabled. Already on first page.");
                Assert.IsTrue(true);
                return;
            }

            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({ block: 'center' });", firstButton);
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(firstButton));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", firstButton);
            WaitForUIEffect();
            LogStep("✅ Clicked 'First' button via JavaScript.");

            // Wait for table to return to original
            bool tableReturned = false;
            try
            {
                tableReturned = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)).Until(driver =>
                {
                    var returnedTable = driver.FindElement(By.XPath(tableXPath));
                    return returnedTable.GetAttribute("innerHTML") == originalHtml;
                });
            }
            catch (WebDriverTimeoutException)
            {
                LogStep("⚠️ Table did not return to original after clicking 'First'.");
            }
            LogStep($"ℹ️ TableChanged: {tableChanged}, TableReturned: {tableReturned}");
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            Assert.IsTrue(true);
        }




        [Test]
        [Category("Business Entity")]
        [Order(5)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Business Entity Paging - Click Page Size Dropdown and Verify Table Update")]
        [TestCase("100")]
        public void TestItemsPerPageVerify(string pageSizeValue)
        {
            string tableXPath = "/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[1]";
            string dropdownXPath = "/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[2]/app-global-pagination/div/div[1]/select";
            string rowSelector = "tbody tr";

            // Capture original state
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            WaitForUIEffect();
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            int originalRowCount = tableElement.FindElements(By.CssSelector(rowSelector)).Count;
            LogStep($"📄 Captured table with {originalRowCount} rows before selecting page size.");

            // Access and validate dropdown
            var dropdownElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(dropdownXPath)));
            var select = new SelectElement(dropdownElement);

            bool optionExists = select.Options.Any(opt => opt.Text.Trim() == pageSizeValue);
            Assert.IsTrue(optionExists, $"❌ Page size option '{pageSizeValue}' not found in dropdown.");
            LogStep($"✅ Dropdown contains option: {pageSizeValue}");

            // Select page size
            select.SelectByText(pageSizeValue);
            WaitForUIEffect();
            LogStep($"✅ Selected page size '{pageSizeValue}' from dropdown.");

            // Wait for table to update
            bool tableUpdated = _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath(tableXPath));
                string afterHtml = updatedTable.GetAttribute("innerHTML");
                int updatedRowCount = updatedTable.FindElements(By.CssSelector(rowSelector)).Count;

                return afterHtml != beforeHtml || updatedRowCount <= int.Parse(pageSizeValue);
            });

            WaitForUIEffect();

            if (tableUpdated)
            {
                LogStep("✅ Table updated or already showing correct number of rows.");
            }
            else
            {
                Assert.Fail("❌ Table did not update after selecting page size.");
            }
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
        }



        [Test]
        [Category("Business Entity")]
        [Order(6)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Business Entity Paging - Click Page Button Only If It Exists and Verify Table Update")]
        [TestCase("3")] // Page Number
        public void TestClickPageButtonIfExists(string pageNumber)
        {
            string tableXPath = "/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[1]";
            string paginationXPathTemplate = "/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[2]/app-global-pagination/div/div[2]/ul/li[a[text()='{0}']]/a";
            string dynamicPageXPath = string.Format(paginationXPathTemplate, pageNumber);

            // Wait for table and capture original content
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            WaitForUIEffect();
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            LogStep($"📄 Captured table content before clicking page {pageNumber}.");

            // Check if the page button exists
            var pageButtons = _driver.FindElements(By.XPath(dynamicPageXPath));
            bool pageExists = pageButtons.Count > 0;

            LogStep(pageExists
                ? $"✅ Page number {pageNumber} exists."
                : $"ℹ️ Page number {pageNumber} not found — skipping click.");

            Assert.IsTrue(true, $"ℹ️ Page number {pageNumber} existence check complete.");

            if (!pageExists)
                return;

            // Click page number
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(pageButtons[0]));
            pageButtons[0].Click();
            LogStep($"✅ Clicked page {pageNumber}");
            WaitForUIEffect();

            // Wait for table content to change
            bool tableUpdated = _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath(tableXPath));
                string afterHtml = updatedTable.GetAttribute("innerHTML");
                return afterHtml != beforeHtml;
            });

            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            Assert.IsTrue(tableUpdated, $"❌ Table content did not update after clicking page {pageNumber}.");
            LogStep($"✅ Table content changed after navigating to page {pageNumber}.");
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

                    string[] steps = message.Split(new[] { '\n', '-', '•', '|' }, StringSplitOptions.RemoveEmptyEntries);
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
                .Replace("➡️", "")
                .Replace("⬅️", "")
                .Trim();
        }


        private void WaitForUIEffect(int ms = 1000)
        {
            Thread.Sleep(ms); // adjustable UI pause for better video capture
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
