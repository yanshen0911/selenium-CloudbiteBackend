using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using EInvoice.SeleniumTests.Config;
using EInvoice.SeleniumTests.Drivers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ScreenRecorderLib;
using SeleniumExtras.WaitHelpers;
using SeleniumTests.Helpers;
using SeleniumTests.Pages.Setting;
using System.Media;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using helperFunction = SeleniumTests.Helper.HelperFunction;
using OfficeOpenXml;
using System.Drawing;
using System.Globalization;
using OfficeOpenXml.Style;



namespace SeleniumTests.Tests.Setting
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Setting - Setting - Valid")]
    [AllureEpic("ERP-117")]
    public class SettingOnDashboard_Valid
    {
        private IWebDriver _driver;
        private SettingPage _SettingPage;
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
            string moduleName = "Setting On Dashboard Page"; // You can make this dynamic if needed
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
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/dashboard");
            helperFunction.WaitForPageToLoad(_wait);

            _SettingPage = new SettingPage(_driver);
            _logMessages.Clear();
            _moduleName = "Setting On Dashboard Page";

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
        [Category("Setting")]
        [Order(1)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCase("3", "4", "false")]
        public void Create_New_SettingOnDashboard(string CutOffDate, string ConsolidateCutOffDate, string securityToken)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

            try
            {
                // Navigate to Setting On Dashboard Tab
                var SettingOnDashboardTab = _driver.FindElement(
                    By.XPath("/html/body/app-layout/div/div/div/app-header/div/app-topbar/i[1]")
                );
                SettingOnDashboardTab.Click();
                WaitForUIEffect();

                // 🔄 Clear fields before input
                var consolidateInput = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.CssSelector("#kt_engage_demos > div.px-7.py-6.flex-grow-1.overflow-auto > div:nth-child(1) > input")));
                consolidateInput.Clear();
                LogStep("🧹 Cleared Consolidate Cut-Off Date field.");

                var convertInput = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.CssSelector("#kt_engage_demos > div.px-7.py-6.flex-grow-1.overflow-auto > div:nth-child(1) > input")));
                convertInput.Clear();
                LogStep("🧹 Cleared Convert Cut-Off Date field.");

                _SettingPage.EnterConvertCutOffQS(CutOffDate);
                WaitForUIEffect();
                LogStep($"Entered Convert Cut-Off Date: {CutOffDate}");

                _SettingPage.EnterCosolidateCutOffQS(ConsolidateCutOffDate);
                WaitForUIEffect();
                LogStep($"Entered Consolidate Cut-Off Date: {ConsolidateCutOffDate}");

                bool isSecurityTokenChecked = bool.TryParse(securityToken, out var result) && result;
                _SettingPage.SetCheckboxStateQS(isSecurityTokenChecked);
                WaitForUIEffect();
                LogStep($"Security Token Checkbox set to: {securityToken}");

                string filePath = AppConfig.SampleReceiptImage;
                if (!File.Exists(filePath))
                {
                    LogStep($"❌ File not found at: {filePath}");
                    Assert.Fail("File not found: " + filePath);
                }

                // Locate the hidden <input type="file"> element (not the visible button)
                var fileInput = wait.Until(ExpectedConditions.ElementExists(
                    By.CssSelector("input[type='file']")));

                // If input is hidden (display: none), make it visible temporarily using JS
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].style.display = 'block';", fileInput);

                // Send file path directly to file input — this triggers upload without pop-up
                fileInput.SendKeys(filePath);
                WaitForUIEffect(200);
                LogStep("📤 File uploaded via hidden input bypassing file picker.");

                // Proceed with the rest (crop modal, click save)
                var cropSaveBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.CssSelector("#kt_body > ngb-modal-window > div > div > app-image-crop-modal > div > div > div > button.btn.btn-primary")));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", cropSaveBtn);
                WaitForUIEffect();
                LogStep("Clicked 'Save' on crop modal.");

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
                    By.CssSelector("#kt_body > ngb-modal-window > div > div > app-image-crop-modal")));
                WaitForUIEffect();
                LogStep("Crop modal closed successfully.");

                var previewImg = wait.Until(driver =>
                {
                    try
                    {
                        var img = driver.FindElement(By.CssSelector(
                            "#kt_engage_demos > div.px-7.py-6.flex-grow-1.overflow-auto > div.d-flex.flex-column.b-7 > div.row.mb-7 > img"));
                        return !string.IsNullOrEmpty(img.GetAttribute("src")) ? img : null;
                    }
                    catch { return null; }
                });

                if (previewImg == null)
                {
                    LogStep("❌ Failed to display uploaded image preview.");
                    Assert.Fail("Image preview not found.");
                }

                WaitForUIEffect();
                LogStep("🖼️ Image preview displayed successfully.");


                _SettingPage.ClickSaveButtonQS();
                WaitForUIEffect();
                LogStep("Clicked final 'Save' button to submit settings.");

                var modal = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div")));
                string message = modal.Text.Trim();
                LogStep($"📢 System displayed message: {message}");

                string messageNormalized = message.Replace("\r", " ").Replace("\n", " ").Trim().ToLower();
                if (messageNormalized.Contains("saved") || messageNormalized.Contains("success"))
                {
                    LogStep("✅ Settings saved successfully.");
                    Assert.IsTrue(true);


                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Setting_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);


                    var okBtn = modal.FindElement(By.XPath(".//button[contains(., 'Ok, got it!')]"));
                    okBtn.Click();
                    WaitForUIEffect();
                    LogStep("✅ Acknowledged success message.");
                }
                else
                {
                    LogStep("❌ Unexpected message received after saving: " + message);
                    throw new Exception("Unexpected message: " + message);
                }
            }
            catch (Exception ex)
            {
                LogStep($"❌ An unexpected error occurred during the test: {ex.Message}");

                try
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Setting_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                    LogStep("📸 Failure screenshot captured.");
                }
                catch (Exception innerEx)
                {
                    LogStep($"⚠️ Could not capture failure screenshot: {innerEx.Message}");
                }

                Assert.Fail("Exception occurred: " + ex.Message);
            }
        }


        [Test]
        [Category("Setting")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCase("3", "4", "true")]
        public void Reset_Receipt_Image_Setting(string CutOffDate, string ConsolidateCutOffDate, string securityToken)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

            try
            {
                // Navigate to Setting On Dashboard Tab
                var SettingOnDashboardTab = _driver.FindElement(
                    By.XPath("/html/body/app-layout/div/div/div/app-header/div/app-topbar/i[1]")
                );
                SettingOnDashboardTab.Click();
                WaitForUIEffect();

                // 🔄 Clear fields before input
                var consolidateInput = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.CssSelector("#kt_engage_demos > div.px-7.py-6.flex-grow-1.overflow-auto > div:nth-child(1) > input")));
                consolidateInput.Clear();
                LogStep("🧹 Cleared Consolidate Cut-Off Date field.");

                var convertInput = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.CssSelector("#kt_engage_demos > div.px-7.py-6.flex-grow-1.overflow-auto > div:nth-child(1) > input")));
                convertInput.Clear();
                LogStep("🧹 Cleared Convert Cut-Off Date field.");

                _SettingPage.EnterConvertCutOffQS(CutOffDate);
                WaitForUIEffect();
                LogStep($"Entered Convert Cut-Off Date: {CutOffDate}");

                _SettingPage.EnterCosolidateCutOffQS(ConsolidateCutOffDate);
                WaitForUIEffect();
                LogStep($"Entered Consolidate Cut-Off Date: {ConsolidateCutOffDate}");

                bool isSecurityTokenChecked = bool.TryParse(securityToken, out var result) && result;
                _SettingPage.SetCheckboxStateQS(isSecurityTokenChecked);
                WaitForUIEffect();
                LogStep($"Security Token Checkbox set to: {securityToken}");

                // 🗑️ Try to find the Clear Image button
                IReadOnlyCollection<IWebElement> clearBtnElements = _driver.FindElements(By.XPath(
                    "/html/body/app-layout/div/div/div/app-header/div/app-topbar/app-side-setting/div/div[2]/div[4]/div[2]/div/div/button"));

                if (clearBtnElements.Count == 0)
                {
                    LogStep("ℹ️ Default image cannot be cleared — passing test.");
                    Assert.IsTrue(true,"Default image cannot be cleared.");
                    return; // stop here
                }

                var clearBtn = wait.Until(ExpectedConditions.ElementToBeClickable(clearBtnElements.First()));
                clearBtn.Click();
                WaitForUIEffect();
                LogStep("🗑️ Clicked 'Clear Image' button.");

                // ✅ Confirm reset (Save button in dialog)
                var confirmBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/div/div/div[6]/button[1]")));
                confirmBtn.Click();
                WaitForUIEffect();
                LogStep("✅ Clicked 'Save' button on reset dialog.");

                var modal = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div")));
                string message = modal.Text.Trim();
                LogStep($"📢 System displayed message: {message}");

                string messageNormalized = message.Replace("\r", " ").Replace("\n", " ").Trim().ToLower();
                if (messageNormalized.Contains("saved") || messageNormalized.Contains("success"))
                {
                    LogStep("✅ Settings saved successfully.");
                    Assert.IsTrue(true);

                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Setting_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                    var okBtn = modal.FindElement(By.XPath(".//button[contains(., 'Ok, got it!')]"));
                    okBtn.Click();
                    WaitForUIEffect();
                    LogStep("✅ Acknowledged success message.");
                }
                else
                {
                    LogStep("❌ Unexpected message received after saving: " + message);
                    throw new Exception("Unexpected message: " + message);
                }
            }
            catch (Exception ex)
            {
                LogStep($"❌ An unexpected error occurred during the test: {ex.Message}");

                try
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Setting_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                    LogStep("📸 Failure screenshot captured.");
                }
                catch (Exception innerEx)
                {
                    LogStep($"⚠️ Could not capture failure screenshot: {innerEx.Message}");
                }

                Assert.Fail("Exception occurred: " + ex.Message);
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


        private void WaitForUIEffect(int ms = 1000)
        {
            Thread.Sleep(ms);
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

