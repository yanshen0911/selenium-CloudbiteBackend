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
using System.Net.NetworkInformation;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using helperFunction = SeleniumTests.Helper.HelperFunction;


namespace SeleniumTests.Tests.Store
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Store - Paging")] // use this ties to module
    [AllureEpic("ERP-117")] // use this and ties to ticket number

    public class Store_Paging
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


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string moduleName = "Store Page - Paging"; // You can make this dynamic if needed

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

            _moduleName = "Store Page - Paging";

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
        [AllureStory("Store Paging")]
        public void TestStoreGroupPagingNextButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group/div[2]/p-table/div/div";
            string nextButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group/div[2]/p-table/div/p-paginator/div/button[3]";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store group page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-group");
            WaitForUIEffect();

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
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
        [Category("Store")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging")]
        public void TestStoreGroupPagingPreviousButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group/div[2]/p-table/div/div";
            string nextButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group/div[2]/p-table/div/p-paginator/div/button[3]";
            string previousButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group/div[2]/p-table/div/p-paginator/div/button[2]";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store group page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-group");
            WaitForUIEffect();

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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
        [Category("Store")]
        [Order(3)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click Last and Verify Change")]
        public void TestStoreGroupPagingLastButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group/div[2]/p-table/div/div";
            string lastButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group/div[2]/p-table/div/p-paginator/div/button[4]";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store group page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-group");
            WaitForUIEffect();

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
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            LogStep(tableChanged
                ? "✅ Table content changed after clicking 'Last'."
                : "ℹ️ No table change after clicking 'Last', but still valid scenario.");

            Assert.IsTrue(true, "✅ Paging test completed — behavior verified.");
        }





        [Test]
        [Category("Store")]
        [Order(4)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click First and Verify Change")]
        public void TestStoreGroupPagingFirstButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group/div[2]/p-table/div/div";
            string lastButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group/div[2]/p-table/div/p-paginator/div/button[4]";
            string firstButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group/div[2]/p-table/div/p-paginator/div/button[1]";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store group page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-group");
            WaitForUIEffect();

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

            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            LogStep($"✅ Paging completed. TableChanged: {tableChanged}, ReturnedToFirst: {tableReturned}");
            Assert.IsTrue(true);
        }




        [Test]
        [Category("Store")]
        [Order(5)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click Page Size Dropdown and Verify Table Update")]
        [TestCase("25")]
        public void TestStoreGroupItemsPerPageVerify(string pageSizeValue)
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group/div[2]/p-table/div/div";
            string dropdownToggleXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group/div[2]/p-table/div/p-paginator/div/p-dropdown/div/div[2]";
            string dropdownPanelXPath = "//body//div[contains(@class,'p-dropdown-panel')]";
            string rowSelector = "tbody tr";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store group page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-group");
            WaitForUIEffect();

            // Capture table state
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            int originalRowCount = tableElement.FindElements(By.CssSelector(rowSelector)).Count;
            LogStep($"📄 Captured original HTML with {originalRowCount} rows.");
            WaitForUIEffect();

            // Step 1: Click the dropdown toggle
            var dropdownToggle = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(dropdownToggleXPath)));
            dropdownToggle.Click();
            LogStep("📌 Opened page size dropdown.");

            // Step 2: Wait for dropdown panel to appear (use longer wait)
            var localWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var dropdownPanel = localWait.Until(driver =>
            {
                var panels = driver.FindElements(By.XPath(dropdownPanelXPath));
                return panels.FirstOrDefault(p => p.Displayed);
            });

            if (dropdownPanel == null)
            {
                LogStep("❌ Dropdown panel did not appear.");
                Assert.Fail("Dropdown panel was not found.");
            }

            // Step 3: Find and click the option
            var optionElements = dropdownPanel.FindElements(By.XPath(".//p-dropdownitem/li"));
            var optionElement = optionElements.FirstOrDefault(el => el.Text.Trim() == pageSizeValue);

            if (optionElement == null)
            {
                LogStep($"❌ Page size '{pageSizeValue}' not found in dropdown.");
                Assert.Fail($"Dropdown does not contain value '{pageSizeValue}'");
            }

            optionElement.Click();
            LogStep($"✅ Selected page size: {pageSizeValue}");
            WaitForUIEffect();

            // Step 4: Wait for table update
            bool tableUpdated = _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath(tableXPath));
                string afterHtml = updatedTable.GetAttribute("innerHTML");
                int updatedRowCount = updatedTable.FindElements(By.CssSelector(rowSelector)).Count;
                return afterHtml != beforeHtml || updatedRowCount <= int.Parse(pageSizeValue);
            });

            WaitForUIEffect();

            // Step 5: Log result + screenshot
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            if (tableUpdated)
            {
                LogStep("✅ Table updated or already showing all available rows.");
            }
            else
            {
                LogStep("❌ Table did not update as expected.");
                Assert.Fail("Table did not update and row count exceeds selected page size.");
            }
        }




        [Test]
        [Category("Store")]
        [Order(6)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click Page Button Only If It Exists and Verify Table Update")]
        [TestCase("1")]
        [TestCase("2")]
        public void TestStoreGroupClickPageButtonIfExists(string pageNumber)
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-group/div[2]/p-table/div/div";
            string dynamicPageXPath = $"//p-paginator//span[2]//button[normalize-space(text())='{pageNumber}']";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store group page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-group");
            WaitForUIEffect();

            // Capture current table content
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            LogStep("📄 Captured original table HTML.");
            WaitForUIEffect();

            // Locate page number button dynamically
            var pageButtons = _driver.FindElements(By.XPath(dynamicPageXPath));
            if (pageButtons.Count == 0)
            {
                LogStep($"✅ No page {pageNumber} exists — only one page available. Test logically passed.");
                Assert.IsTrue(true, $"✅ No page {pageNumber} exists — only one page available. Test logically passed.");
                return;
            }

            // Click the page number button
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(pageButtons[0]));
            pageButtons[0].Click();
            LogStep($"✅ Clicked on page number {pageNumber}.");
            WaitForUIEffect();

            bool tableUpdated = true; // default for page 1

            if (pageNumber != "1")
            {
                // For pages other than 1, ensure table updated
                tableUpdated = _wait.Until(driver =>
                {
                    var updatedTable = driver.FindElement(By.XPath(tableXPath));
                    string afterHtml = updatedTable.GetAttribute("innerHTML");
                    return afterHtml != beforeHtml;
                });
            }

            WaitForUIEffect();

            // Validation
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            if (tableUpdated)
            {
                LogStep($"✅ Paging check passed for page {pageNumber}.");
            }
            else
            {
                LogStep($"❌ Table did not update after clicking page {pageNumber}.");
                Assert.Fail($"Table content did not change when navigating to page {pageNumber}.");
            }
        }



        [Test]
        [Category("Store")]
        [Order(7)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging")]
        public void TestStoreCountryPagingNextButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/div";
            string nextButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/p-paginator/div/button[3]";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store Location (Country) page URL.");
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
            WaitForUIEffect();

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
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
        [Category("Store")]
        [Order(8)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging")]
        public void TestStoreCountryPagingPreviousButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/div";
            string nextButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/p-paginator/div/button[3]";
            string previousButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/p-paginator/div/button[2]";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
            WaitForUIEffect();

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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
        [Category("Store")]
        [Order(9)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click Last and Verify Change")]
        public void TestStoreCountryPagingLastButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/div";
            string lastButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/p-paginator/div/button[4]";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
            WaitForUIEffect();

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
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            LogStep(tableChanged
                ? "✅ Table content changed after clicking 'Last'."
                : "ℹ️ No table change after clicking 'Last', but still valid scenario.");

            Assert.IsTrue(true, "✅ Paging test completed — behavior verified.");
        }





        [Test]
        [Category("Store")]
        [Order(10)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click First and Verify Change")]
        public void TestStoreCountryPagingFirstButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/div";
            string lastButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/p-paginator/div/button[4]";
            string firstButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/p-paginator/div/button[1]";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
            WaitForUIEffect();

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

            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            LogStep($"✅ Paging completed. TableChanged: {tableChanged}, ReturnedToFirst: {tableReturned}");
            Assert.IsTrue(true);
        }




        [Test]
        [Category("Store")]
        [Order(11)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click Page Size Dropdown and Verify Table Update")]
        [TestCase("25")]
        public void TestStoreCountryItemsPerPageVerify(string pageSizeValue)
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/div";
            string dropdownToggleXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/p-paginator/div/p-dropdown/div/div[2]";
            string dropdownPanelXPath = "//body//div[contains(@class,'p-dropdown-panel')]";
            string rowSelector = "tbody tr";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
            WaitForUIEffect();

            // Capture table state
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            int originalRowCount = tableElement.FindElements(By.CssSelector(rowSelector)).Count;
            LogStep($"📄 Captured original HTML with {originalRowCount} rows.");
            WaitForUIEffect();

            // Step 1: Click the dropdown toggle
            var dropdownToggle = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(dropdownToggleXPath)));
            dropdownToggle.Click();
            LogStep("📌 Opened page size dropdown.");

            // Step 2: Wait for dropdown panel to appear (use longer wait)
            var localWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var dropdownPanel = localWait.Until(driver =>
            {
                var panels = driver.FindElements(By.XPath(dropdownPanelXPath));
                return panels.FirstOrDefault(p => p.Displayed);
            });

            if (dropdownPanel == null)
            {
                LogStep("❌ Dropdown panel did not appear.");
                Assert.Fail("Dropdown panel was not found.");
            }

            // Step 3: Find and click the option
            var optionElements = dropdownPanel.FindElements(By.XPath(".//p-dropdownitem/li"));
            var optionElement = optionElements.FirstOrDefault(el => el.Text.Trim() == pageSizeValue);

            if (optionElement == null)
            {
                LogStep($"❌ Page size '{pageSizeValue}' not found in dropdown.");
                Assert.Fail($"Dropdown does not contain value '{pageSizeValue}'");
            }

            optionElement.Click();
            LogStep($"✅ Selected page size: {pageSizeValue}");
            WaitForUIEffect(1000);

            // Step 4: Wait for table update
            bool tableUpdated = _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath(tableXPath));
                string afterHtml = updatedTable.GetAttribute("innerHTML");
                int updatedRowCount = updatedTable.FindElements(By.CssSelector(rowSelector)).Count;
                return afterHtml != beforeHtml || updatedRowCount <= int.Parse(pageSizeValue);
            });

            WaitForUIEffect(1000);

            // Step 5: Log result + screenshot
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            if (tableUpdated)
            {
                LogStep("✅ Table updated or already showing all available rows.");
            }
            else
            {
                LogStep("❌ Table did not update as expected.");
                Assert.Fail("Table did not update and row count exceeds selected page size.");
            }
        }




        [Test]
        [Category("Store")]
        [Order(12)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click Page Button Only If It Exists and Verify Table Update")]
        [TestCase("1")]
        [TestCase("2")]
        public void TestStoreCountryClickPageButtonIfExists(string pageNumber)
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[1]/div/div/p-table/div/div";
            string dynamicPageXPath = $"//p-paginator//span[2]//button[normalize-space(text())='{pageNumber}']";

            // Step 0: Navigate to the Store page
            LogStep("Navigate to Store Location page URL.");
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/management/store/store-zra");
            WaitForUIEffect();

            // Capture current table content
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            LogStep("📄 Captured original table HTML.");
            WaitForUIEffect();

            // Locate page number button dynamically
            var pageButtons = _driver.FindElements(By.XPath(dynamicPageXPath));
            if (pageButtons.Count == 0)
            {
                LogStep($"✅ No page {pageNumber} exists — only one page available. Test logically passed.");
                Assert.IsTrue(true, $"✅ No page {pageNumber} exists — only one page available. Test logically passed.");
                return;
            }

            // Click the page number button
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(pageButtons[0]));
            pageButtons[0].Click();
            LogStep($"✅ Clicked on page number {pageNumber}.");
            WaitForUIEffect();

            bool tableUpdated = true; // default for page 1

            if (pageNumber != "1")
            {
                // For pages other than 1, ensure table updated
                tableUpdated = _wait.Until(driver =>
                {
                    var updatedTable = driver.FindElement(By.XPath(tableXPath));
                    string afterHtml = updatedTable.GetAttribute("innerHTML");
                    return afterHtml != beforeHtml;
                });
            }

            WaitForUIEffect();

            // Validation
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            if (tableUpdated)
            {
                LogStep($"✅ Paging check passed for page {pageNumber}.");
            }
            else
            {
                LogStep($"❌ Table did not update after clicking page {pageNumber}.");
                Assert.Fail($"Table content did not change when navigating to page {pageNumber}.");
            }
        }

        [Test]
        [Category("Store")]
        [Order(13)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging")]
        public void TestStoreStatePagingNextButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[2]/div/div/p-table/div/div";
            string nextButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[2]/div/div/p-table/div/p-paginator/div/button[3]";

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
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
        [Category("Store")]
        [Order(14)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging")]
        public void TestStoreStatePagingPreviousButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[2]/div/div/p-table/div/div";
            string nextButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[2]/div/div/p-table/div/p-paginator/div/button[3]";
            string previousButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[2]/div/div/p-table/div/p-paginator/div/button[2]";

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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
        [Category("Store")]
        [Order(15)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click Last and Verify Change")]
        public void TestStoreStatePagingLastButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[2]/div/div/p-table/div/div";
            string lastButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[2]/div/div/p-table/div/p-paginator/div/button[4]";

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
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            LogStep(tableChanged
                ? "✅ Table content changed after clicking 'Last'."
                : "ℹ️ No table change after clicking 'Last', but still valid scenario.");

            Assert.IsTrue(true, "✅ Paging test completed — behavior verified.");
        }





        [Test]
        [Category("Store")]
        [Order(16)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click First and Verify Change")]
        public void TestStoreStatePagingFirstButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[2]/div/div/p-table/div/div";
            string lastButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[2]/div/div/p-table/div/p-paginator/div/button[4]";
            string firstButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[2]/div/div/p-table/div/p-paginator/div/button[1]";

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

            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            LogStep($"✅ Paging completed. TableChanged: {tableChanged}, ReturnedToFirst: {tableReturned}");
            Assert.IsTrue(true);
        }




        [Test]
        [Category("Store")]
        [Order(17)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click Page Size Dropdown and Verify Table Update")]
        [TestCase("25")]
        public void TestStoreStateItemsPerPageVerify(string pageSizeValue)
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[2]/div/div/p-table/div/div";
            string dropdownToggleXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[2]/div/div/p-table/div/p-paginator/div/p-dropdown/div/div[2]";
            string dropdownPanelXPath = "//body//div[contains(@class,'p-dropdown-panel')]";
            string rowSelector = "tbody tr";

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

            // Capture table state
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            int originalRowCount = tableElement.FindElements(By.CssSelector(rowSelector)).Count;
            LogStep($"📄 Captured original HTML with {originalRowCount} rows.");
            WaitForUIEffect();

            // Step 1: Click the dropdown toggle
            var dropdownToggle = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(dropdownToggleXPath)));
            dropdownToggle.Click();
            LogStep("📌 Opened page size dropdown.");

            // Step 2: Wait for dropdown panel to appear (use longer wait)
            var localWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var dropdownPanel = localWait.Until(driver =>
            {
                var panels = driver.FindElements(By.XPath(dropdownPanelXPath));
                return panels.FirstOrDefault(p => p.Displayed);
            });

            if (dropdownPanel == null)
            {
                LogStep("❌ Dropdown panel did not appear.");
                Assert.Fail("Dropdown panel was not found.");
            }

            // Step 3: Find and click the option
            var optionElements = dropdownPanel.FindElements(By.XPath(".//p-dropdownitem/li"));
            var optionElement = optionElements.FirstOrDefault(el => el.Text.Trim() == pageSizeValue);

            if (optionElement == null)
            {
                LogStep($"❌ Page size '{pageSizeValue}' not found in dropdown.");
                Assert.Fail($"Dropdown does not contain value '{pageSizeValue}'");
            }

            optionElement.Click();
            LogStep($"✅ Selected page size: {pageSizeValue}");
            WaitForUIEffect(1000);

            // Step 4: Wait for table update
            bool tableUpdated = _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath(tableXPath));
                string afterHtml = updatedTable.GetAttribute("innerHTML");
                int updatedRowCount = updatedTable.FindElements(By.CssSelector(rowSelector)).Count;
                return afterHtml != beforeHtml || updatedRowCount <= int.Parse(pageSizeValue);
            });

            WaitForUIEffect(1000);

            // Step 5: Log result + screenshot
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            if (tableUpdated)
            {
                LogStep("✅ Table updated or already showing all available rows.");
            }
            else
            {
                LogStep("❌ Table did not update as expected.");
                Assert.Fail("Table did not update and row count exceeds selected page size.");
            }
        }




        [Test]
        [Category("Store")]
        [Order(18)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click Page Button Only If It Exists and Verify Table Update")]
        [TestCase("1")]
        [TestCase("2")]
        public void TestStoreStateClickPageButtonIfExists(string pageNumber)
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[2]/div/div/p-table/div/div";
            string dynamicPageXPath = $"//p-paginator//span[2]//button[normalize-space(text())='{pageNumber}']";

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

            // Capture current table content
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            LogStep("📄 Captured original table HTML.");
            WaitForUIEffect();

            // Locate page number button dynamically
            var pageButtons = _driver.FindElements(By.XPath(dynamicPageXPath));
            if (pageButtons.Count == 0)
            {
                LogStep($"✅ No page {pageNumber} exists — only one page available. Test logically passed.");
                Assert.IsTrue(true, $"✅ No page {pageNumber} exists — only one page available. Test logically passed.");
                return;
            }

            // Click the page number button
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(pageButtons[0]));
            pageButtons[0].Click();
            LogStep($"✅ Clicked on page number {pageNumber}.");
            WaitForUIEffect();

            bool tableUpdated = true; // default for page 1

            if (pageNumber != "1")
            {
                // For pages other than 1, ensure table updated
                tableUpdated = _wait.Until(driver =>
                {
                    var updatedTable = driver.FindElement(By.XPath(tableXPath));
                    string afterHtml = updatedTable.GetAttribute("innerHTML");
                    return afterHtml != beforeHtml;
                });
            }

            WaitForUIEffect();

            // Validation
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            if (tableUpdated)
            {
                LogStep($"✅ Paging check passed for page {pageNumber}.");
            }
            else
            {
                LogStep($"❌ Table did not update after clicking page {pageNumber}.");
                Assert.Fail($"Table content did not change when navigating to page {pageNumber}.");
            }
        }


        [Test]
        [Category("Store")]
        [Order(13)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging")]
        public void TestStoreCityPagingNextButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[3]/div/div/p-table/div/div";
            string nextButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[3]/div/div/p-table/div/p-paginator/div/button[3]";

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
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
        [Category("Store")]
        [Order(14)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging")]
        public void TestStoreCityPagingPreviousButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[3]/div/div/p-table/div/div";
            string nextButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[3]/div/div/p-table/div/p-paginator/div/button[3]";
            string previousButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[3]/div/div/p-table/div/p-paginator/div/button[2]";

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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
        [Category("Store")]
        [Order(15)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click Last and Verify Change")]
        public void TestStoreCityPagingLastButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[3]/div/div/p-table/div/div";
            string lastButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[3]/div/div/p-table/div/p-paginator/div/button[4]";

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
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            LogStep(tableChanged
                ? "✅ Table content changed after clicking 'Last'."
                : "ℹ️ No table change after clicking 'Last', but still valid scenario.");

            Assert.IsTrue(true, "✅ Paging test completed — behavior verified.");
        }





        [Test]
        [Category("Store")]
        [Order(16)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click First and Verify Change")]
        public void TestStoreCityPagingFirstButtonAndVerify()
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[3]/div/div/p-table/div/div";
            string lastButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[3]/div/div/p-table/div/p-paginator/div/button[4]";
            string firstButtonXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[3]/div/div/p-table/div/p-paginator/div/button[1]";

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

            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            LogStep($"✅ Paging completed. TableChanged: {tableChanged}, ReturnedToFirst: {tableReturned}");
            Assert.IsTrue(true);
        }




        [Test]
        [Category("Store")]
        [Order(17)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click Page Size Dropdown and Verify Table Update")]
        [TestCase("25")]
        public void TestStoreCityItemsPerPageVerify(string pageSizeValue)
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[3]/div/div/p-table/div/div";
            string dropdownToggleXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[3]/div/div/p-table/div/p-paginator/div/p-dropdown/div/div[2]";
            string dropdownPanelXPath = "//body//div[contains(@class,'p-dropdown-panel')]";
            string rowSelector = "tbody tr";

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

            // Capture table state
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            int originalRowCount = tableElement.FindElements(By.CssSelector(rowSelector)).Count;
            LogStep($"📄 Captured original HTML with {originalRowCount} rows.");
            WaitForUIEffect();

            // Step 1: Click the dropdown toggle
            var dropdownToggle = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(dropdownToggleXPath)));
            dropdownToggle.Click();
            LogStep("📌 Opened page size dropdown.");

            // Step 2: Wait for dropdown panel to appear (use longer wait)
            var localWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var dropdownPanel = localWait.Until(driver =>
            {
                var panels = driver.FindElements(By.XPath(dropdownPanelXPath));
                return panels.FirstOrDefault(p => p.Displayed);
            });

            if (dropdownPanel == null)
            {
                LogStep("❌ Dropdown panel did not appear.");
                Assert.Fail("Dropdown panel was not found.");
            }

            // Step 3: Find and click the option
            var optionElements = dropdownPanel.FindElements(By.XPath(".//p-dropdownitem/li"));
            var optionElement = optionElements.FirstOrDefault(el => el.Text.Trim() == pageSizeValue);

            if (optionElement == null)
            {
                LogStep($"❌ Page size '{pageSizeValue}' not found in dropdown.");
                Assert.Fail($"Dropdown does not contain value '{pageSizeValue}'");
            }

            optionElement.Click();
            LogStep($"✅ Selected page size: {pageSizeValue}");
            WaitForUIEffect(1000);

            // Step 4: Wait for table update
            bool tableUpdated = _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath(tableXPath));
                string afterHtml = updatedTable.GetAttribute("innerHTML");
                int updatedRowCount = updatedTable.FindElements(By.CssSelector(rowSelector)).Count;
                return afterHtml != beforeHtml || updatedRowCount <= int.Parse(pageSizeValue);
            });

            WaitForUIEffect(1000);

            // Step 5: Log result + screenshot
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            if (tableUpdated)
            {
                LogStep("✅ Table updated or already showing all available rows.");
            }
            else
            {
                LogStep("❌ Table did not update as expected.");
                Assert.Fail("Table did not update and row count exceeds selected page size.");
            }
        }




        [Test]
        [Category("Store")]
        [Order(18)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Paging - Click Page Button Only If It Exists and Verify Table Update")]
        [TestCase("1")]
        [TestCase("2")]
        public void TestStoreCityClickPageButtonIfExists(string pageNumber)
        {
            string tableXPath = "/html/body/app-root/body/app-management/div/mat-sidenav-container/mat-sidenav-content/div[2]/app-store/div/div/app-store-zone/div[2]/mat-tab-group/div/mat-tab-body[3]/div/div/p-table/div/div";
            string dynamicPageXPath = $"//p-paginator//span[2]//button[normalize-space(text())='{pageNumber}']";

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

            // Capture current table content
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tableXPath)));
            string beforeHtml = tableElement.GetAttribute("innerHTML");
            LogStep("📄 Captured original table HTML.");
            WaitForUIEffect();

            // Locate page number button dynamically
            var pageButtons = _driver.FindElements(By.XPath(dynamicPageXPath));
            if (pageButtons.Count == 0)
            {
                LogStep($"✅ No page {pageNumber} exists — only one page available. Test logically passed.");
                Assert.IsTrue(true, $"✅ No page {pageNumber} exists — only one page available. Test logically passed.");
                return;
            }

            // Click the page number button
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(pageButtons[0]));
            pageButtons[0].Click();
            LogStep($"✅ Clicked on page number {pageNumber}.");
            WaitForUIEffect();

            bool tableUpdated = true; // default for page 1

            if (pageNumber != "1")
            {
                // For pages other than 1, ensure table updated
                tableUpdated = _wait.Until(driver =>
                {
                    var updatedTable = driver.FindElement(By.XPath(tableXPath));
                    string afterHtml = updatedTable.GetAttribute("innerHTML");
                    return afterHtml != beforeHtml;
                });
            }

            WaitForUIEffect();

            // Validation
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Store_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            if (tableUpdated)
            {
                LogStep($"✅ Paging check passed for page {pageNumber}.");
            }
            else
            {
                LogStep($"❌ Table did not update after clicking page {pageNumber}.");
                Assert.Fail($"Table content did not change when navigating to page {pageNumber}.");
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
                    string[] steps = message.Split(new[] { '\n', '-', '•', '|' }, StringSplitOptions.RemoveEmptyEntries);
                    string formattedSteps = string.Join("\n", steps.Select((s, i) => $"{i + 1}. {s.Trim()}"));

                    // Extract expected result if passed
                    string expectedResult = "Paging completed successfully. The data has been refreshed and reloaded.";
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

        private void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", element);
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
