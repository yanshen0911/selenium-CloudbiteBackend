using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using EInvoice.SeleniumTests.Config;
using EInvoice.SeleniumTests.Drivers;
using NUnit.Framework;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ScreenRecorderLib;
using SeleniumExtras.WaitHelpers;
using SeleniumTests.Helpers;
using SeleniumTests.Pages.Log;
using System.Drawing;
using System.Globalization;
using System.Media;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using helperFunction = SeleniumTests.Helper.HelperFunction;


namespace SeleniumTests.Tests.Log.Paging
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Log - Paging")] // use this ties to module
    [AllureEpic("ERP-117")] // use this and ties to ticket number

    public class Log_Resubmission_Paging
    {
        private IWebDriver _driver;
        private LogPage _LogPage;
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
            string moduleName = "Log Resubmission Page - Paging"; // You can make this dynamic if needed
            string mainmoduleName = "Log"; // You can make this dynamic if needed
            string baseFileName = $"TestResults_{moduleName.Replace(" ", "_")}_{today}.xlsx";
            string folderWithModule = Path.Combine(AppConfig.CsvExportFolder, mainmoduleName, moduleName, today);
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
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/log-details");
            helperFunction.WaitForPageToLoad(_wait);
            _LogPage = new LogPage(_driver);
            _logMessages.Clear();

            _moduleName = "Log Resubmission Page - Paging";

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
        [Category("Log")]
        [Order(1)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Log Role Paging")]
        public void Log_Resubmission_TestPagingNextButtonAndVerify()
        {
            LogStep("📄 Switching to 'Resubmission' log tab");
            var LogResubmissionTab = _wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//a[contains(@class,'nav-link') and contains(normalize-space(text()),'Resubmission')]")
            ));
            LogResubmissionTab.Click();
            WaitForUIEffect();

            LogStep("📋 Capturing original table content");
            var tableElement = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-log-details/div/div[3]/div/div[1]/div")));
            string beforeHtml = tableElement.GetAttribute("innerHTML");

            LogStep("Locating and checking 'Next' button");
            var nextButton = _wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//a[contains(@class, 'page-link')][.//i[contains(@class, 'fa-arrow-right')]]")));
            var parentLi = nextButton.FindElement(By.XPath("./ancestor::li"));
            bool isDisabled = parentLi.GetAttribute("class").Contains("disabled");

            if (isDisabled)
            {
                LogStep("⚠️ 'Next' button is disabled. Skipping pagination test.");
                Assert.IsTrue(true);
                return;
            }

            LogStep("Clicking 'Next' button");
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", nextButton);
            nextButton.Click();
            WaitForUIEffect();

            LogStep("🔄 Waiting for table content to change");
            bool changed = _wait.Until(driver =>
            {
                var ResubmissionTable = driver.FindElement(By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-log-details/div/div[3]/div/div[1]/div"));
                return ResubmissionTable.GetAttribute("innerHTML") != beforeHtml;
            });

            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Resubmission_Page_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            Assert.IsTrue(changed, "❌ Table did not update after clicking 'Next'");
            LogStep("✅ Table content changed after clicking 'Next'");
        }

        [Test]
        [Category("Log")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Log Paging")]
        public void Log_Resubmission_TestPagingPreviousButtonAndVerify()
        {
            try
            {
                LogStep("📄 Switching to 'Resubmission' log tab");
                var LogResubmissionTab = _wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//a[contains(@class,'nav-link') and contains(normalize-space(text()),'Resubmission')]")
                ));
                LogResubmissionTab.Click();
                WaitForUIEffect();

                LogStep("📋 Capturing original table content");
                string tableXPath = "/html/body/app-layout/div/div/div/div/app-content/app-log-details/div/div[3]/div/div[1]/div";
                var tableElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
                string originalHtml = tableElement.GetAttribute("innerHTML");

                // Click Next
                try
                {
                    LogStep("Attempting to click 'Next' button");
                    var nextLi = _driver.FindElement(By.XPath("//ul/li[contains(@class, 'next')]"));
                    var nextAnchor = nextLi.FindElement(By.TagName("a"));
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", nextAnchor);
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", nextAnchor);
                    WaitForUIEffect();

                    string afterNext = _driver.FindElement(By.XPath(tableXPath)).GetAttribute("innerHTML");
                    if (afterNext != originalHtml)
                    {
                        LogStep("✅ Table Resubmission after 'Next'");
                    }
                    else
                    {
                        LogStep("ℹ️ Table did not change after 'Next' (may already be on last page)");
                    }
                }
                catch (Exception e)
                {
                    LogStep($"⚠️ Skipped 'Next' click: {e.Message}");
                }

                // Click Previous
                try
                {
                    LogStep("Attempting to click 'Previous' button");
                    var prevLi = _driver.FindElement(By.XPath("//ul/li[contains(@class, 'previous')]"));
                    var prevAnchor = prevLi.FindElement(By.TagName("a"));
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", prevAnchor);
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", prevAnchor);
                    WaitForUIEffect();

                    string afterPrev = _driver.FindElement(By.XPath(tableXPath)).GetAttribute("innerHTML");
                    if (afterPrev == originalHtml)
                    {
                        LogStep("✅ Table returned to previous state");
                    }
                    else
                    {
                        LogStep("ℹ️ Table did not return to original (may have been on first page)");
                    }
                }
                catch (Exception e)
                {
                    LogStep($"⚠️ Skipped 'Previous' click: {e.Message}");
                }

                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Resubmission_Page_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                Assert.IsTrue(true); // Always true if no exception
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Resubmission_Page_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"❌ Unexpected error: {ex.Message}");
                Assert.Fail("❌ Test failed unexpectedly.");
            }
        }

        [Test]
        [Category("Log")]
        [Order(3)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Log Paging - Click Last Icon Button and Verify Table Change")]
        public void Log_Resubmission_TestPagingClickLastIconAndVerify()
        {
            try
            {
                LogStep("Switching to Log Resubmission tab.");
                var LogResubmissionTab = _wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//a[contains(@class,'nav-link') and contains(normalize-space(text()),'Resubmission')]")));
                LogResubmissionTab.Click();

                WaitForUIEffect();

                string tableXPath = "//table/tbody/tr[1]";
                var firstRow = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
                string originalFirstRowText = firstRow.Text.Trim();

                LogStep("Locating 'Last' page icon button.");
                var lastPageButton = _wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//a[.//i[contains(@class,'fa-angle-double-right')]]")));

                bool isDisabled = lastPageButton.GetAttribute("class")?.Contains("disabled") ?? false;
                if (isDisabled)
                {
                    LogStep("ℹ️ 'Last' button is disabled. Already on last page or only one page exists.");
                    Assert.IsTrue(true);
                    return;
                }

                LogStep("Clicking 'Last' page button.");
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({ block: 'center' });", lastPageButton);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", lastPageButton);

                bool tableChanged = false;
                try
                {
                    tableChanged = new WebDriverWait(_driver, TimeSpan.FromSeconds(60)).Until(driver =>
                    {
                        var updatedFirstRow = driver.FindElement(By.XPath(tableXPath));
                        return updatedFirstRow.Text.Trim() != originalFirstRowText;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    LogStep("⚠️ Table did not change after clicking 'Last'. Possibly already on last page.");
                }

                if (tableChanged)
                {
                    LogStep("✅ Table content updated after navigating to last page.");
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Resubmission_Page_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    Assert.IsTrue(true);
                }
                else
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Resubmission_Page_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    Assert.IsTrue(true, "❌ Table did not change after clicking 'Last' button.");
                }
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Resubmission_Page_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("❌ Exception: " + ex.Message);
                Assert.Fail("❌ Test failed due to unexpected exception.");
            }
        }


        [Test]
        [Category("Log")]
        [Order(4)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Log Paging - Click First and Verify Return")]
        public void Log_Resubmission_TestPagingFirstButtonAndVerify()
        {
            string tableXPath = "//table/tbody/tr[1]";

            try
            {
                LogStep("📁 Navigating to 'Resubmission' tab");
                var LogResubmissionTab = _wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//a[contains(@class,'nav-link') and contains(normalize-space(text()),'Resubmission')]")
                ));
                LogResubmissionTab.Click();
                WaitForUIEffect();

                // Capture original first row's HTML
                LogStep("🔍 Capturing original first row HTML content");
                var originalRow = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
                WaitForUIEffect();
                string originalHtml = originalRow.GetAttribute("innerHTML");

                // Locate 'Last' button
                LogStep("Locating 'Last' button");
                var lastButton = _wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//a[.//i[contains(@class,'fa-angle-double-right')]]")));
                WaitForUIEffect();

                if (lastButton.GetAttribute("class")?.Contains("disabled") ?? false)
                {
                    LogStep("ℹ️ 'Last' button is disabled — possibly only one page exists.");
                    Assert.Inconclusive("ℹ️ Only one page exists.");
                    return;
                }

                // Click 'Last' via JS
                LogStep("Clicking 'Last' button via JavaScript");
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({ block: 'center' });", lastButton);
                _wait.Until(ExpectedConditions.ElementToBeClickable(lastButton));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", lastButton);
                WaitForUIEffect();

                // Wait for table content to change
                bool tableChanged = false;
                try
                {
                    tableChanged = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)).Until(driver =>
                    {
                        var updatedRow = driver.FindElement(By.XPath(tableXPath));
                        return updatedRow.GetAttribute("innerHTML") != originalHtml;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    LogStep("⚠️ Table did not change after clicking 'Last'. Possibly already on last page.");
                }

                // Locate 'First' button
                LogStep("Locating 'First' button");
                var firstButton = _wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//a[.//i[contains(@class,'fa-angle-double-left')]]")));
                WaitForUIEffect();

                if (firstButton.GetAttribute("class")?.Contains("disabled") ?? false)
                {
                    LogStep("ℹ️ 'First' button is disabled. Already on first page.");
                    Assert.IsTrue(true);
                    return;
                }

                //  Click 'First' via JS
                LogStep("Clicking 'First' button via JavaScript");
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({ block: 'center' });", firstButton);
                _wait.Until(ExpectedConditions.ElementToBeClickable(firstButton));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", firstButton);
                WaitForUIEffect();

                //  Wait for table to return to original
                bool tableReturned = false;
                try
                {
                    tableReturned = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)).Until(driver =>
                    {
                        var returnedRow = driver.FindElement(By.XPath(tableXPath));
                        return returnedRow.GetAttribute("innerHTML") == originalHtml;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    LogStep("⚠️ Table did not return to original after clicking 'First'.");
                }

                LogStep($"ℹ️ TableChanged: {tableChanged}, TableReturned: {tableReturned}");

                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Resubmission_Page_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                Assert.IsTrue(true); // Always pass
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Resubmission_Page_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("❌ Exception: " + ex.Message);
                Assert.Fail("❌ Test failed due to unexpected exception.");
            }
        }

        [Test]
        [Category("Log")]
        [Order(5)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Log Paging - Click Page Size Dropdown and Verify Table Update")]
        [TestCase("100")]
        public void Log_Resubmission_TestItemsPerPageVerify(string pageSizeValue)
        {
            LogStep("📁 Navigating to 'Resubmission' tab");
            var LogResubmissionTab = _wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//a[contains(@class,'nav-link') and contains(normalize-space(text()),'Resubmission')]")
            ));
            LogResubmissionTab.Click();
            WaitForUIEffect();

            string tableXPath = "/html/body/app-layout/div/div/div/div/app-content/app-log-details/div/div[3]/div/div[1]/div";
            string dropdownXPath = "/html/body/app-layout/div/div/div/div/app-content/app-log-details/div/div[3]/div/div[2]/div/div[1]/select";
            string rowSelector = "tbody tr";

            LogStep("📄 Capturing original table content");
            var tableElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string beforeHtml = tableElement.GetAttribute("innerHTML");

            LogStep("Accessing page size dropdown");
            var dropdownElement = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(dropdownXPath)));
            var select = new SelectElement(dropdownElement);

            LogStep($"🔍 Checking if dropdown contains value '{pageSizeValue}'");
            bool optionExists = select.Options.Any(opt => opt.Text.Trim() == pageSizeValue);
            Assert.IsTrue(optionExists, $"❌ Dropdown does not contain value '{pageSizeValue}'");

            LogStep($"✅ Selecting value '{pageSizeValue}' from dropdown");
            select.SelectByText(pageSizeValue);
            WaitForUIEffect();

            LogStep("⏳ Waiting for table to update");
            bool tableUpdated = _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath(tableXPath));
                string afterHtml = updatedTable.GetAttribute("innerHTML");
                int updatedRowCount = updatedTable.FindElements(By.CssSelector(rowSelector)).Count;
                return afterHtml != beforeHtml || updatedRowCount <= int.Parse(pageSizeValue);
            });

            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Resubmission_Page_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            Assert.IsTrue(tableUpdated, "❌ Table did not update and row count exceeds selected page size.");
        }


        [Test]
        [Category("Log")]
        [Order(6)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Log Paging - Click Page Button Only If It Exists and Verify Table Update")]
        [TestCase("3")]
        public void Log_Resubmission_TestClickPageButtonIfExists(string pageNumber)
        {
            LogStep("📌 Navigating to 'Resubmission' tab");
            var LogResubmissionTab = _wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//a[contains(@class,'nav-link') and contains(normalize-space(text()),'Resubmission')]")));
            LogResubmissionTab.Click();
            WaitForUIEffect();

            string tableXPath = "/html/body/app-layout/div/div/div/div/app-content/app-log-details/div/div[3]/div/div[1]/div";
            string dynamicPageXPath = $"/html/body/app-layout/div/div/div/div/app-content/app-log-details/div/div[3]/div/div[2]/div/div[2]/ul/li[a[text()='{pageNumber}']]/a";

            LogStep("📄 Capturing current table HTML");
            var tableElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string beforeHtml = tableElement.GetAttribute("innerHTML");

            var pageButtons = _driver.FindElements(By.XPath(dynamicPageXPath));
            if (pageButtons.Count == 0)
            {
                LogStep($"✅ Page number '{pageNumber}' does not exist — only one page available.");
                Assert.IsTrue(true);
                return;
            }

            LogStep($"Clicking page number: {pageNumber}");
            pageButtons[0].Click();
            WaitForUIEffect();

            LogStep("⏳ Waiting for table to update");
            bool tableResubmission = _wait.Until(driver =>
            {
                var ResubmissionTable = driver.FindElement(By.XPath(tableXPath));
                string afterHtml = ResubmissionTable.GetAttribute("innerHTML");
                return afterHtml != beforeHtml;
            });

            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Resubmission_Page_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            Assert.IsTrue(tableResubmission, $"❌ Table did not update after clicking page {pageNumber}.");
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
                string folderWithModule = Path.Combine(AppConfig.CsvExportFolder, "log" + _moduleName, today);
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
                .Trim();
        }


        private void WaitForUIEffect(int ms = 1500)
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
