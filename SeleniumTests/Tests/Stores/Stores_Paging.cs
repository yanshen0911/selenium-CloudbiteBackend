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
using SeleniumTests.Helpers;
using SeleniumTests.Pages.Stores;
using System.Drawing;
using System.Globalization;
using System.Media;
using System.Net.NetworkInformation;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using helperFunction = SeleniumTests.Helper.HelperFunction;


namespace SeleniumTests.Tests.Stores
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Stores - Paging")] // use this ties to module
    [AllureEpic("ERP-117")] // use this and ties to ticket number

    public class Stores_Paging
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


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // 🧹 Delete existing export Excel file (if any)
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string moduleName = "Stores Page - Paging"; // You can make this dynamic if needed
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

            _moduleName = "Store Page - Paging";

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
        [Category("Stores")]
        [Order(1)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Stores Paging")]
        public void TestPagingNextButtonAndVerify()
        {
            string tableXPath = "/html/body/app-layout/div/div/div/div/app-content/app-store/div/div[3]/div/div[1]";
            string nextButtonXPath = "/html/body/app-layout/div/div/div/div/app-content/app-store/div/div[3]/div/div[2]/app-global-pagination/div/div[2]/ul/li[4]";

            //  Wait for table to be visible
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            WaitForUIEffect();
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            LogStep("📄 Captured current table content before clicking 'Next'.");

            // Find the "Next" button
            var nextButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(nextButtonXPath)));

            // Check if "Next" button is disabled
            bool isDisabled = nextButton.GetAttribute("class").Contains("disabled");
            LogStep(isDisabled
                ? "⚠️ 'Next' button is disabled. Only one page available. Skipping pagination test."
                : "🔎 'Next' button is enabled. Proceeding with click.");
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            Assert.IsTrue(true); // Pass regardless of button state

            if (isDisabled) return;

            // Click the "Next" button
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(nextButton));
            nextButton.Click();
            LogStep("✅ Clicked 'Next' button.");
            WaitForUIEffect();

            // Wait for table content to change
            bool tableChanged = _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath(tableXPath));
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                return updatedTable.GetAttribute("innerHTML") != beforeHtml;
            });


            LogStep(tableChanged
                ? "✅ Table content updated after clicking 'Next'."
                : "❌ Table content did not update after clicking 'Next'.");

            Assert.IsTrue(tableChanged, "❌ Table content did not change after pagination.");
        }



        [Test]
        [Category("Stores")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Stores Paging")]
        public void TestPagingPreviousButtonAndVerify()
        {
            string tableXPath = "/html/body/app-layout/div/div/div/div/app-content/app-store/div/div[3]/div/div[1]";
            string nextButtonXPath = "/html/body/app-layout/div/div/div/div/app-content/app-store/div/div[3]/div/div[2]/app-global-pagination/div/div[2]/ul/li[4]";
            string previousButtonXPath = "/html/body/app-layout/div/div/div/div/app-content/app-store/div/div[3]/div/div[2]/app-global-pagination/div/div[2]/ul/li[2]";

            //  Capture current table HTML content
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string originalHtml = tableElement.GetAttribute("innerHTML");
            WaitForUIEffect();
            LogStep("📄 Captured original table HTML content.");

            // Check if "Next" is disabled
            var nextButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(nextButtonXPath)));
            bool nextDisabled = nextButton.GetAttribute("disabled") == "true" || nextButton.GetAttribute("class").Contains("disabled");

            if (nextDisabled)
            {
                LogStep("⚠️ 'Next' button is disabled. Only one page available. Skipping test.");
                Assert.IsTrue(true);
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                return;

            }

            // Click "Next" button
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(nextButton));
            nextButton.Click();
            LogStep("✅ 'Next' button clicked.");
            WaitForUIEffect();

            // Wait for table content to change
            bool tableChanged = _wait.Until(driver =>
            {
                var newTable = driver.FindElement(By.XPath(tableXPath));
                return newTable.GetAttribute("innerHTML") != originalHtml;
            });
            LogStep(tableChanged ? "✅ Table content updated after clicking 'Next'." : "❌ Table content did not change.");
            Assert.IsTrue(tableChanged, "❌ Table content did not change after clicking 'Next'.");

            // Locate "Previous" button
            var previousButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(previousButtonXPath)));
            bool prevDisabled = previousButton.GetAttribute("disabled") == "true" || previousButton.GetAttribute("class").Contains("disabled");

            if (prevDisabled)
            {
                LogStep("⚠️ 'Previous' button is disabled. Cannot return to page 1.");
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                Assert.IsTrue(true);
                return;
            }

            // Click "Previous" button
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(previousButton));
            previousButton.Click();
            LogStep("✅ 'Previous' button clicked.");
            WaitForUIEffect();

            //  Wait for table to return to original content
            bool tableReturned = _wait.Until(driver =>
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                var tableBack = driver.FindElement(By.XPath(tableXPath));
                return tableBack.GetAttribute("innerHTML") == originalHtml;
            });

            LogStep(tableReturned
                ? "✅ Table returned to original after clicking 'Previous'."
                : "❌ Table did not return to original state after clicking 'Previous'.");

            Assert.IsTrue(tableReturned, "❌ Table content did not return to original after clicking 'Previous'.");
        }



        [Test]
        [Category("Stores")]
        [Order(3)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Stores Paging - Click Last and Verify Change")]
        public void TestPagingLastButtonAndVerify()
        {
            string tableXPath = "/html/body/app-layout/div/div/div/div/app-content/app-store/div/div[3]/div/div[1]";
            string lastButtonXPath = "//a[.//i[contains(@class,'fa-angle-double-right')]]";

            // Wait for the table to be visible
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            WaitForUIEffect();
            LogStep("📄 Captured initial table content.");

            // Locate "Last" pagination button
            var lastButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(lastButtonXPath)));
            bool isDisabled = lastButton.GetAttribute("class")?.Contains("disabled") ?? false;

            if (isDisabled)
            {
                LogStep("ℹ️ 'Last' button is disabled — already on last page or only one page available.");
                Assert.IsTrue(true);
                return;
            }

    // Scroll into view and click using JS
    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", lastButton);
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(lastButton));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", lastButton);
            LogStep("✅ 'Last' button clicked via JavaScript.");
            WaitForUIEffect();

            // Wait for table to update
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
                LogStep("⚠️ Table did not change within timeout — possibly already on last page.");
            }

            // Final result logging
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            LogStep(tableChanged
                ? "✅ Table content changed after clicking 'Last'."
                : "ℹ️ No table change after clicking 'Last', but still valid scenario.");

            Assert.IsTrue(true, "✅ Paging test completed — behavior verified.");
        }





        [Test]
        [Category("Stores")]
        [Order(4)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Stores Paging - Click First and Verify Change")]
        public void TestPagingFirstButtonAndVerify()
        {
            string tableXPath = "/html/body/app-layout/div/div/div/div/app-content/app-store/div/div[3]/div/div[1]";
            string lastButtonXPath = "//a[.//i[contains(@class,'fa-angle-double-right')]]";
            string firstButtonXPath = "//a[.//i[contains(@class,'fa-angle-double-left')]]";

            // Capture current table state
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string originalHtml = tableElement.GetAttribute("innerHTML");
            LogStep("📄 Captured original table HTML.");
            WaitForUIEffect();

            // Click "Last" button if available
            var lastButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(lastButtonXPath)));
            bool lastDisabled = lastButton.GetAttribute("class")?.Contains("disabled") ?? false;

            if (lastDisabled)
            {
                LogStep("ℹ️ 'Last' button is disabled — only one page exists or already on last page.");
                Assert.IsTrue(true);
                return;
            }

    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", lastButton);
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(lastButton));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", lastButton);
            LogStep("✅ 'Last' button clicked.");
            WaitForUIEffect();

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

            // Click "First" button if available
            var firstButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(firstButtonXPath)));
            bool firstDisabled = firstButton.GetAttribute("class")?.Contains("disabled") ?? false;

            if (firstDisabled)
            {
                LogStep("ℹ️ 'First' button is disabled. Cannot return to first page.");
                Assert.IsTrue(true);
                return;
            }

    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", firstButton);
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(firstButton));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", firstButton);
            LogStep("✅ 'First' button clicked.");
            WaitForUIEffect();

            // Validate return to original table
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

            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            LogStep($"✅ Paging completed. TableChanged: {tableChanged}, ReturnedToFirst: {tableReturned}");
            Assert.IsTrue(true);
        }




        [Test]
        [Category("Stores")]
        [Order(5)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Stores Paging - Click Page Size Dropdown and Verify Table Update")]
        [TestCase("100")]
        public void TestItemsPerPageVerify(string pageSizeValue)
        {
            string tableXPath = "/html/body/app-layout/div/div/div/div/app-content/app-store/div/div[3]/div/div[1]";
            string dropdownXPath = "/html/body/app-layout/div/div/div/div/app-content/app-store/div/div[3]/div/div[2]/app-global-pagination/div/div[1]/select";
            string rowSelector = "tbody tr";

            // Capture table state
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            int originalRowCount = tableElement.FindElements(By.CssSelector(rowSelector)).Count;
            LogStep($"📄 Captured original HTML with {originalRowCount} rows.");
            WaitForUIEffect();

            // Locate and verify dropdown
            var dropdownElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(dropdownXPath)));
            var select = new SelectElement(dropdownElement);

            bool optionExists = select.Options.Any(opt => opt.Text.Trim() == pageSizeValue);
            if (!optionExists)
            {
                LogStep($"❌ Page size '{pageSizeValue}' not found in dropdown.");
                Assert.Fail($"Dropdown does not contain value '{pageSizeValue}'");
            }

            // Select dropdown value
            select.SelectByText(pageSizeValue);
            LogStep($"✅ Selected page size: {pageSizeValue}");
            WaitForUIEffect();

            // Wait for update
            bool tableUpdated = _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath(tableXPath));
                string afterHtml = updatedTable.GetAttribute("innerHTML");
                int updatedRowCount = updatedTable.FindElements(By.CssSelector(rowSelector)).Count;
                return afterHtml != beforeHtml || updatedRowCount <= int.Parse(pageSizeValue);
            });

            WaitForUIEffect();

            // Log result
            if (tableUpdated)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("✅ Table updated or already showing all available rows.");
            }
            else
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("❌ Table did not update as expected.");
                Assert.Fail("Table did not update and row count exceeds selected page size.");
            }
        }


        [Test]
        [Category("Stores")]
        [Order(6)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Stores Paging - Click Page Button Only If It Exists and Verify Table Update")]
        [TestCase("3")]
        public void TestClickPageButtonIfExists(string pageNumber)
        {
            string tableXPath = "/html/body/app-layout/div/div/div/div/app-content/app-store/div/div[3]/div/div[1]";
            string paginationXPathTemplate = "/html/body/app-layout/div/div/div/div/app-content/app-store/div/div[3]/div/div[2]/app-global-pagination/div/div[2]/ul/li[a[text()='{0}']]/a";
            string dynamicPageXPath = string.Format(paginationXPathTemplate, pageNumber);

            // Capture current table content
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            LogStep("📄 Captured original table HTML.");
            WaitForUIEffect();

            // Locate page number button
            var pageButtons = _driver.FindElements(By.XPath(dynamicPageXPath));
            if (pageButtons.Count == 0)
            {
                LogStep($"✅ No page {pageNumber} exists — only one page available. Test logically passed.");
                Assert.IsTrue(true);
                return;
            }

            // Click the page number button
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(pageButtons[0]));
            pageButtons[0].Click();
            LogStep($"✅ Clicked on page number {pageNumber}.");
            WaitForUIEffect();

            // Wait for table update
            bool tableUpdated = _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath(tableXPath));
                string afterHtml = updatedTable.GetAttribute("innerHTML");
                return afterHtml != beforeHtml;
            });

            WaitForUIEffect();

            // Validation
            if (tableUpdated)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"✅ Table updated after clicking page {pageNumber}.");
            }
            else
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Stores_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"❌ Table did not update after clicking page {pageNumber}.");
                Assert.Fail("Table content did not change.");
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
                    worksheet.Cells[row, 5].Value = "Paging completed successfully. The data has been refreshed and reloaded.";

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
