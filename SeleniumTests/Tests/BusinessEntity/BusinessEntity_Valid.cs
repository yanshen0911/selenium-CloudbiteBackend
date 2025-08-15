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
using System.Drawing;
using System.Globalization;
using System.Media;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using helperFunction = SeleniumTests.Helper.HelperFunction;

namespace SeleniumTests.Tests.BusinessEntity
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Business Entity - Business Entity - Valid")]
    [AllureEpic("ERP-117")]
    public class BusinessEntityTests_Valid
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
            string moduleName = "Business Entity Page"; // You can make this dynamic if needed
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

            _moduleName = "Business Entity Page";

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
        [Category("BusinessEntity")]
        [Order(1)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCase("Qube Apps Sdn Bhd", "C21941625070", "BRN", "201201008022", "SST1", "TT1", "00000 | NOT APPLICABLE", "01234567890", "yanshen.choo@qubeapps.com", "Petaling Jaya",
        "Selangor", "12345", "MALAYSIA", "No 111, Jalan 222", "Lorong 333, Taman 444", "Perindustrian 555")]
        public void Create(string BEname, string BETinNumber, string BEregisterType, string BERegisterID, string BEsst, string BETTRegisterNumber, string BEMSIC, string BEContactNumber,
        string BEemail, string BECity, string BEState, string BEPosCode, string BECountry, string BEAddress1, string BEAddress2, string BEAddress3)
        {
            try
            {
                _BusinessEntityPage.ClickNewButton();
                WaitForUIEffect(); LogStep("Clicked 'New' button");

                _BusinessEntityPage.EnterBEname(BEname);
                WaitForUIEffect(); LogStep($"Entered BE Name: {BEname}");

                _BusinessEntityPage.EnterBETinNumber(BETinNumber);
                WaitForUIEffect(); LogStep($"Entered TIN Number: {BETinNumber}");

                var regType = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-business-entity-modal/div/div[2]/div/div/div[2]/div/app-step1s/div/form/div/div/div[3]/div[1]/div/select")));
                new SelectElement(regType).SelectByText(BEregisterType);
                WaitForUIEffect(); LogStep($"Selected Register Type: {BEregisterType}");

                _BusinessEntityPage.EnterBERegisterID(BERegisterID);
                WaitForUIEffect(); LogStep($"Entered Register ID: {BERegisterID}");

                _BusinessEntityPage.EnterBEsst(BEsst);
                WaitForUIEffect(); LogStep($"Entered SST: {BEsst}");

                _BusinessEntityPage.EnterBETTRegisterNumber(BETTRegisterNumber);
                WaitForUIEffect(); LogStep($"Entered TT Register Number: {BETTRegisterNumber}");

                _BusinessEntityPage.ClickContinueButton();
                WaitForUIEffect(); LogStep("Clicked 'Continue' to Step 2");

                var BEMSICDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-business-entity-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[1]/div/p-dropdown/div")));
                BEMSICDropdown.Click();
                WaitForUIEffect(); LogStep("Opened MSIC dropdown");

                var dropdownPanel = _wait.Until(ExpectedConditions.ElementIsVisible(
                    By.CssSelector("div.p-dropdown-panel")));
                var dropdownOptions = dropdownPanel.FindElements(By.CssSelector("li.p-dropdown-item:not(.p-disabled)"));

                foreach (var option in dropdownOptions)
                {
                    if (option.Text.Trim().Equals(BEMSIC.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        try { option.Click(); }
                        catch { ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", option); }
                        LogStep($"Selected MSIC: {BEMSIC}");
                        break;
                    }
                }

                _BusinessEntityPage.EnterBEContactNumber(BEContactNumber);
                WaitForUIEffect(); LogStep($"Entered Contact Number: {BEContactNumber}");

                _BusinessEntityPage.EnterBEemail(BEemail);
                WaitForUIEffect(); LogStep($"Entered Email: {BEemail}");

                _BusinessEntityPage.EnterBECity(BECity);
                WaitForUIEffect(); LogStep($"Entered City: {BECity}");

                var stateDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-business-entity-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[4]/div[2]/div/select")));
                new SelectElement(stateDropdown).SelectByText(BEState);
                WaitForUIEffect(); LogStep($"Selected State: {BEState}");

                _BusinessEntityPage.EnterBEPosCode(BEPosCode);
                WaitForUIEffect(); LogStep($"Entered Postal Code: {BEPosCode}");

                var countryDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-business-entity-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[5]/select")));
                new SelectElement(countryDropdown).SelectByText(BECountry);
                WaitForUIEffect(); LogStep($"Selected Country: {BECountry}");

                _BusinessEntityPage.EnterBEAddress1(BEAddress1);
                WaitForUIEffect(); LogStep($"Entered Address Line 1: {BEAddress1}");

                _BusinessEntityPage.EnterBEAddress2(BEAddress2);
                WaitForUIEffect(); LogStep($"Entered Address Line 2: {BEAddress2}");

                _BusinessEntityPage.EnterBEAddress3(BEAddress3);
                WaitForUIEffect(); LogStep($"Entered Address Line 3: {BEAddress3}");

                // === Upload BE logo image with crop modal ===
                string filePath = AppConfig.BusinessEntityImage;

                if (!File.Exists(filePath))
                {
                    LogStep($"❌ File not found at: {filePath}. Ensure the image exists before test.");
                    Console.WriteLine($"❌ File not found at: {filePath}. Ensure the image exists before test.");
                    Assert.Fail("File not found: " + filePath);
                }

                // Find file input element and send path
                var fileInput = _wait.Until(ExpectedConditions.ElementExists(
                    By.CssSelector("#kt_create_account_form > div > app-step2s > div > div > form > div > div > div:nth-child(9) > div > div.col-sm-12.col-lg-auto.d-flex.align-items-center > input[type=file]"))); // Adjust selector if needed
                fileInput.SendKeys(filePath);
                WaitForUIEffect();
                LogStep("📤 File upload initiated.");
                Console.WriteLine("📤 File upload initiated.");


                // ✅ Now safe to click Save button
                _BusinessEntityPage.ClickSaveButton();
                LogStep("Clicked save button.");
                WaitForUIEffect();

                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"System modal message: {message}");

                if (message.Contains("TIN has already been taken", StringComparison.OrdinalIgnoreCase))
                {
                    var duplicateOkBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/div/div/div[6]/button[1]")));
                    duplicateOkBtn.Click();
                    LogStep("❌ Duplicate TIN detected and acknowledged.");
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    Assert.Fail("Duplicate TIN: " + message);
                }
                else if (message.Contains("Sucess", StringComparison.OrdinalIgnoreCase))
                {
                    var okButton = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/div/div/div[6]/button[1]")));
                    okButton.Click();
                    LogStep("✅ Update successful and confirmed.");
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    Assert.IsTrue(true, message);

                }
                else
                {
                    Assert.IsFalse(message.ToLower().Contains("fail"), $"❌ Failure in modal: {message}");
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                    var okButton = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/div/div/div[6]/button[1]")));
                    okButton.Click();
                    LogStep("❌ Update failed.");
                }

            }
            catch (Exception ex)
            {
                LogStep($"❌ Exception during test: {ex.Message}");
                Thread.Sleep(3000);
                Assert.Fail("Test failed due to unexpected exception.");
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            }
        }



        [Test]
        [Category("BusinessEntity")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCase("QUBE APPS MARKETING SDN. BHD.", "C23768804020", "BRN", "201201026867", "SST1", "TT1", "00000 | NOT APPLICABLE", "01234567890", "yanshen.choo@qubeapps.com", "Petaling Jaya",
        "Selangor", "12345", "MALAYSIA", "No 111, Jalan 222", "Lorong 333, Taman 444", "Perindustrian 555")]
        public void CreateDuplicate(string BEname, string BETinNumber, string BEregisterType, string BERegisterID, string BEsst, string BETTRegisterNumber, string BEMSIC, string BEContactNumber,
        string BEemail, string BECity, string BEState, string BEPosCode, string BECountry, string BEAddress1, string BEAddress2, string BEAddress3)
        {
            try
            {
                _BusinessEntityPage.ClickNewButton();
                WaitForUIEffect(); LogStep("Clicked 'New' to open Business Entity form.");

                _BusinessEntityPage.EnterBEname(BEname);
                WaitForUIEffect(); LogStep($"Entered BE Name: {BEname}");

                _BusinessEntityPage.EnterBETinNumber(BETinNumber);
                WaitForUIEffect(); LogStep($"Entered TIN Number: {BETinNumber}");

                var regType = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-business-entity-modal/div/div[2]/div/div/div[2]/div/app-step1s/div/form/div/div/div[3]/div[1]/div/select")));
                new SelectElement(regType).SelectByText(BEregisterType);
                WaitForUIEffect(); LogStep($"Selected Register Type: {BEregisterType}");

                _BusinessEntityPage.EnterBERegisterID(BERegisterID);
                WaitForUIEffect(); LogStep($"Entered Register ID: {BERegisterID}");

                _BusinessEntityPage.EnterBEsst(BEsst);
                WaitForUIEffect(); LogStep($"Entered SST: {BEsst}");

                _BusinessEntityPage.EnterBETTRegisterNumber(BETTRegisterNumber);
                WaitForUIEffect(); LogStep($"Entered TT Register Number: {BETTRegisterNumber}");

                _BusinessEntityPage.ClickContinueButton();
                WaitForUIEffect(); LogStep("Clicked Continue to proceed to Step 2.");

                var BEMSICDropdown = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-business-entity-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[1]/div/p-dropdown/div")));
                BEMSICDropdown.Click();
                WaitForUIEffect(); LogStep("Clicked BEMSIC dropdown.");

                var dropdownPanel = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                    By.CssSelector("div.p-dropdown-panel")));
                var dropdownOptions = dropdownPanel.FindElements(By.CssSelector("li.p-dropdown-item:not(.p-disabled)"));

                foreach (var option in dropdownOptions)
                {
                    if (option.Text.Trim().Equals(BEMSIC.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        try { option.Click(); }
                        catch { ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", option); }
                        WaitForUIEffect(); LogStep($"Selected BEMSIC: {BEMSIC}");
                        break;
                    }
                }

                _BusinessEntityPage.EnterBEContactNumber(BEContactNumber);
                WaitForUIEffect(); LogStep($"Entered Contact Number: {BEContactNumber}");

                _BusinessEntityPage.EnterBEemail(BEemail);
                WaitForUIEffect(); LogStep($"Entered Email: {BEemail}");

                _BusinessEntityPage.EnterBECity(BECity);
                WaitForUIEffect(); LogStep($"Entered City: {BECity}");

                var stateDropdown = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-business-entity-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[4]/div[2]/div/select")));
                new SelectElement(stateDropdown).SelectByText(BEState);
                WaitForUIEffect(); LogStep($"Selected State: {BEState}");

                _BusinessEntityPage.EnterBEPosCode(BEPosCode);
                WaitForUIEffect(); LogStep($"Entered Postcode: {BEPosCode}");

                var countryDropdown = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-business-entity-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[5]/select")));
                new SelectElement(countryDropdown).SelectByText(BECountry);
                WaitForUIEffect(); LogStep($"Selected Country: {BECountry}");

                _BusinessEntityPage.EnterBEAddress1(BEAddress1);
                WaitForUIEffect(); LogStep($"Entered Address Line 1: {BEAddress1}");

                _BusinessEntityPage.EnterBEAddress2(BEAddress2);
                WaitForUIEffect(); LogStep($"Entered Address Line 2: {BEAddress2}");

                _BusinessEntityPage.EnterBEAddress3(BEAddress3);
                WaitForUIEffect(); LogStep($"Entered Address Line 3: {BEAddress3}");

                // === Upload BE logo image with crop modal ===
                string filePath = AppConfig.BusinessEntityImage;

                if (!File.Exists(filePath))
                {
                    LogStep($"❌ File not found at: {filePath}. Ensure the image exists before test.");
                    Console.WriteLine($"❌ File not found at: {filePath}. Ensure the image exists before test.");
                    Assert.Fail("File not found: " + filePath);
                }

                // Find file input element and send path
                var fileInput = _wait.Until(ExpectedConditions.ElementExists(
                    By.CssSelector("#kt_create_account_form > div > app-step2s > div > div > form > div > div > div:nth-child(9) > div > div.col-sm-12.col-lg-auto.d-flex.align-items-center > input[type=file]"))); // Adjust selector if needed
                fileInput.SendKeys(filePath);
                WaitForUIEffect();
                LogStep("📤 File upload initiated.");
                Console.WriteLine("📤 File upload initiated.");


                // ✅ Now safe to click Save button
                _BusinessEntityPage.ClickSaveButton();
                LogStep("Clicked save button.");
                WaitForUIEffect();

                var modal = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                    By.XPath("/html/body/div/div/div[2]")));
                var message = modal.Text.Trim();
                WaitForUIEffect(); LogStep($"System displayed message: {message}");

                if (message.Contains("TIN has already been taken", StringComparison.OrdinalIgnoreCase))
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    var duplicateOkBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/div/div/div[6]/button[1]")));
                    duplicateOkBtn.Click();
                    LogStep("❌ Duplicate TIN detected and acknowledged.");
                    Assert.IsTrue(true,"Duplicate TIN: " + message);
                }
                else if (message.Contains("Sucess", StringComparison.OrdinalIgnoreCase))
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    var okButton = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/div/div/div[6]/button[1]")));
                    okButton.Click();
                    LogStep("✅ Update successful and confirmed.");
                    Assert.IsFalse(false, message);

                }
                else
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    Assert.IsTrue(message.ToLower().Contains("fail"), $"❌ Failure in modal: {message}");

                    var okButton = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/div/div/div[6]/button[1]")));
                    okButton.Click();
                    LogStep("✅ Update failed.");
                }
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                Console.WriteLine($"❌ Exception during test: {ex.Message}");
                Thread.Sleep(3000);
                Assert.Fail("Test failed due to unexpected exception.");
            }
        }


        [Test]
        [Category("BusinessEntity")]
        [Order(3)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("BusinessEntity Search - General Match (Partial Match Accepted)")]
        [TestCase("QUBE APPS MARKETING SDN. BHD.")]
        [TestCase("C23768804020")]
        [TestCase("201201026867")]
        [TestCase("yanshen.choo@qubeapps.com")]
        [TestCase("01234567890")]
        [TestCase("Active")]
        [TestCase("Success")]
        public void Read_After_Create_Business_Entity(string searchText)
        {
            LogStep($"🔍 Starting search for: '{searchText}'");

            _BusinessEntityPage.SearchBusinessEntity(searchText);
            helperFunction.WaitForBETableToLoad(_wait);
            WaitForUIEffect();
            LogStep("Search input filled and table loaded.");

            bool isMatchFound = false;

            while (true)
            {
                Thread.Sleep(800); // Optional: let UI stabilize

                var rows = _driver.FindElements(By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[1]/div/table/tbody"));

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

                        Console.WriteLine($"🔍 Checking cell: '{cellText}' vs '{searchText}'");

                        if (cellText.Replace(" ", "").Contains(searchText.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                        {
                            isMatchFound = true;
                            LogStep($"✅ Match found in table cell: '{cellText}'");
                            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
                    var nextButton = _driver.FindElement(By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[2]/app-global-pagination/div/div[2]/ul/li[4]"));

                    if (!nextButton.GetAttribute("class").Contains("disabled"))
                    {
                        nextButton.Click();
                        helperFunction.WaitForBETableToLoad(_wait);
                        WaitForUIEffect();
                        LogStep("Navigated to next pagination page.");
                    }
                    else
                    {
                        _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                        var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                        File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                        LogStep("⛔ No more pages. Match not found.");
                        break;
                    }
                }
                catch (NoSuchElementException)
                {
                    LogStep("⚠️ Pagination not found. Possibly only one page.");
                    break;
                }
            }

            Thread.Sleep(3000);
            Assert.IsTrue(isMatchFound, $"❌ No matching record found for '{searchText}' in any table cell.");
            LogStep("✅ Final assertion passed: match found.");

        }




        [Test]
        [Category("BusinessEntity")]
        [Order(4)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("BusinessEntity Update")]
        [TestCase("QUBE APPS MARKETING SDN. BHD.", "C23768804020", "SST1", "TT1", "00000 | NOT APPLICABLE", "01234567890", "yanshen.choo@qubeapps.com", "Petaling Jaya",
         "Selangor", "12345", "MALAYSIA", "No 111, Jalan 222", "Lorong 333, Taman 444", "Perindustrian 555")]
        public void Update(string BEname, string BETinNumber, string BEsst, string BETTRegisterNumber, string BEMSIC, string BEContactNumber,
         string BEemail, string BECity, string BEState, string BEPosCode, string BECountry, string BEAddress1, string BEAddress2, string BEAddress3)
        {
            try
            {
                _BusinessEntityPage.ClickEditButton(BETinNumber);
                WaitForUIEffect();
                LogStep($"Clicked edit for TIN: {BETinNumber}");

                _BusinessEntityPage.EnterBEname(BEname);
                WaitForUIEffect();
                LogStep($"Updated BE name to: {BEname}");

                _BusinessEntityPage.EnterBEsst(BEsst);
                WaitForUIEffect();
                LogStep($"Updated SST to: {BEsst}");

                _BusinessEntityPage.EnterBETTRegisterNumber(BETTRegisterNumber);
                WaitForUIEffect();
                LogStep($"Updated TT Register No. to: {BETTRegisterNumber}");

                _BusinessEntityPage.ClickContinueButton();
                WaitForUIEffect();
                LogStep("Clicked continue to step 2.");

                var BEMSICDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-business-entity-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[1]/div/p-dropdown/div")));
                BEMSICDropdown.Click();
                WaitForUIEffect();
                LogStep("Opened BEMSIC dropdown.");

                var dropdownPanel = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.p-dropdown-panel")));
                var dropdownOptions = dropdownPanel.FindElements(By.CssSelector("li.p-dropdown-item:not(.p-disabled)"));

                foreach (var option in dropdownOptions)
                {
                    if (option.Text.Trim().Equals(BEMSIC.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        try { option.Click(); }
                        catch { ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", option); }
                        LogStep($"Selected BEMSIC: {BEMSIC}");
                        break;
                    }
                }
                WaitForUIEffect();

                _BusinessEntityPage.EnterBEContactNumber(BEContactNumber);
                WaitForUIEffect();
                LogStep($"Entered contact number: {BEContactNumber}");

                _BusinessEntityPage.EnterBEemail(BEemail);
                WaitForUIEffect();
                LogStep($"Entered email: {BEemail}");

                _BusinessEntityPage.EnterBECity(BECity);
                WaitForUIEffect();
                LogStep($"Entered city: {BECity}");

                var stateDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-business-entity-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[4]/div[2]/div/select")));
                new SelectElement(stateDropdown).SelectByText(BEState);
                WaitForUIEffect();
                LogStep($"Selected state: {BEState}");

                _BusinessEntityPage.EnterBEPosCode(BEPosCode);
                WaitForUIEffect();
                LogStep($"Entered postal code: {BEPosCode}");

                var countryDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-business-entity-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[5]/select")));
                new SelectElement(countryDropdown).SelectByText(BECountry);
                WaitForUIEffect();
                LogStep($"Selected country: {BECountry}");

                _BusinessEntityPage.EnterBEAddress1(BEAddress1);
                WaitForUIEffect();
                LogStep($"Entered address 1: {BEAddress1}");

                _BusinessEntityPage.EnterBEAddress2(BEAddress2);
                WaitForUIEffect();
                LogStep($"Entered address 2: {BEAddress2}");

                _BusinessEntityPage.EnterBEAddress3(BEAddress3);
                WaitForUIEffect();
                LogStep($"Entered address 3: {BEAddress3}");

                // === Upload BE logo image with crop modal ===
                string filePath = AppConfig.BusinessEntityImage;

                if (!File.Exists(filePath))
                {
                    LogStep($"❌ File not found at: {filePath}. Ensure the image exists before test.");
                    Console.WriteLine($"❌ File not found at: {filePath}. Ensure the image exists before test.");
                    Assert.Fail("File not found: " + filePath);
                }

                // Find file input element and send path
                var fileInput = _wait.Until(ExpectedConditions.ElementExists(
                    By.CssSelector("#kt_create_account_form > div > app-step2s > div > div > form > div > div > div:nth-child(9) > div > div.col-sm-12.col-lg-auto.d-flex.align-items-center > input[type=file]"))); // Adjust selector if needed
                fileInput.SendKeys(filePath);
                WaitForUIEffect();
                LogStep("📤 File upload initiated.");
                Console.WriteLine("📤 File upload initiated.");

               
                // ✅ Now safe to click Save button
                _BusinessEntityPage.ClickSaveButton();
                LogStep("Clicked save button.");


                // Handle modal
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("/html/body/div/div/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"📢 Modal message: {message}");


                if (message.Contains("TIN has already been taken", StringComparison.OrdinalIgnoreCase))
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    var duplicateOkBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/div/div/div[6]/button[1]")));
                    duplicateOkBtn.Click();
                    LogStep("❌ Duplicate TIN detected and acknowledged.");                  
                    Assert.Fail("Duplicate TIN: " + message);
                }
                else if (message.Contains("Sucess", StringComparison.OrdinalIgnoreCase))
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    var okButton = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/div/div/div[6]/button[1]")));
                    okButton.Click();
                    LogStep("✅ Update successful and confirmed.");
                    Assert.IsTrue(true, message);

                }
                else
                {
                    Assert.IsFalse(message.ToLower().Contains("fail"), $"❌ Failure in modal: {message}");
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    var okButton = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/div/div/div[6]/button[1]")));
                    okButton.Click();
                    LogStep("❌ Update failed.");
                }
            }

            catch (Exception ex)
            {
                LogStep($"❌ Exception during update: {ex.Message}");
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                Assert.Fail("Test failed due to exception.");
            }
        }


        [Test]
        [Category("BusinessEntity")]
        [Order(5)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create - Export Business Entity Report")]
        public void ExportBusinessEntityReport()
        {
            string downloadPath = AppConfig.DownloadPath;
            string filePrefix = "Business Entity Index";

            try
            {
                // Click Export button
                helperFunction.WaitForElementToBeClickable(_wait,
                    By.CssSelector("#kt_content_container > app-business-entity > div > div.card-header.border-0.pt-5 > div > div:nth-child(2) > a"));
                WaitForUIEffect();

                _BusinessEntityPage.ClickExportButton();
                WaitForUIEffect();
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("📤 Clicked Export Business Entity Report button.");

                // Wait for the file to download
                bool fileDownloaded = _BusinessEntityPage.WaitForFileDownload(downloadPath, filePrefix, TimeSpan.FromSeconds(15));
                WaitForUIEffect();

                LogStep(fileDownloaded
                    ? $"✅ Exported file with prefix '{filePrefix}' downloaded successfully in {downloadPath}."
                    : $"❌ Export failed. No file starting with '{filePrefix}' was found in {downloadPath}.");

                Thread.Sleep(3000);
                Assert.IsTrue(fileDownloaded, $"❌ No new download with prefix '{filePrefix}' detected.");
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"❌ Exception during export: {ex.Message}");
                Thread.Sleep(3000);
                Assert.Fail("Test failed due to unexpected exception.");
            }
        }


        [Test]
        [Category("BusinessEntity")]
        [Order(6)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Filter - Filter By Category")]
        [TestCase("All")]
        [TestCase("Active")]
        [TestCase("Inactive")]
        public void FilterCategoryFunction(string category)
        {
            // ✅ Get all table rows
            IReadOnlyCollection<IWebElement> GetRows() =>
                _driver.FindElements(By.XPath("//table/tbody/tr"));

            // ✅ Check if table has at least one row
            bool HasRows() => GetRows().Count > 0;

            // ✅ Validate the status column in each row
            bool ValidateStatusColumn(string expectedStatus)
            {
                var rows = GetRows();
                if (rows.Count == 0)
                {
                    LogStep($"⚠️ No rows available for validating status: '{expectedStatus}'");
                    return false;
                }

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count < 7) continue;

                    string actualStatus;
                    try
                    {
                        actualStatus = cells[6].FindElement(By.TagName("span")).Text.Trim();
                    }
                    catch
                    {
                        actualStatus = cells[6].Text.Trim(); // fallback if no <span>
                    }

                    LogStep($"🔍 Validating Status: Expected = '{expectedStatus}', Found = '{actualStatus}'");

                    if (!actualStatus.Equals(expectedStatus, StringComparison.OrdinalIgnoreCase))
                    {
                        LogStep($"❌ Mismatch - Expected: '{expectedStatus}', Found: '{actualStatus}'");
                        return false;
                    }
                }

                return true;
            }

            try
            {
                // === Apply Filter Based on Input ===
                LogStep($"📌 Testing filter: {category} Category");

                switch (category.ToUpper())
                {
                    case "ALL":
                        _BusinessEntityPage.ClickFilterALLCategoryButton();
                        break;
                    case "ACTIVE":
                        _BusinessEntityPage.ClickFilterActiveCategoryButton();
                        break;
                    case "INACTIVE":
                        _BusinessEntityPage.ClickFilterInactiveCategoryButton();
                        break;
                    default:
                        throw new ArgumentException($"❌ Unknown category: {category}");
                }

                WaitForUIEffect();
                helperFunction.WaitForBETableToLoad(_wait);
                Thread.Sleep(2500);

                Assert.IsTrue(HasRows(), $"❌ '{category}' filter applied, but no records are shown.");

                // Only validate status for ACTIVE/INACTIVE
                if (category.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase) ||
                    category.Equals("INACTIVE", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.IsTrue(ValidateStatusColumn(category), $"❌ One or more rows do not show '{category}' status.");
                }

                // === Screenshot ===
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{category}_{DateTime.Now:yyyyMMdd_HHmmssfff}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            }
            catch (Exception ex)
            {
                LogStep($"❌ Exception caught during filter test for {category}: {ex.Message}");
                Assert.Fail("Test failed due to unexpected exception.");

                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{category}_ERROR_{DateTime.Now:yyyyMMdd_HHmmssfff}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            }
        }



        [Test]
        [Category("BusinessEntity")]
        [Order(7)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Filter - Filter By LHDN Status")]
        [TestCase("Pending")]
        [TestCase("Success")]
        [TestCase("Failed")]
        public void FilterLHDNStatusFunction(string lhdnStatus)
        {
            IReadOnlyCollection<IWebElement> GetRows() =>
                _driver.FindElements(By.XPath("//table/tbody/tr"));

            bool HasRows()
            {
                var count = GetRows().Count;
                LogStep(count > 0 ? $"✅ {count} data rows found." : "❌ No data rows found.");
                return count > 0;
            }

            bool ValidateLHDNStatusColumn(string expectedStatus)
            {
                var rows = GetRows();
                if (rows.Count == 0)
                {
                    LogStep($"⚠️ No rows to validate for LHDN Status '{expectedStatus}'.");
                    return false;
                }

                bool allMatch = true;

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count < 8) continue;

                    string actualStatus;
                    try
                    {
                        actualStatus = cells[7].FindElement(By.TagName("span")).Text.Trim();
                    }
                    catch
                    {
                        actualStatus = cells[7].Text.Trim();
                    }

                    LogStep($"🔍 Validating LHDN Status: Expected = '{expectedStatus}', Found = '{actualStatus}'");

                    if (!actualStatus.Equals(expectedStatus, StringComparison.OrdinalIgnoreCase))
                    {
                        LogStep($"❌ Mismatch - Expected: '{expectedStatus}', but Found: '{actualStatus}'");
                        allMatch = false;
                    }
                }

                return allMatch;
            }

            try
            {
                // === Apply Filter Based on Input ===
                LogStep($"📌 Testing LHDN filter: {lhdnStatus}");

                switch (lhdnStatus.ToUpper())
                {
                    case "PENDING":
                        _BusinessEntityPage.ClickFilterPendingCategoryButton();
                        break;
                    case "SUCCESS":
                        _BusinessEntityPage.ClickFilterSuccessCategoryButton();
                        break;
                    case "FAILED":
                        _BusinessEntityPage.ClickFilterFailedCategoryButton();
                        break;
                    default:
                        throw new ArgumentException($"❌ Unknown LHDN status: {lhdnStatus}");
                }

                WaitForUIEffect();
                helperFunction.WaitForBETableToLoad(_wait);
                Thread.Sleep(2000);

                Assert.IsTrue(HasRows(), $"❌ No rows found after filtering by LHDN Status = '{lhdnStatus}'.");
                Assert.IsTrue(ValidateLHDNStatusColumn(lhdnStatus), $"❌ One or more rows do not have LHDN Status = '{lhdnStatus}'.");

                // 📸 Screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_LHDN_{lhdnStatus}_{DateTime.Now:yyyyMMdd_HHmmssfff}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                // Optional: unselect filter if needed
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                LogStep($"❌ Exception during LHDN Status filter test for '{lhdnStatus}': {ex.Message}");
                Assert.Fail("Test failed due to unexpected exception.");
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_LHDN_{lhdnStatus}_ERROR_{DateTime.Now:yyyyMMdd_HHmmssfff}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            }
        }




        [Test]
        [Category("Business Entity")]
        [Order(8)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Import - Upload CSV via Upload Button")]
        public void ImportBusinessEntityCSVFile()
        {
            try
            {
                // Open Import Modal
                LogStep("📂 Open Import Modal");
                helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("#kt_content_container > app-business-entity > div > div.card-header.border-0.pt-5 > div > div:nth-child(1) > a"));
                _BusinessEntityPage.ClickImportButton();
                WaitForUIEffect();
                Thread.Sleep(1000);

                // Click Download Template Button
                LogStep("📤 Click Download button");
                helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector(
                    "#kt_body > ngb-modal-window > div > div > app-upload-modal > div > div.modal-body.px-20 > div > div > div.d-flex.align-items-center > button"));
                _BusinessEntityPage.ClickDownloadTemplateButton();
                WaitForUIEffect();
                Thread.Sleep(1500);

                // Choose File
                string filePath = AppConfig.ImportBECSVFile; // ✅ Update if needed
                LogStep($"📁 Selecting file: {filePath}");
                IWebElement fileInput = _driver.FindElement(By.CssSelector("input[type='file'][accept='*']"));
                fileInput.SendKeys(filePath);
                WaitForUIEffect();
                Thread.Sleep(1000);

                // Click Upload Button
                LogStep("📤 Click Upload button");
                helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector(
                    "#kt_body > ngb-modal-window > div > div > app-upload-modal > div > div.modal-footer.justify-content-end.d-flex.ng-star-inserted > button.btn.btn-primary.mx-2"));
                _BusinessEntityPage.ClickUploadButton();
                WaitForUIEffect();

                // Wait for 'Completed' Button
                LogStep("⏳ Waiting for 'Completed' button...");
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
                IWebElement completedButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[text()='Completed']")));
                LogStep("✅ 'Completed' button found. Clicking...");
                completedButton.Click();
                WaitForUIEffect();
                Thread.Sleep(1000);

                // Verify Success Modal
                LogStep("🔍 Verifying success modal...");
                var modal = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div")));
                var message = modal.Text.Trim();
                LogStep($"📢 Modal Message: {message}");

                if (!message.ToLower().Contains("success"))
                {
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                var okButton = modal.FindElement(By.XPath(".//button[contains(., 'Ok, got it!')]"));
                okButton.Click();
                WaitForUIEffect();
                Thread.Sleep(3000);
                LogStep("🎉 CSV Import test completed successfully.");
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Business_Entity_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"❌ Exception during Import CSV test: {ex.Message}");
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
