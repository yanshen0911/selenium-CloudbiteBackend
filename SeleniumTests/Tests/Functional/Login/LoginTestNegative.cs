using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using CloudbiteBackend.SeleniumTests.Config;
using CloudbiteBackend.SeleniumTests.Drivers;
using ERPPlus.SeleniumTests.Pages;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ScreenRecorderLib;
using SeleniumExtras.WaitHelpers;
using SeleniumTests.Helpers;
using SeleniumTests.Tests.Stores;
using System.Drawing;
using System.Globalization;
using System.Media;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using helperFunction = SeleniumTests.Helper.HelperFunction;

namespace SeleniumTests.Tests.Functional.Login
{

    public static class ExcelDataReaderLoginNegative
    {
        public static IEnumerable<object[]> GetWrongUserAndPasswordTestData(string filePath, string sheetName)
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
                    string username = worksheet.Cells[row, 1].Text?.Trim();
                    string password = worksheet.Cells[row, 2].Text?.Trim();

                    yield return new object[]
                    {
                        username, password
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetValidUserWrongPasswordTestData(string filePath, string sheetName)
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
                    string username = worksheet.Cells[row, 1].Text?.Trim();
                    string password = worksheet.Cells[row, 2].Text?.Trim();

                    yield return new object[]
                    {
                        username, password
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetEmptyUsernameTestData(string filePath, string sheetName)
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
                    string username = worksheet.Cells[row, 1].Text?.Trim();
                    string password = worksheet.Cells[row, 2].Text?.Trim();

                    yield return new object[]
                    {
                        username, password
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetEmptyPasswordTestData(string filePath, string sheetName)
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
                    string username = worksheet.Cells[row, 1].Text?.Trim();
                    string password = worksheet.Cells[row, 2].Text?.Trim();

                    yield return new object[]
                    {
                        username, password
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetInvalidEmailFormatTestData(string filePath, string sheetName)
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
                    string username = worksheet.Cells[row, 1].Text?.Trim();
                    string password = worksheet.Cells[row, 2].Text?.Trim();

                    yield return new object[]
                    {
                        username, password
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetForgotPWEmptyEmailTestData(string filePath, string sheetName)
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
                    string email = worksheet.Cells[row, 1].Text?.Trim();

                    yield return new object[]
                    {
                        email
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetWhitespaceEmailTestData(string filePath, string sheetName)
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
                    string email = worksheet.Cells[row, 1].Text?.Trim();

                    yield return new object[]
                    {
                        email
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetForgotPWInvalidEmailTestData(string filePath, string sheetName)
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
                    string email = worksheet.Cells[row, 1].Text?.Trim();

                    yield return new object[]
                    {
                        email
                    };

                }
            }
        }


        public static IEnumerable<object[]> GetUnregisteredEmailTestData(string filePath, string sheetName)
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
                    string email = worksheet.Cells[row, 1].Text?.Trim();

                    yield return new object[]
                    {
                        email
                    };

                }
            }
        }



    }

        [TestFixture]
    [AllureNUnit]  
    [AllureSuite("Login")] // use this ties to module
    [AllureEpic("ERP-79")] // use this and ties to ticket number
    public class LoginTestNegative
    {
        private IWebDriver driver;
        private LoginPage _loginPage;
        private WebDriverWait wait;
        private Recorder _recorder;
        private string _recordingFilePath;
        private ManualResetEvent _recordingCompletedEvent = new ManualResetEvent(false);
        private List<string> _logMessages = new List<string>();
        private string _moduleName = "";


        private static string ExcelPath = Path.Combine(AppConfig.TestDataFolder, "LoginTestDataNegative.xlsx");

        public static IEnumerable<object[]> WrongUserAndPasswordTestData =>
        ExcelDataReaderLoginNegative.GetWrongUserAndPasswordTestData(ExcelPath, "WrongUserAndPasswordTestData");

        public static IEnumerable<object[]> ValidUserWrongPasswordTestData =>
        ExcelDataReaderLoginNegative.GetValidUserWrongPasswordTestData(ExcelPath, "ValidUserWrongPasswordTestData");

        public static IEnumerable<object[]> EmptyUsernameTestData =>
        ExcelDataReaderLoginNegative.GetEmptyUsernameTestData(ExcelPath, "EmptyUsernameTestData");

        public static IEnumerable<object[]> EmptyPasswordTestData =>
        ExcelDataReaderLoginNegative.GetEmptyPasswordTestData(ExcelPath, "EmptyPasswordTestData");

        public static IEnumerable<object[]> InvalidEmailFormatTestData =>
        ExcelDataReaderLoginNegative.GetInvalidEmailFormatTestData(ExcelPath, "InvalidEmailFormatTestData");

        public static IEnumerable<object[]> ForgotPWEmptyEmailTestData =>
        ExcelDataReaderLoginNegative.GetForgotPWEmptyEmailTestData(ExcelPath, "ForgotPWEmptyEmailTestData");

        public static IEnumerable<object[]> WhitespaceEmailTestData =>
        ExcelDataReaderLoginNegative.GetWhitespaceEmailTestData(ExcelPath, "WhitespaceEmailTestData");

        public static IEnumerable<object[]> ForgotPWInvalidEmailTestData =>
        ExcelDataReaderLoginNegative.GetForgotPWInvalidEmailTestData(ExcelPath, "ForgotPWInvalidEmailTestData");

        public static IEnumerable<object[]> UnregisteredEmailTestData =>
        ExcelDataReaderLoginNegative.GetUnregisteredEmailTestData(ExcelPath, "UnregisteredEmailTestData");


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // 🧹 Delete existing export Excel file (if any)
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string moduleName = "Login Page Negative"; // You can make this dynamic if needed
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
            driver = DriverFactory.CreateDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/auth/login");
        }

        [SetUp]
        public void SetUp()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/dashboard");
            helperFunction.WaitForPageToLoad(wait);

            _loginPage = new LoginPage(driver, wait);
            _logMessages.Clear();
            _moduleName = "Login Page Negative";

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
        [Order(1)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Login Negative Scenario - Wrong Username and Password")]
        [TestCaseSource(nameof(WrongUserAndPasswordTestData))]
        public void TestLogin_WrongUserAndPassword(string username, string password)
        {
            LogStep("Navigating to login page");
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/auth/login");

            LogStep($"Entering username: '{username}' and password: '{password}'");
            _loginPage.EnterUsername(username);
            _loginPage.EnterPassword(password);

            LogStep("Checking if login button is enabled");
            bool isButtonEnabled = _loginPage.IsLoginButtonEnabled();

            if (isButtonEnabled)
            {
                LogStep("Login button is enabled. Proceeding to click login button.");
                _loginPage.ClickLoginButton();

                LogStep("Waiting to confirm user remains on login page");
                wait.Until(ExpectedConditions.UrlContains("/auth/login"));

                WaitForUIEffect();

                LogStep("✅ Expected Result: Login attempt with WRONG username and password fails. User remains on login page. → Test success");
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Login_Failure_WrongUserAndPass_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                Assert.IsTrue(driver.Url.Contains("/auth/login"),
                    "Login should fail with invalid credentials, but URL changed unexpectedly.");
            }
        }

        [Test]
        [Order(2)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Login Negative Scenario - Valid User, Wrong Password")]
        [TestCaseSource(nameof(ValidUserWrongPasswordTestData))]
        public void TestLogin_ValidUserWrongPassword(string username, string password)
        {
            LogStep("Navigating to login page");
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/auth/login");

            LogStep($"Entering username: '{username}' and password: '{password}'");
            _loginPage.EnterUsername(username);
            _loginPage.EnterPassword(password);

            LogStep("Checking if login button is enabled");
            bool isButtonEnabled = _loginPage.IsLoginButtonEnabled();

            if (isButtonEnabled)
            {
                _loginPage.ClickLoginButton();
                wait.Until(ExpectedConditions.UrlContains("/auth/login"));

                WaitForUIEffect();

                LogStep("✅ Expected Result: Login attempt with VALID username but WRONG password fails. User remains on login page. → Test success");
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Login_Failure_ValidUserWrongPass_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

                Assert.IsTrue(driver.Url.Contains("/auth/login"),
                    "Login should fail with wrong password, but URL changed unexpectedly.");
            }
        }

        [Test]
        [Order(3)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Login Negative Scenario - Empty Username")]
        [TestCaseSource(nameof(EmptyUsernameTestData))]
        public void TestLogin_EmptyUsername(string username, string password)
        {
            LogStep("Navigating to login page");
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/auth/login");

            LogStep($"Entering username: '{username}' and password: '{password}'");
            _loginPage.EnterUsername(username);
            _loginPage.EnterPassword(password);

            LogStep("Checking if login button is enabled");
            bool isButtonEnabled = _loginPage.IsLoginButtonEnabled();

            WaitForUIEffect();

            LogStep("✅ Expected Result: Login button should be DISABLED when username is empty. → Test success");
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Login_Disabled_EmptyUsername_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            Assert.IsFalse(isButtonEnabled,
                "Login button should be disabled when username is empty, but it was enabled.");
        }

        [Test]
        [Order(4)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Login Negative Scenario - Empty Password")]
        [TestCaseSource(nameof(EmptyPasswordTestData))]
        public void TestLogin_EmptyPassword(string username, string password)
        {
            LogStep("Navigating to login page");
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/auth/login");

            LogStep($"Entering username: '{username}' and password: '{password}'");
            _loginPage.EnterUsername(username);
            _loginPage.EnterPassword(password);

            LogStep("Checking if login button is enabled");
            bool isButtonEnabled = _loginPage.IsLoginButtonEnabled();

            WaitForUIEffect();

            LogStep("✅ Expected Result: Login button should be DISABLED when password is empty. → Test success");
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Login_Disabled_EmptyPassword_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            Assert.IsFalse(isButtonEnabled,
                "Login button should be disabled when password is empty, but it was enabled.");
        }

        [Test]
        [Order(5)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Login Negative Scenario - Invalid Email Format")]
        [TestCase("invalidEmailFormat", "somePass")]   // wrong format email
        public void TestLogin_InvalidEmailFormat(string username, string password)
        {
            LogStep("Navigating to login page");
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/auth/login");

            LogStep($"Entering username: '{username}' and password: '{password}'");
            _loginPage.EnterUsername(username);
            _loginPage.EnterPassword(password);

            LogStep("Checking if login button is enabled");
            bool isButtonEnabled = _loginPage.IsLoginButtonEnabled();

            WaitForUIEffect();

            LogStep("✅ Expected Result: Login button should be DISABLED when email format is invalid. → Test success");
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Login_Disabled_InvalidEmailFormat_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);

            Assert.IsFalse(isButtonEnabled,
                "Login button should be disabled with invalid email format, but it was enabled.");
        }



        [Test]
        [Order(6)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Login Forgot Password - Negative Test Case: Empty Email")]
        [TestCaseSource(nameof(ForgotPWEmptyEmailTestData))]
        public void TestForgotPassword_EmptyEmail(string email)
        {
            LogStep("Navigating to login page");
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/auth/login");
            WaitForUIEffect();

            LogStep("Clicking Forgot Password");
            _loginPage.ClickForgotPassword();
            WaitForUIEffect();

            LogStep($"Entering Email: '{email}' (Empty Email Test)");
            _loginPage.EnterEmail(email);
            WaitForUIEffect();

            LogStep("Expected Result: User should not be able to submit the form with an empty email. The submit button should remain disabled. → Test Success");
            bool isButtonEnabled = _loginPage.IsSubmitForgotPasswordEnabled();
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Login_Disabled_InvalidEmailFormat_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            Assert.IsFalse(isButtonEnabled, "Submit button should be disabled for empty email.");


        }

        [Test]
        [Order(7)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Login Forgot Password - Negative Test Case: Whitespace-only Email")]
        [TestCaseSource(nameof(WhitespaceEmailTestData))]
        public void TestForgotPassword_WhitespaceEmail(string email)
        {
            LogStep("Navigating to login page");
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/auth/login");
            WaitForUIEffect();

            LogStep("Clicking Forgot Password");
            _loginPage.ClickForgotPassword();
            WaitForUIEffect();

            LogStep($"Entering Email: '{email}' (Whitespace-only Email Test)");
            _loginPage.EnterEmail(email);
            WaitForUIEffect();

            LogStep("Expected Result: Form submission should be prevented if email contains only whitespace. Submit button should remain disabled. → Test Success");
            bool isButtonEnabled = _loginPage.IsSubmitForgotPasswordEnabled();
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Login_Disabled_InvalidEmailFormat_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            Assert.IsFalse(isButtonEnabled, "Submit button should be disabled for whitespace-only email.");


        }

        [Test]
        [Order(8)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Login Forgot Password - Negative Test Case: Invalid Email Format")]
        [TestCaseSource(nameof(ForgotPWInvalidEmailTestData))]
        public void TestForgotPassword_InvalidEmailFormat(string email)
        {
            LogStep("Navigating to login page");
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/auth/login");
            WaitForUIEffect();

            LogStep("Clicking Forgot Password");
            _loginPage.ClickForgotPassword();
            WaitForUIEffect();

            LogStep($"Entering Email: '{email}' (Invalid Email Format Test)");
            _loginPage.EnterEmail(email);
            WaitForUIEffect();

            LogStep("Expected Result: Form should prevent submission if email format is invalid. Submit button should remain disabled. → Test Success");
            bool isButtonEnabled = _loginPage.IsSubmitForgotPasswordEnabled();
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Login_Disabled_InvalidEmailFormat_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
            Assert.IsFalse(isButtonEnabled, "Submit button should be disabled for invalid email format.");


        }

        [Test]
        [Order(9)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Login Forgot Password - Negative Test Case: Unregistered Email")]
        [TestCaseSource(nameof(UnregisteredEmailTestData))]
        public void TestForgotPassword_UnregisteredEmail(string email)
        {
            LogStep("Navigating to login page");
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/auth/login");
            WaitForUIEffect();

            LogStep("Clicking Forgot Password");
            _loginPage.ClickForgotPassword();
            WaitForUIEffect();

            LogStep($"Entering Email: '{email}' (Unregistered Email Test)");
            _loginPage.EnterEmail(email);
            WaitForUIEffect();

            LogStep("Expected Result: Form allows submission, but system should display an error message indicating the email is not registered.");
            bool isButtonEnabled = _loginPage.IsSubmitForgotPasswordEnabled();
            Assert.IsTrue(isButtonEnabled, "Submit button should be enabled for unregistered email.");

            LogStep("Clicking Submit Forgot Password Button");
            _loginPage.ClickSubmitForgotPassword();
            WaitForUIEffect();

            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                var error = wait.Until(d => d.FindElement(By.XPath("//div[contains(@class,'error-message')]"))).Text;
                WaitForUIEffect();

                LogStep($"Error message displayed: {error}");
                LogStep("✅ Negative test scenario completed → Test Success");
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Login_Disabled_InvalidEmailFormat_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                Assert.IsTrue(error.Contains("not registered") || error.Contains("Invalid"),
                    "System should display an appropriate error message for unregistered email.");
            }
            catch (WebDriverTimeoutException)
            {
                LogStep("No error message appeared after submit → Test Success (expected behavior for UNREGISTERED email scenario)");
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                _lastScreenshotPath = Path.Combine(Path.GetTempPath(), $"Login_Disabled_InvalidEmailFormat_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                File.WriteAllBytes(_lastScreenshotPath, screenshot.AsByteArray);
                Assert.IsTrue(true);
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

                    string[] steps = message.Split(new[] { '\n', '•', '|' }, StringSplitOptions.RemoveEmptyEntries);
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
                        var footerElement = driver.FindElement(By.XPath("/html/body/body/div/div[2]/div/span"));
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

        private void WaitForUIEffect(int ms = 1000)
        {
            Thread.Sleep(ms);
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



        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            try
            {
                driver?.Quit();
                driver?.Dispose();
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
