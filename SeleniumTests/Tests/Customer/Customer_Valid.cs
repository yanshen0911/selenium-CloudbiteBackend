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
using SeleniumTests.Pages.Customer;
using SeleniumTests.Pages.Stores;
using System.Drawing;
using System.Globalization;
using System.Linq.Expressions;
using System.Media;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using helperFunction = SeleniumTests.Helper.HelperFunction;

namespace SeleniumTests.Tests.Customer
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Customer - Customer - Valid")]
    [AllureEpic("ERP-117")]
    public class CustomerTests_Valid
    {
        private IWebDriver _driver;
        private CustomerPage _CustomerPage;
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
            string moduleName = "Customer Page"; // You can make this dynamic if needed
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
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/customer");
            helperFunction.WaitForPageToLoad(_wait);
            _CustomerPage = new CustomerPage(_driver);
            _logMessages.Clear();

            _moduleName = "Customer Page";

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
        [Category("Customer")]
        [Order(1)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCase("QUBE APPS MARKETING SDN. BHD.", "C23768804020", "BRN", "201201026867", "SST1", "yanshen.choo@qubeapps.com", "01234567890", "Petaling Jaya",
  "Selangor", "12345", "MALAYSIA", "No 111, Jalan 222", "Lorong 333, Taman 444", "Perindustrian 555", "Ext12345")]
        public void Create(string Custname, string CustTinNumber, string BEregisterType, string CustRegisterID, string Custsst, string CustEmail, string CustContactNumber,
     string CustCity, string CustState, string CustPosCode, string CustCountry, string CustAddress1, string CustAddress2, string CustAddress3, string CustExternalCode)
        {
            try
            {
                LogStep("Click 'New' button");
                _CustomerPage.ClickNewButton();
                WaitForUIEffect();

                LogStep($"Enter Customer Name: {Custname}");
                _CustomerPage.EnterCustname(Custname);
                WaitForUIEffect();

                LogStep($"Enter TIN Number: {CustTinNumber}");
                _CustomerPage.EnterCustTinNumber(CustTinNumber);
                WaitForUIEffect();

                LogStep($"Select Register Type: {BEregisterType}");
                var regType = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step1/div/form/div/div/div[4]/div[1]/div/select")));
                new SelectElement(regType).SelectByText(BEregisterType);
                WaitForUIEffect();

                LogStep($"Enter Register ID: {CustRegisterID}");
                _CustomerPage.EnterCustRegisterID(CustRegisterID);
                WaitForUIEffect();

                LogStep($"Enter SST: {Custsst}");
                _CustomerPage.EnterCustsst(Custsst);
                WaitForUIEffect();

                LogStep($"Enter Email: {CustEmail}");
                _CustomerPage.EnterCustEmail(CustEmail);
                WaitForUIEffect();

                LogStep("Click 'Continue' to Step 2");
                _CustomerPage.ClickContinueButton();
                WaitForUIEffect();

                LogStep($"Enter Contact Number: {CustContactNumber}");
                _CustomerPage.EnterCustContactNumber(CustContactNumber);
                WaitForUIEffect();

                LogStep($"Enter City: {CustCity}");
                _CustomerPage.EnterCustomerCity(CustCity);
                WaitForUIEffect();

                LogStep($"Select State: {CustState}");
                var stateDropdown = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step2/div/form/div/div/div[2]/div[2]/div/select")));
                new SelectElement(stateDropdown).SelectByText(CustState);
                WaitForUIEffect();

                LogStep($"Enter Postcode: {CustPosCode}");
                _CustomerPage.EnterCustPosCode(CustPosCode);
                WaitForUIEffect();

                LogStep($"Select Country: {CustCountry}");
                var countryDropdown = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step2/div/form/div/div/div[3]/select")));
                new SelectElement(countryDropdown).SelectByText(CustCountry);
                WaitForUIEffect();

                LogStep($"Enter Address 1: {CustAddress1}");
                _CustomerPage.EnterCustAddress1(CustAddress1);
                WaitForUIEffect();

                LogStep($"Enter Address 2: {CustAddress2}");
                _CustomerPage.EnterCustAddress2(CustAddress2);
                WaitForUIEffect();

                LogStep($"Enter Address 3: {CustAddress3}");
                _CustomerPage.EnterCustAddress3(CustAddress3);
                WaitForUIEffect();

                LogStep($"Enter External Code: {CustExternalCode}");
                _CustomerPage.EnterCustExternalCode(CustExternalCode);
                WaitForUIEffect();

                LogStep("Click 'Save' button");
                _CustomerPage.ClickSaveButton();
                WaitForUIEffect();

                var modal = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div")));
                var message = modal.Text.Trim();
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("Modal Message: " + message);

                if (!message.ToLower().Contains("success"))
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                var okButton = modal.FindElement(By.XPath(".//button[contains(., 'Ok, got it!')]"));
                okButton.Click();
                WaitForUIEffect();

                LogStep("✅ Customer created successfully.");
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"❌ Exception during test: {ex.Message}");
                Assert.Fail("Test failed due to unexpected exception.");
            }
        }


        [Test]
        [Category("Customer")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCase("QUBE APPS MARKETING SDN. BHD.", "C23768804020", "BRN", "201201026867", "SST1", "yanshen.choo@qubeapps.com", "01234567890", "Petaling Jaya",
        "Selangor", "12345", "MALAYSIA", "No 111, Jalan 222", "Lorong 333, Taman 444", "Perindustrian 555", "Ext12345")]
        public void CreateDuplicate(string Custname, string CustTinNumber, string BEregisterType, string CustRegisterID, string Custsst, string CustEmail, string CustContactNumber,
            string CustCity, string CustState, string CustPosCode, string CustCountry, string CustAddress1, string CustAddress2, string CustAddress3, string CustExternalCode)
        {
            try
            {
                LogStep("Click 'New' button");
                _CustomerPage.ClickNewButton();
                WaitForUIEffect();

                LogStep($"Enter Customer Name: {Custname}");
                _CustomerPage.EnterCustname(Custname);
                WaitForUIEffect();

                LogStep($"Enter TIN Number: {CustTinNumber}");
                _CustomerPage.EnterCustTinNumber(CustTinNumber);
                WaitForUIEffect();

                LogStep($"Select Register Type: {BEregisterType}");
                var regType = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step1/div/form/div/div/div[4]/div[1]/div/select")));
                new SelectElement(regType).SelectByText(BEregisterType);
                WaitForUIEffect();

                LogStep($"Enter Register ID: {CustRegisterID}");
                _CustomerPage.EnterCustRegisterID(CustRegisterID);
                WaitForUIEffect();

                LogStep($"Enter SST: {Custsst}");
                _CustomerPage.EnterCustsst(Custsst);
                WaitForUIEffect();

                LogStep($"Enter Email: {CustEmail}");
                _CustomerPage.EnterCustEmail(CustEmail);
                WaitForUIEffect();

                LogStep("Click 'Continue' to Step 2");
                _CustomerPage.ClickContinueButton();
                WaitForUIEffect();

                LogStep($"Enter Contact Number: {CustContactNumber}");
                _CustomerPage.EnterCustContactNumber(CustContactNumber);
                WaitForUIEffect();

                LogStep($"Enter City: {CustCity}");
                _CustomerPage.EnterCustomerCity(CustCity);
                WaitForUIEffect();

                LogStep($"Select State: {CustState}");
                var stateDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step2/div/form/div/div/div[2]/div[2]/div/select")));
                new SelectElement(stateDropdown).SelectByText(CustState);
                WaitForUIEffect();

                LogStep($"Enter Postcode: {CustPosCode}");
                _CustomerPage.EnterCustPosCode(CustPosCode);
                WaitForUIEffect();

                LogStep($"Select Country: {CustCountry}");
                var countryDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step2/div/form/div/div/div[3]/select")));
                new SelectElement(countryDropdown).SelectByText(CustCountry);
                WaitForUIEffect();

                LogStep($"Enter Address 1: {CustAddress1}");
                _CustomerPage.EnterCustAddress1(CustAddress1);
                WaitForUIEffect();

                LogStep($"Enter Address 2: {CustAddress2}");
                _CustomerPage.EnterCustAddress2(CustAddress2);
                WaitForUIEffect();

                LogStep($"Enter Address 3: {CustAddress3}");
                _CustomerPage.EnterCustAddress3(CustAddress3);
                WaitForUIEffect();

                LogStep($"Enter External Code: {CustExternalCode}");
                _CustomerPage.EnterCustExternalCode(CustExternalCode);
                WaitForUIEffect();

                LogStep("Click 'Save' button");
                _CustomerPage.ClickSaveButton();
                WaitForUIEffect();

                // Modal message validation
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div/div[2]")));
                string message = modal.Text.Trim();

                // Always take screenshot
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("📢 Modal Message: " + message);

                var okButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div/div/div[6]/button[1]")));
                okButton.Click();

                if (message.Contains("TIN has already been taken", StringComparison.OrdinalIgnoreCase))
                {
                    LogStep("✅ Duplicate TIN correctly detected.");
                    Assert.IsTrue(true, "Duplicate handling worked as expected.");
                }
                else if (message.ToLower().Contains("success"))
                {
                    Assert.Fail("❌ Unexpected success message. Duplicate should not be allowed.");
                }
                else if (message.ToLower().Contains("fail"))
                {
                    Assert.IsTrue(true, "Expected failure message detected.");
                }
                else
                {
                    Assert.Fail("❌ Unexpected modal message: " + message);
                }

                LogStep("✅ Customer duplicate creation check completed.");
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"❌ Exception during duplicate test: {ex.Message}");
                Assert.Fail("Test failed due to unexpected exception.");
            }
        }



        [Test]
        [Category("Customer")]
        [Order(3)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Customer Search - General Match (Partial Match Accepted)")]
        [TestCase("CUST_000001")]
        [TestCase("Qube Solution")]
        [TestCase("qubeapps@email.com")]
        [TestCase("0123456789")]
        [TestCase("Active")]
        [TestCase("Success")]
        public void Read_After_Create_Customer(string searchText)
        {
            LogStep($"🔍 Searching for text: {searchText}");
            _CustomerPage.SearchCustomer(searchText);
            helperFunction.WaitForCustTableToLoad(_wait);
            WaitForUIEffect();

            bool isMatchFound = false;

            while (true)
            {
                LogStep("Checking table rows on current page...");
                WaitForUIEffect();

                var rows = _driver.FindElements(By.XPath(
                    "/html/body/app-layout/div/div/div/div/app-content/app-customer/div/div[4]/div/div[1]/div/table/tbody"));

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));

                    foreach (var cell in cells)
                    {
                        string cellText;
                        try
                        {
                            // Attempt to get inner span text if styled
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
                            break;
                        }
                    }

                    if (isMatchFound) break;
                }

                if (isMatchFound)
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    LogStep("✅ Match found.");
                    break;
                }

                // Attempt to click the Next button (pagination)
                try
                {
                    var nextButton = _driver.FindElement(By.XPath(
                        "/html/body/app-layout/div/div/div/div/app-content/app-customer/div/div[4]/div/div[2]/app-global-pagination/div/div[2]/ul/li[4]"));

                    if (!nextButton.GetAttribute("class").Contains("disabled"))
                    {
                        _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                        var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                        File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                        LogStep("Navigating to next page...");
                        nextButton.Click();
                        helperFunction.WaitForBETableToLoad(_wait);
                        WaitForUIEffect();
                    }
                    else
                    {
                        _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                        var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                        File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                        LogStep("⛔ Reached last page. No more pages to check.");
                        break;
                    }
                }
                catch (NoSuchElementException)
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    LogStep("⚠️ No pagination found. Stopping search.");
                    break;
                }
            }

            WaitForUIEffect();
            Assert.IsTrue(isMatchFound, $"❌ No matching record found for: '{searchText}' in any table cell.");
        }




        [Test]
        [Category("Customer")]
        [Order(4)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Customer Update")]
        [TestCase("CUST_000001", "Qube Solution", "C23768804020", "BRN", "201201026867", "SST1", "qubeapps@email.com", "0123456789", "Petaling Jaya",
"Johor", "46000", "MALAYSIA", "No 111, Jalan 222", "Lorong 333, Taman 444", "Perindustrian 555", "Ext12345")]
        public void Update(string CustomerCode, string Custname, string CustTinNumber, string BEregisterType, string CustRegisterID, string Custsst, string CustEmail, string CustContactNumber,
string CustCity, string CustState, string CustPosCode, string CustCountry, string CustAddress1, string CustAddress2, string CustAddress3, string CustExternalCode)
        {
            try
            {
                LogStep("⏳ Clicking edit button for customer: " + CustomerCode);
                _CustomerPage.ClickEditButton(CustomerCode);
                WaitForUIEffect();

                LogStep("⏳ Updating customer details...");
                _CustomerPage.EnterCustname(Custname);
                WaitForUIEffect();

                _CustomerPage.EnterCustTinNumber(CustTinNumber);
                WaitForUIEffect();

                var regType = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step1/div/form/div/div/div[4]/div[1]/div/select")));
                new SelectElement(regType).SelectByText(BEregisterType);
                WaitForUIEffect();

                _CustomerPage.EnterCustRegisterID(CustRegisterID);
                WaitForUIEffect();

                _CustomerPage.EnterCustsst(Custsst);
                WaitForUIEffect();

                _CustomerPage.EnterCustEmail(CustEmail);
                WaitForUIEffect();

                LogStep("Proceeding to step 2...");
                _CustomerPage.ClickContinueButton();
                WaitForUIEffect();

                _CustomerPage.EnterCustContactNumber(CustContactNumber);
                WaitForUIEffect();

                _CustomerPage.EnterCustomerCity(CustCity);
                WaitForUIEffect();

                var stateDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step2/div/form/div/div/div[2]/div[2]/div/select")));
                new SelectElement(stateDropdown).SelectByText(CustState);
                WaitForUIEffect();

                _CustomerPage.EnterCustPosCode(CustPosCode);
                WaitForUIEffect();

                var countryDropdown = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step2/div/form/div/div/div[3]/select")));
                new SelectElement(countryDropdown).SelectByText(CustCountry);
                WaitForUIEffect();

                _CustomerPage.EnterCustAddress1(CustAddress1);
                WaitForUIEffect();

                _CustomerPage.EnterCustAddress2(CustAddress2);
                WaitForUIEffect();

                _CustomerPage.EnterCustAddress3(CustAddress3);
                WaitForUIEffect();

                _CustomerPage.EnterCustExternalCode(CustExternalCode);
                WaitForUIEffect();

                LogStep("💾 Saving updated customer record...");
                _CustomerPage.ClickSaveButton();
                WaitForUIEffect();

                // ✅ Modal validation
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div/div[2]")));
                var message = modal.Text.Trim();
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("📢 Modal message: " + message);

                if (message.Contains("TIN has already been taken", StringComparison.OrdinalIgnoreCase))
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                    var duplicateOkBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div/div/div[6]/button[1]")));
                    duplicateOkBtn.Click();

                    Assert.Fail("❌ Duplicate TIN error: " + message);
                }
                else if (message.ToLower().Contains("fail"))
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                    var duplicateOkBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div/div/div[6]/button[1]")));
                    duplicateOkBtn.Click();

                    Assert.Fail("❌ Unexpected failure in modal: " + message);
                }
                else
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                    Assert.IsTrue(message.IndexOf("Successful", StringComparison.OrdinalIgnoreCase) >= 0, $"❌ Modal does not indicate success: {message}");

                    var okButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div/div/div[6]/button[1]")));
                    okButton.Click();
                    WaitForUIEffect();

                    LogStep("✅ Customer update completed successfully.");
                }

            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"❌ Exception during update: {ex.Message}");
                Assert.Fail("Test failed due to unexpected exception.");
            }
        }


        [Test]
        [Category("StoreGroup")]
        [Order(5)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create - Export Customer Report")]
        public void ExportCustomerReport()
        {
            string downloadPath = AppConfig.DownloadPath;
            string filePrefix = "Customer Index";

            LogStep("📤 Clicking export button for customer report...");
            helperFunction.WaitForElementToBeClickable(_wait,
                By.CssSelector("#kt_content_container > app-customer > div > div.card-header.border-0.pt-5 > div > div:nth-child(2) > a"));
            _CustomerPage.ClickExportButton();

            LogStep("⏳ Waiting for download to complete...");
            bool fileDownloaded = _CustomerPage.WaitForFileDownload(downloadPath, filePrefix, TimeSpan.FromSeconds(15));

            // 📸 Always capture screenshot
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            if (!fileDownloaded)
            {
                Assert.Fail("❌ Export file was not found in the Downloads folder.");
            }
            else
            {
                LogStep("✅ Export file successfully detected in Downloads folder.");
            }
        }



        [Test]
        [Category("Customer")]
        [Order(6)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Filter - Filter By Category")]
        [TestCase("ALL")]
        [TestCase("ACTIVE")]
        [TestCase("INACTIVE")]
        public void FilterCategoryFunction(string category)
        {
            IReadOnlyCollection<IWebElement> GetRows() =>
                _driver.FindElements(By.XPath("//table/tbody[1]/tr"));

            string GetStatusFromCell(IWebElement cell)
            {
                try
                {
                    var spanText = cell.FindElement(By.TagName("span")).Text.Trim();
                    if (!string.IsNullOrEmpty(spanText))
                        return spanText;
                }
                catch { }

                try
                {
                    var directText = cell.Text.Trim();
                    if (!string.IsNullOrEmpty(directText))
                        return directText;
                }
                catch { }

                return string.Empty;
            }

            bool IsNoDataMessageShown()
            {
                try
                {
                    var noDataElement = _driver.FindElement(By.XPath("/html/body/app-layout/div[1]/div/div/div/app-content/app-customer/div/div[4]/div/div[1]/div/table/tbody[2]/tr/td/p"));
                    string message = noDataElement?.Text?.Trim();
                    return message != null && message.Equals("No data available", StringComparison.OrdinalIgnoreCase);
                }
                catch
                {
                    return false;
                }
            }

            void ValidateStatusColumn(string expectedStatus)
            {
                var rows = GetRows();
                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count < 6) continue;

                    string actualStatus = GetStatusFromCell(cells[5]);

                    LogStep($"🔍 Validating Status: Expected = '{expectedStatus}', Found = '{actualStatus}'");

                    if (!actualStatus.Equals(expectedStatus, StringComparison.OrdinalIgnoreCase))
                    {
                        LogStep($"❌ Mismatch - Expected: '{expectedStatus}', Found: '{actualStatus}'");
                        Assert.Fail("❌ One or more rows have unexpected status.");
                    }
                }
            }

            // === Apply Filter ===
            switch (category.Trim().ToUpperInvariant())
            {
                case "ALL":
                    LogStep("📌 Testing filter: All Categories");
                    _CustomerPage.ClickFilterALLCategoryButton();
                    break;

                case "ACTIVE":
                    LogStep("📌 Testing filter: Active Category");
                    _CustomerPage.ClickFilterActiveCategoryButton();
                    break;

                case "INACTIVE":
                    LogStep("📌 Testing filter: Inactive Category");
                    _CustomerPage.ClickFilterInactiveCategoryButton();
                    break;

                default:
                    Assert.Fail($"❌ Invalid filter category input: '{category}'");
                    break;
            }

            WaitForUIEffect();
            helperFunction.WaitForCustTableToLoad(_wait);

            // === Screenshot ===
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            LogStep($"📸 Screenshot saved to: {_lastScreenshotPath}");

            // === Handle "No Data" Scenario ===
            if (IsNoDataMessageShown())
            {
                LogStep($"✅ Filter applied: '{category}' - No records found.");
                LogStep("📤 Export: No data found. Test passed as no invalid data shown.");
                return;
            }

            var rows = GetRows();
            if (!rows.Any())
            {
                Assert.Fail("❌ Expected data rows but none were found.");
            }

            // === Validate based on category ===
            if (category.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase))
            {
                ValidateStatusColumn("Active");
            }
            else if (category.Equals("INACTIVE", StringComparison.OrdinalIgnoreCase))
            {
                ValidateStatusColumn("Inactive");
            }
            else if (category.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count < 6) continue;

                    string actualStatus = GetStatusFromCell(cells[5]);
                    LogStep($"🔍 Found Status = '{actualStatus}'");
                }
            }
        }




        [Test]
        [Category("Customer")]
        [Order(7)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Filter - Filter By LHDN Status")]
        [TestCase("Pending")]
        [TestCase("Success")]
        [TestCase("Failed")]
        public void FilterLHDNStatusFunction(string lhdnStatus)
        {
            // === Get only data rows from tbody[1] ===
            IReadOnlyCollection<IWebElement> GetRows() =>
                _driver.FindElements(By.XPath("//table/tbody[1]/tr"));

            // === Check if "No data available" is shown ===
            bool IsNoDataMessageShown()
            {
                try
                {
                    var noDataElement = _driver.FindElement(By.XPath("//table/tbody[2]/tr/td/p"));
                    var message = noDataElement.Text.Trim();
                    return message.Equals("No data available", StringComparison.OrdinalIgnoreCase);
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            }

            // === Validate all rows match expected status ===
            bool AllRowsMatchExpectedStatus(string expectedStatus)
            {
                var rows = GetRows();
                bool allMatch = true;

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
                        actualStatus = cells[6].Text.Trim();
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

            // === Apply Filter ===
            switch (lhdnStatus.Trim().ToLowerInvariant())
            {
                case "pending":
                    LogStep("🟡 Filtering by LHDN Status: Pending");
                    _CustomerPage.ClickFilterPendingCategoryButton();
                    break;
                case "success":
                    LogStep("🟢 Filtering by LHDN Status: Success");
                    _CustomerPage.ClickFilterSuccessCategoryButton();
                    break;
                case "failed":
                    LogStep("🔴 Filtering by LHDN Status: Failed");
                    _CustomerPage.ClickFilterFailedCategoryButton();
                    break;
                default:
                    Assert.Fail($"❌ Invalid LHDN status input: '{lhdnStatus}'");
                    break;
            }

            // === Wait and Screenshot ===
            helperFunction.WaitForCustTableToLoad(_wait);
            WaitForUIEffect();

            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            // === If "No data available" is displayed, pass the test ===
            if (IsNoDataMessageShown())
            {
                LogStep($"✅ 'No data available' is shown for '{lhdnStatus}' filter. Test succeeded.");
                return; // Exit the test early — pass
            }

            // === If data exists, validate all rows ===
            var dataRows = GetRows();
            if (dataRows.Count == 0)
            {
                Assert.Fail("❌ No data rows found, and no 'No data available' message. Possible UI rendering issue.");
            }

            Assert.IsTrue(AllRowsMatchExpectedStatus(lhdnStatus), $"❌ Some rows do not match the '{lhdnStatus}' status.");
        }




        [Test]
        [Category("Customer")]
        [Order(8)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Import - Upload CSV via Upload Button")]
        public void ImportCustomerCSVFile()
        {
            try
            {
                LogStep("📤 Opening Import Modal...");
                helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector(
                    "#kt_content_container > app-customer > div > div.card-header.border-0.pt-5 > div > div:nth-child(1) > a"));
                _CustomerPage.ClickImportButton();
                WaitForUIEffect();

                LogStep("📤 Click Download button");
                helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector(
                    "#kt_body > ngb-modal-window > div > div > app-upload-modal > div > div.modal-body.px-20 > div > div > div.d-flex.align-items-center > button"));
                _CustomerPage.ClickDownloadTemplateButton();
                WaitForUIEffect();
                Thread.Sleep(1500);

                LogStep("📂 Uploading Customer CSV File...");
                string filePath = AppConfig.ImportCustomerCSVFile; // ✅ Update if needed
                IWebElement fileInput = _driver.FindElement(By.CssSelector("input[type='file'][accept='*']"));
                fileInput.SendKeys(filePath);
                LogStep($"✅ File selected: {filePath}");
                WaitForUIEffect();

                LogStep("✅ Clicking Upload button...");
                helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector(
                    "#kt_body > ngb-modal-window > div > div > app-upload-modal > div > div.modal-footer.justify-content-end.d-flex.ng-star-inserted > button.btn.btn-primary.mx-2"));
                _CustomerPage.ClickUploadButton();

                LogStep("⏳ Waiting for 'Completed' button...");
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
                IWebElement completedButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[text()='Completed']")));
                LogStep("✅ 'Completed' button detected. Clicking...");
                completedButton.Click();
                WaitForUIEffect();

                LogStep("⏳ Waiting for modal success message...");
                var modal = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div")));
                var message = modal.Text.Trim();
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("📢 Modal Message: " + message);

                if (!message.ToLower().Contains("success"))
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                var okButton = modal.FindElement(By.XPath(".//button[contains(., 'Ok, got it!')]"));
                okButton.Click();
                WaitForUIEffect();
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Customer_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"❌ Exception during CSV import: {ex.Message}");
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
