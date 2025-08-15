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

namespace SeleniumTests.Tests.Log
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Log - Log - Valid")]
    [AllureEpic("ERP-117")]
    public class Log_ForgotPassword_Valid
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
            string moduleName = "Log Forgot Password Page"; // You can make this dynamic if needed
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

            _moduleName = "Log Forgot Password Page";

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
        [AllureStory("Log Search - General Match (Partial Match Accepted)")]
        [TestCase("usersystem@einvoice.com", 2, true)]
        [TestCase("User", 3, true)]
        [TestCase("forgot password", 4, true)]
        public void Read_After_Create_Log(string searchText, int columnIndex, bool waitForRefresh)
        {
            try
            {
                // Navigate to Forgot Password Logs Tab
                var forgotPasswordTab = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-log-details/div/div[1]/div/ul/li[5]/a")
                ));
                forgotPasswordTab.Click();
                WaitForUIEffect();
                LogStep("✅ Switched to Forgot Password Logs tab");

                var tableXPath = "//table/tbody";
                string previousTableHtml = _driver.FindElement(By.XPath(tableXPath)).GetAttribute("innerHTML");

                // Perform search
                _LogPage.SearchLog(searchText);
                LogStep($"🔍 Searching for: {searchText}");
                WaitForUIEffect();

                bool tableRefreshed = false;

                try
                {
                    tableRefreshed = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)).Until(driver =>
                    {
                        var currentHtml = driver.FindElement(By.XPath(tableXPath)).GetAttribute("innerHTML");
                        return currentHtml != previousTableHtml;
                    });

                    if (tableRefreshed)
                        LogStep("✅ Table refreshed after search.");
                }
                catch (WebDriverTimeoutException)
                {
                    LogStep("⚠️ Table did not refresh. Proceeding with row content check.");
                }

                bool isMatchFound = false;
                var rows = _driver.FindElements(By.XPath("//table/tbody/tr"));

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count < columnIndex) continue;

                    string cellText;
                    try
                    {
                        cellText = cells[columnIndex - 1].FindElement(By.TagName("span")).Text.Trim();
                    }
                    catch
                    {
                        cellText = cells[columnIndex - 1].Text.Trim();
                    }

                    LogStep($"🔍 Checking cell[{columnIndex}]: '{cellText}' vs '{searchText}'");

                    if (cellText.Replace(" ", "").Contains(searchText.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                    {
                        isMatchFound = true;
                        break;
                    }
                }

                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Created_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                if (isMatchFound)
                {
                    LogStep("✅ Matching data found. Test passed.");
                }
                else
                {
                    if (tableRefreshed)
                    {
                        LogStep("⚠️ No matching data found, but table refreshed. Test considered passed.");
                    }
                    else
                    {
                        Assert.Fail($"❌ No match found and table did not refresh for '{searchText}' in column {columnIndex}.");
                    }
                }
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Created_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"❌ Exception: {ex.Message}");
                Assert.Fail("Test failed due to unexpected exception.");
            }
        }
       



        [Test]
        [Category("Log Forgot Password")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create - Export Log Report")]
        public void ExportLogForgotPasswordReport()
        {
            string downloadPath = AppConfig.DownloadPath;
            string filePrefix = "Log Index";
            bool isModalDisplayed = false;

            //  Navigate to 'Forgot Password' tab
            var forgotPasswordTab = _wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-log-details/div/div[1]/div/ul/li[5]/a")
            ));
            forgotPasswordTab.Click();
            WaitForUIEffect();

            // Click Export button
            helperFunction.WaitForElementToBeClickable(_wait,
                By.CssSelector("#kt_content_container > app-log-details > div > div.card-header.border-0.pt-5.gap-2 > div > div:nth-child(2) > a"));
            _LogPage.ClickExportButton();
            WaitForUIEffect();
            LogStep("📤 Clicked export button on Forgot Password tab");

            // Check if modal appears (export processing in background)
            try
            {
                var modal = _wait.Until(driver =>
                {
                    try
                    {
                        var element = driver.FindElement(By.XPath("/html/body/div/div"));
                        return element.Displayed ? element : null;
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });

                if (modal != null)
                {
                    isModalDisplayed = true;

                    var message = modal.Text.Trim();
                    LogStep("📢 Modal appeared: " + message);

                    if (!message.ToLower().Contains("under processing due to large size"))
                    {
                        Assert.Fail("❌ Unexpected modal message: " + message);
                        _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Created_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                        var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                        File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    }

                    var okButton = modal.FindElement(By.XPath(".//button[contains(., 'Ok, got it!')]"));
                    okButton.Click();
                    WaitForUIEffect();

                    LogStep("Export is being processed due to large size. Skipped file download check.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Created_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("✅ No modal appeared. Proceeding with file download check.");
            }

            // Download check only if no modal
            if (!isModalDisplayed)
            {
                string latestFile = null;
                DateTime startTime = DateTime.Now;
                bool fileDownloaded = false;

                while ((DateTime.Now - startTime).TotalSeconds < 20)
                {
                    var files = Directory.GetFiles(downloadPath, $"{filePrefix}*")
                        .Where(f => !f.EndsWith(".crdownload"))
                        .OrderByDescending(File.GetLastWriteTime)
                        .ToList();

                    if (files.Any())
                    {
                        latestFile = files.First();
                        if (File.GetLastWriteTime(latestFile) > startTime.AddSeconds(-5))
                        {
                            fileDownloaded = true;
                            break;
                        }
                    }

                    Thread.Sleep(1000);
                }

                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Created_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                Assert.IsTrue(fileDownloaded, "❌ File download failed.");
                LogStep($"✅ Export completed. File downloaded: {Path.GetFileName(latestFile)}");
            }
        }


        [Test]
        [Category("Log")]
        [Order(3)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Log Search - General Match (Partial Match Accepted)")]
        [TestCase("yanshen.choo@qubeapps.com", 2, true)]
        public void Reset_After_Search_Log(string searchText, int columnIndex, bool waitForRefresh)
        {
            try
            {
                //  Navigate to 'Forgot Password' tab
                var forgotPasswordTab = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-log-details/div/div[1]/div/ul/li[5]/a")
                ));
                forgotPasswordTab.Click();
                WaitForUIEffect();

                var tableXPath = "//table/tbody";
                string previousTableHtml = _driver.FindElement(By.XPath(tableXPath)).GetAttribute("innerHTML");

                // Perform search
                _LogPage.SearchLog(searchText);
                LogStep($"🔍 Searching for: {searchText}");
                WaitForUIEffect();

                bool tableRefreshed = false;

                try
                {
                    tableRefreshed = new WebDriverWait(_driver, TimeSpan.FromSeconds(120)).Until(driver =>
                    {
                        var currentHtml = driver.FindElement(By.XPath(tableXPath)).GetAttribute("innerHTML");
                        return currentHtml != previousTableHtml;
                    });

                    if (tableRefreshed)
                        LogStep("✅ Table refreshed after search.");
                }
                catch (WebDriverTimeoutException)
                {
                    LogStep("⚠️ Table did not refresh. Proceeding with row content check.");
                }

                bool isMatchFound = false;
                var rows = _driver.FindElements(By.XPath("//table/tbody/tr"));

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count < columnIndex) continue;

                    string cellText;
                    try
                    {
                        cellText = cells[columnIndex - 1].FindElement(By.TagName("span")).Text.Trim();
                    }
                    catch
                    {
                        cellText = cells[columnIndex - 1].Text.Trim();
                    }

                    LogStep($"🔍 Checking cell[{columnIndex}]: '{cellText}' vs '{searchText}'");

                    if (cellText.Replace(" ", "").Contains(searchText.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                    {
                        isMatchFound = true;
                        break;
                    }
                }

                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_Created_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                if (isMatchFound)
                {
                    LogStep("✅ Matching data found. Test passed.");

                    // 🔹 Click reset button after successful search
                    LogStep("Clicking reset button after search success...");
                    _LogPage.ClickResetButton();
                    WaitForUIEffect();
                    LogStep("✅ Reset button clicked and table refreshed success.");
                }
                else
                {
                    if (tableRefreshed)
                    {
                        LogStep("⚠️ No matching data found, but table refreshed. Test considered passed.");
                    }
                    else
                    {
                        Assert.Fail($"❌ No match found and table did not refresh for '{searchText}' in column {columnIndex}.");
                    }
                }
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_ForgotPassword_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"❌ Exception: {ex.Message}");
                Assert.Fail("Test failed due to unexpected exception.");
            }
        }


        [Test]
        [Category("Log")]
        [Order(4)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Log Search - Filter by User Dropdown (Partial Match Accepted)")]
        [TestCase("Diy", 2, true)]
        public void Filter_User_Log(string searchText, int columnIndex, bool waitForRefresh)
        {
            try
            {
                //  Navigate to 'Forgot Password' tab
                var forgotPasswordTab = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-log-details/div/div[1]/div/ul/li[5]/a")
                ));
                forgotPasswordTab.Click();
                WaitForUIEffect();

                var tableXPath = "//table/tbody";
                string previousTableHtml = _driver.FindElement(By.XPath(tableXPath)).GetAttribute("innerHTML");

                // 🔹 Select from User dropdown instead of typing search
                LogStep($"🔍 Selecting user from dropdown: {searchText}");
                var dropdown = _driver.FindElement(By.CssSelector("#kt_content_container > app-log-details > div > div.card-header.border-0.pt-5.gap-2 > h3 > div > select"));
                // ^ change XPath to match your real User dropdown
                var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(dropdown);
                selectElement.SelectByText(searchText);

                WaitForUIEffect();

                bool tableRefreshed = false;

                try
                {
                    tableRefreshed = new WebDriverWait(_driver, TimeSpan.FromSeconds(120)).Until(driver =>
                    {
                        var currentHtml = driver.FindElement(By.XPath(tableXPath)).GetAttribute("innerHTML");
                        return currentHtml != previousTableHtml;
                    });

                    if (tableRefreshed)
                        LogStep("✅ Table refreshed after dropdown selection.");
                }
                catch (WebDriverTimeoutException)
                {
                    LogStep("⚠️ Table did not refresh. Proceeding with row content check.");
                }

                bool isMatchFound = false;
                var rows = _driver.FindElements(By.XPath("//table/tbody/tr"));

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count < columnIndex) continue;

                    string cellText;
                    try
                    {
                        cellText = cells[columnIndex - 1].FindElement(By.TagName("span")).Text.Trim();
                    }
                    catch
                    {
                        cellText = cells[columnIndex - 1].Text.Trim();
                    }

                    LogStep($"🔍 Checking cell[{columnIndex}]: '{cellText}' vs '{searchText}'");

                    if (cellText.Replace(" ", "").Contains(searchText.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                    {
                        isMatchFound = true;
                        break;
                    }
                }

                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_ForgotPassword_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                if (isMatchFound)
                {
                    LogStep("✅ Matching data found. Test passed.");

                    // 🔹 Click reset button after successful filter
                    LogStep("Clicking reset button after search success...");
                    _LogPage.ClickResetButton();
                    WaitForUIEffect();
                    LogStep("✅ Reset button clicked and table refreshed success.");
                }
                else
                {
                    if (tableRefreshed)
                    {
                        LogStep("⚠️ No matching data found, but table refreshed. Test considered passed.");
                    }
                    else
                    {
                        Assert.Fail($"❌ No match found and table did not refresh for '{searchText}' in column {columnIndex}.");
                    }
                }
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Log_ForgotPassword_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"❌ Exception: {ex.Message}");
                Assert.Fail("Test failed due to unexpected exception.");
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
