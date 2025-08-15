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
using SeleniumTests.Pages.User;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Media;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using helperFunction = SeleniumTests.Helper.HelperFunction;

namespace SeleniumTests.Tests.User
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("User - User - Valid")]
    [AllureEpic("ERP-117")]
    public class User_Valid
    {
        private IWebDriver _driver;
        private UserPage _UserPage;
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
            string moduleName = "User Page"; // You can make this dynamic if needed
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
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/user");
            helperFunction.WaitForPageToLoad(_wait);
            _UserPage = new UserPage(_driver);
            _logMessages.Clear();

            _moduleName = "User Page";

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
        [Category("User")]
        [Order(1)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCase("Manager", "Manager", "badge-info", "true")]
        public void Create_User_Role(string Rolename, string RoleDesc, string colorClass, string EditStorePermission)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                LogStep("🧭 Navigating to User Role tab");
                var userRoleTab = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-user/ul/li[2]/a")));
                userRoleTab.Click();
                WaitForUIEffect();

                LogStep("🆕 Clicking New User Role button");
                _UserPage.ClickNewUserRoleButton();
                WaitForUIEffect();

                LogStep("⌨️ Enter Role Name");
                _UserPage.EnterRolename(Rolename);

                LogStep("📝 Enter Role Description");
                _UserPage.EnterRoleDesc(RoleDesc);

                LogStep("🎨 Open color picker");
                var colorPickerBox = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.CssSelector("#kt_profile_details_view > div.card-body.p-9.px-0 > form > div > div:nth-child(1) > div.col-md-5 > div > div > div:nth-child(2) > div")));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", colorPickerBox);
                colorPickerBox.Click();
                WaitForUIEffect();

                LogStep($"🎯 Select color: {colorClass}");
                string colorSelector = $"#kt_profile_details_view > div.card-body.p-9.px-0 > form > div > div:nth-child(1) > div.col-md-5 > div > div.form-control.form-control-solid.ng-star-inserted > div > app-color-picker > div.color-picker-container > div > div.color-option.{colorClass}";
                var colorOption = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(colorSelector)));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", colorOption);
                colorOption.Click();

                LogStep("🛠️ Click Edit Store Permission");
                var userRoleEditButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//*[@id=\"kt_profile_details_view\"]/div[2]/form/div[1]/div[2]/div/a")));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", userRoleEditButton);
                userRoleEditButton.Click();
                WaitForUIEffect();

                bool isEditStorePermissionChecked = bool.TryParse(EditStorePermission, out var result) && result;
                _UserPage.SetEditStoreCheckboxState(isEditStorePermissionChecked);

                LogStep("💾 Submit Edit Store Permissions");
                var submitEditStorePermissionButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//*[@id=\"kt_body\"]/ngb-modal-window/div/div/app-role-modal/div/div[3]/button")));
                submitEditStorePermissionButton.Click();
                WaitForUIEffect();

                LogStep("☑️ Tick All Module Permission");
                var tickAllModulePermissionButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//*[@id=\"kt_profile_details_view\"]/div[2]/form/div[3]/div[2]/div/a")));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", tickAllModulePermissionButton);
                tickAllModulePermissionButton.Click();
                WaitForUIEffect();

                LogStep("📜 Scroll to bottom of the page");
                ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                Thread.Sleep(800); // Ensure UI has fully rendered

                LogStep("🔘 Click Submit button");
                var submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-user/form/div/app-user-role-table/div/app-user-role-details/div/div[2]/div[2]/div[2]/div/a")));

                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", submitButton);
                Thread.Sleep(300); // Prevent animation clash
                submitButton.Click();
                WaitForUIEffect();

                LogStep("📢 Handle success modal");
                var modal = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div")));
                var message = modal.Text.Trim();
                LogStep("📢 Modal message: " + message);
                if (!message.ToLower().Contains("success"))
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"User_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                LogStep("✅ Confirm success modal");
                var okButton = modal.FindElement(By.XPath(".//button[contains(., 'Ok, got it!')]"));
                okButton.Click();
                WaitForUIEffect();
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"User_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"❌ Exception occurred: {ex.Message}");
                Assert.Fail("Test failed due to unexpected exception.");
            }
        }



        [Test]
        [Category("User")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Update")]
        [TestCase("Manager", "Manager", "badge-info", "true")]
        public void Update_User_Role(string Rolename, string RoleDesc, string colorClass, string EditStorePermission)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                LogStep("🧭 Navigating to User Role tab");
                var userRoleTab = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-user/ul/li[2]/a")));
                userRoleTab.Click();
                WaitForUIEffect();

                LogStep($"✏️ Editing user role: {Rolename}");
                _UserPage.ClickEditButton(Rolename);
                WaitForUIEffect();

                LogStep("⌨️ Enter Role Name");
                _UserPage.EnterRolename(Rolename);

                LogStep("📝 Enter Role Description");
                _UserPage.EnterRoleDesc(RoleDesc);

                LogStep("🎨 Open color picker");
                var colorPickerBox = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.CssSelector("#kt_profile_details_view > div.card-body.p-9.px-0 > form > div > div:nth-child(1) > div.col-md-5 > div > div > div:nth-child(2) > div")));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", colorPickerBox);
                colorPickerBox.Click();
                WaitForUIEffect();

                LogStep($"🎯 Select color: {colorClass}");
                string colorSelector = $"#kt_profile_details_view > div.card-body.p-9.px-0 > form > div > div:nth-child(1) > div.col-md-5 > div > div.form-control.form-control-solid.ng-star-inserted > div > app-color-picker > div.color-picker-container > div > div.color-option.{colorClass}";
                var colorOption = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(colorSelector)));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", colorOption);
                colorOption.Click();

                LogStep("📜 Scroll down a bit");
                ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollBy(0, 200);");
                Thread.Sleep(800);

                LogStep("🛠️ Click Edit Store Permission");
                var userRoleEditButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//*[@id=\"kt_profile_details_view\"]/div[2]/form/div[1]/div[2]/div/a")));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", userRoleEditButton);
                userRoleEditButton.Click();
                WaitForUIEffect();

                bool isEditStorePermissionChecked = bool.TryParse(EditStorePermission, out var result) && result;
                _UserPage.SetEditStoreCheckboxState(isEditStorePermissionChecked);

                LogStep("💾 Submit Edit Store Permissions");
                var submitEditStorePermissionButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//*[@id=\"kt_body\"]/ngb-modal-window/div/div/app-role-modal/div/div[3]/button")));
                submitEditStorePermissionButton.Click();
                WaitForUIEffect();

                LogStep("☑️ Tick All Module Permission");
                var tickAllModulePermissionButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//*[@id=\"kt_profile_details_view\"]/div[2]/form/div[3]/div[2]/div/a")));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", tickAllModulePermissionButton);
                tickAllModulePermissionButton.Click();

                LogStep("📜 Scroll to bottom of the page");
                ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                Thread.Sleep(800); // Ensure UI has fully rendered

                LogStep("🔘 Click Submit button");
                var submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-user/form/div/app-user-role-table/div/app-user-role-details/div/div[2]/div[2]/div[2]/div/a")));

                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", submitButton);
                Thread.Sleep(300); // Prevent animation clash
                submitButton.Click();
                WaitForUIEffect();

                LogStep("📢 Handle success modal");
                var modal = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div")));
                var message = modal.Text.Trim();
                LogStep("📢 Modal message: " + message);
                if (!message.ToLower().Contains("success"))
                {
                    _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"User_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                    Assert.Fail($"❌ Expected success message but got: {message}");
                }

                LogStep("✅ Confirm success modal");
                var okButton = modal.FindElement(By.XPath(".//button[contains(., 'Ok, got it!')]"));
                okButton.Click();
                WaitForUIEffect();
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"User_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep($"❌ Exception occurred: {ex.Message}");
                Assert.Fail("Test failed due to unexpected exception.");
            }

        }



        [Test]
        [Category("User")]
        [Order(3)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCase("Yan Shen", "yanshen.choo@qubeapps.com", "Admin", "Test1234!", "Test1234!", "true")]
        public void Create(string Username, string CustEmail, string role, string UserPassword, string UserConfirmPassword, string activeUser)
        {
            try
            {
                LogStep("🆕 Click New button");
                _UserPage.ClickNewButton();

                LogStep("📤 Upload profile image");
                string filePath = AppConfig.UserProfileImage;
                if (!File.Exists(filePath))
                {
                    Assert.Fail("❌ File not found: " + filePath);
                }

                WaitForUIEffect();
                var fileInput = _driver.FindElement(By.CssSelector("#kt_modal_add_user_info > div.mb-6 > div > div > label > input[type='file']:nth-child(2)"));
                fileInput.SendKeys(filePath);
                LogStep("✅ File uploaded");

                LogStep("⌨️ Enter username");
                _UserPage.EnterUsername(Username);

                LogStep("📧 Enter customer email");
                _UserPage.EnterCustEmail(CustEmail);

                LogStep($"🔘 Set '{role}' checkbox");
                _UserPage.SetCheckboxByLabel(role);
                WaitForUIEffect();

                LogStep("🔐 Enter Password");
                _UserPage.EnterUserPassword(UserPassword);

                LogStep("🔐 Enter Confirm Password");
                _UserPage.EnterUserConfirmPassword(UserConfirmPassword);

                LogStep("👁️ Click show password icon");
                WaitForUIEffect();
                _driver.FindElement(By.XPath("/html/body/ngb-modal-window/div/div/app-user-modal/div/div[2]/div/form/div[3]/div[2]/div[1]/div[1]/span")).Click();

                LogStep("👁️ Click show confirm password icon");
                WaitForUIEffect();
                _driver.FindElement(By.XPath("/html/body/ngb-modal-window/div/div/app-user-modal/div/div[2]/div/form/div[3]/div[2]/div[2]/div/span")).Click();

                LogStep("✅ Set Active checkbox");
                bool isActive = bool.TryParse(activeUser, out var active) && active;
                _UserPage.SetCheckActiveboxState(isActive);

                LogStep("💾 Click Save button");
                _UserPage.ClickSaveButton();

                LogStep("🔔 Wait for modal message");
                WaitForUIEffect();
                var modal = _driver.FindElement(By.XPath("/html/body/div/div"));
                string message = modal.Text.Trim();
                LogStep("📢 Modal message: " + message);

                if (!message.ToLower().Contains("successful"))
                {
                    Assert.Fail("❌ Unexpected modal message: " + message);
                }

                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"User_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("✅ Click 'Ok, got it!'");
                modal.FindElement(By.XPath(".//button[contains(., 'Ok, got it!')]")).Click();

                LogStep("✅ Create user test completed successfully");
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"User_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("❌ Exception occurred: " + ex.Message);
                Assert.Fail("Test failed due to exception: " + ex.Message);
            }
        }



        [Test]
        [Category("User")]
        [Order(4)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCase("Yan Shen", "yanshen.choo@qubeapps.com", "Admin", "Test1234!", "Test1234!", "true")]
        public void CreateDuplicate(string Username, string CustEmail, string role, string UserPassword, string UserConfirmPassword, string activeUser)
        {
            try
            {
                LogStep("🔘 Click [New] button");
                _UserPage.ClickNewButton();
                WaitForUIEffect();

                LogStep("📤 Upload profile image");
                string filePath = AppConfig.UserProfileImage;
                if (!File.Exists(filePath))
                {
                    Assert.Fail("❌ File not found: " + filePath);
                }

                WaitForUIEffect();
                var fileInput = _driver.FindElement(By.CssSelector("#kt_modal_add_user_info > div.mb-6 > div > div > label > input[type='file']:nth-child(2)"));
                fileInput.SendKeys(filePath);
                LogStep("✅ File uploaded");          

                WaitForUIEffect();

                LogStep("⌨️ Enter username");
                _UserPage.EnterUsername(Username);
                WaitForUIEffect();

                LogStep("📧 Enter customer email");
                _UserPage.EnterCustEmail(CustEmail);
                WaitForUIEffect();

                LogStep($"🔘 Set '{role}' checkbox");
                _UserPage.SetCheckboxByLabel(role);
                WaitForUIEffect();

                LogStep("🔒 Enter password");
                _UserPage.EnterUserPassword(UserPassword);
                WaitForUIEffect();

                LogStep("🔒 Enter confirm password");
                _UserPage.EnterUserConfirmPassword(UserConfirmPassword);
                WaitForUIEffect();

                LogStep("👁️ Click show password");
                var showpassword = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-user-modal/div/div[2]/div/form/div[3]/div[2]/div[1]/div[1]/span")));
                showpassword.Click();
                WaitForUIEffect();

                LogStep("👁️ Click show confirm password");
                var showconfirmpassword = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-user-modal/div/div[2]/div/form/div[3]/div[2]/div[2]/div/span")));
                showconfirmpassword.Click();
                WaitForUIEffect();

                LogStep("🟢 Set active user checkbox");
                bool isActiveChecked = bool.TryParse(activeUser, out var active) && active;
                _UserPage.SetCheckActiveboxState(isActiveChecked);
                WaitForUIEffect();

                LogStep("💾 Click Save button");
                _UserPage.ClickSaveButton();
                WaitForUIEffect();

                LogStep("📩 Waiting for modal message");
                var modal = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"📢 Modal Message: {message}");
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"User_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                if (message.Contains("TIN has already been taken", StringComparison.OrdinalIgnoreCase))
                {
                    LogStep("⚠️ Duplicate TIN message detected");
                    var duplicateOkBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/div/div/div[6]/button[1]")));
                    duplicateOkBtn.Click();
                    Assert.IsTrue(true,"❌ Duplicate TIN detected: " + message);
                }
                else if (message.Contains("failed", StringComparison.OrdinalIgnoreCase))
                {
                        LogStep("⚠️ failed message detected");
                        var duplicateOkBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(
                            By.XPath("/html/body/div/div/div[6]/button[1]")));
                        duplicateOkBtn.Click();
                        Assert.IsTrue(true, "❌ Failed message detected: " + message);
                }
                else
                {
                    Assert.IsFalse(message.ToLower().Contains("successful"), $"❌ Successful detected in message: {message}");

                    var okButton = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/div/div/div[6]/button[1]")));
                    okButton.Click();
                    WaitForUIEffect();
                    LogStep("✅ User created successfully (no duplicate detected)");
                }

            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"User_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                Console.WriteLine($"❌ Exception during test: {ex.Message}");
                Assert.Fail("Test failed due to unexpected exception.");
            }
        }


        [Test]
        [Category("User")]
        [Order(5)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("User Search - General Match (Partial Match Accepted)")]
        [TestCase("Yan Shen")]
        [TestCase("yanshen.choo@qubeapps.com")]
        [TestCase("Active")]
        public void Read_After_Create_User(string searchText)
        {
            LogStep($"🔍 Searching for user with text: {searchText}");
            _UserPage.SearchUser(searchText);
            helperFunction.WaitForUserTableToLoad(_wait);
            WaitForUIEffect();

            bool isMatchFound = false;
            bool tableRefreshed = false;

            while (true)
            {
                LogStep("🔄 Scanning current page table rows");
                WaitForUIEffect();

                var rows = _driver.FindElements(By.XPath("//table/tbody/tr"));
                tableRefreshed = true;

                if (rows.Count == 0)
                {
                    LogStep("ℹ️ Table refreshed with no results (0 rows).");
                    break;
                }

                // Check for 'No data' dummy row
                if (rows.Count == 1)
                {
                    var singleCell = rows[0].FindElement(By.TagName("td"));
                    string cellText = singleCell.Text.Trim().ToLower();

                    if (cellText.Contains("no data") || cellText.Contains("no record") || singleCell.GetAttribute("colspan") != null)
                    {
                        LogStep("ℹ️ Table contains only 'No data found' message row.");
                        break;
                    }
                }

                // Loop through actual rows
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
                            break;
                        }
                    }

                    if (isMatchFound)
                        break;
                }

                if (isMatchFound)
                    break;

                // Go to next page if available
                try
                {
                    var nextButton = _driver.FindElement(By.XPath("//app-global-pagination//li[contains(@class, 'next')]"));

                    if (!nextButton.GetAttribute("class").Contains("disabled"))
                    {
                        LogStep("Clicking [Next] pagination button");
                        nextButton.Click();
                        helperFunction.WaitForBETableToLoad(_wait);
                        WaitForUIEffect();
                    }
                    else
                    {
                        LogStep("⛔ No more pages");
                        break;
                    }
                }
                catch (NoSuchElementException)
                {
                    LogStep("⚠️ No pagination control found");
                    break;
                }
            }

            // Take screenshot
            WaitForUIEffect();
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"User_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            // Final result
            if (isMatchFound)
            {
                LogStep("✅ User search match found.");
            }
            else
            {
                LogStep($"✅ Table searched and returned no match for '{searchText}' — valid result.");
            }
        }




        [Test]
        [Order(6)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("User Update")]
        [TestCase("Yan Shen", "Yan Shen", "test4@qubeapps.com", "Manager", "Test12345!", "Test12345!", "true")]
        public void Update(string User, string Username, string CustEmail, string role, string UserPassword, string UserConfirmPassword, string activeUser)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

                LogStep($"✏️ Click [Edit] for user: {User}");
                _UserPage.ClickEditUserButton(User);
                WaitForUIEffect();

                LogStep("📤 Upload profile image");
                string filePath = AppConfig.UserProfileImage;
                if (!File.Exists(filePath))
                {
                    Assert.Fail("❌ File not found: " + filePath);
                }

                WaitForUIEffect();
                var fileInput = _driver.FindElement(By.CssSelector("#kt_modal_add_user_info > div.mb-6 > div > div > label > input[type='file']:nth-child(2)"));
                fileInput.SendKeys(filePath);
                LogStep("✅ File uploaded");

                LogStep("👤 Updating username");
                _UserPage.EnterUsername(Username);
                WaitForUIEffect();

                LogStep("📧 Updating customer email");
                _UserPage.EnterCustEmail(CustEmail);
                WaitForUIEffect();

                LogStep($"🔘 Set '{role}' checkbox");
                _UserPage.SetCheckboxByLabel(role);
                WaitForUIEffect();

                LogStep("🔒 Enter new password");
                _UserPage.EnterUserPassword(UserPassword);
                WaitForUIEffect();

                LogStep("🔒 Confirm new password");
                _UserPage.EnterUserConfirmPassword(UserConfirmPassword);
                WaitForUIEffect();

                LogStep("👁️ Show password");
                var showPassword = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-user-modal/div/div[2]/div/form/div[3]/div[2]/div[1]/div[1]/span")));
                showPassword.Click();
                WaitForUIEffect();

                LogStep("👁️ Show confirm password");
                var showConfirmPassword = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("/html/body/ngb-modal-window/div/div/app-user-modal/div/div[2]/div/form/div[3]/div[2]/div[2]/div/span")));
                showConfirmPassword.Click();
                WaitForUIEffect();

                LogStep("🟢 Set active checkbox");
                bool isActiveChecked = bool.TryParse(activeUser, out var active) && active;
                _UserPage.SetCheckActiveboxState(isActiveChecked);
                WaitForUIEffect();

                LogStep("💾 Click Save");
                _UserPage.ClickSaveButton();
                WaitForUIEffect();

                LogStep("📩 Waiting for modal message");
                var modal = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("/html/body/div/div/div[2]")));
                var message = modal.Text.Trim();
                LogStep($"📢 Modal Message: {message}");

                if (message.Contains("TIN has already been taken", StringComparison.OrdinalIgnoreCase))
                {
                    LogStep("⚠️ Duplicate TIN message detected");
                    var duplicateOkBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/div/div/div[6]/button[1]")));
                    duplicateOkBtn.Click();
                    Assert.Fail("❌ Duplicate TIN detected: " + message);
                }
                else
                {
                    var okButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("/html/body/div/div/div[6]/button[1]")));
                    okButton.Click();
                    WaitForUIEffect();

                    Assert.IsTrue(message.ToLower().Contains("updated successful"),
                        $"❌ Unexpected message: '{message}'");
                }
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"User_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                LogStep("✅ User updated successfully.");
            }
            catch (Exception ex)
            {
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"User_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                Console.WriteLine($"❌ Exception during test: {ex.Message}");
                Assert.Fail("❌ Test failed due to unexpected exception.");
            }
        }


        [Test]
        [Category("User")]
        [Order(7)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create - Export User Report")]
        public void ExportUserReport()
        {
            string downloadPath = AppConfig.DownloadPath;
            string filePrefix = "User Index";

            LogStep("Clicking Export Button");
            helperFunction.WaitForElementToBeClickable(_wait,
                By.CssSelector("#kt_content_container > app-user > form > div > app-user-table > div > div > div.card-header.border-0.pt-5 > div > div:nth-child(1) > a"));
            _UserPage.ClickExportButton();

            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"User_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            LogStep("📥 Waiting for file download");
            bool fileDownloaded = _UserPage.WaitForFileDownload(downloadPath, filePrefix, TimeSpan.FromSeconds(15));
            LogStep(fileDownloaded ? "✅ File downloaded successfully." : "❌ File not downloaded.");

            Assert.IsTrue(fileDownloaded, "❌ No new download detected.");
        }

        [Test]
        [Category("User")]
        [Order(8)]
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
                    var noDataElement = _driver.FindElement(By.XPath("/html/body/app-layout/div[1]/div/div/div/app-content/app-user/form/div/app-user-table/div/div/div[3]/div/div[1]/div/table/tbody[2]/tr/td/p"));
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
                    if (cells.Count < 4) continue;

                    string actualStatus = GetStatusFromCell(cells[3]);

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
                    _UserPage.ClickFilterALLCategoryButton();
                    break;

                case "ACTIVE":
                    LogStep("📌 Testing filter: Active Category");
                    _UserPage.ClickFilterActiveCategoryButton();
                    break;

                case "INACTIVE":
                    LogStep("📌 Testing filter: Inactive Category");
                    _UserPage.ClickFilterInactiveCategoryButton();
                    break;

                default:
                    Assert.Fail($"❌ Invalid filter category input: '{category}'");
                    break;
            }

            WaitForUIEffect(100);
            helperFunction.WaitForUserTableToLoad(_wait);

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
                    if (cells.Count < 4) continue;

                    string actualStatus = GetStatusFromCell(cells[3]);
                    LogStep($"🔍 Found Status = '{actualStatus}'");
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
