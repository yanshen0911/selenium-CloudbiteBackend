using ERPPlus.SeleniumTests.Config;
using ERPPlus.SeleniumTests.Drivers;
using ERPPlus.SeleniumTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Pages;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;



namespace SeleniumTests.Tests.Login
{
    [TestFixture]
    public class LoginTest
    {
        private IWebDriver driver;
        private LoginPage loginPage;
        private WebDriverWait wait;
        private Dashboard dashboardPage;
        [SetUp]
        public void SetUp()
        {
            driver = DriverFactory.CreateDriver(); // Get WebDriver instance from DriverFactory
            loginPage = new LoginPage(driver); // Initialize login page object
            dashboardPage = new Dashboard(driver); // Initialize dashboard page object
            driver.Manage().Window.Maximize();

            // Initialize WebDriverWait with a timeout of 30 seconds
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        // Data-driven test using NUnit TestCase attribute
        [Test]
        [TestCase("admin", "password", true)]  // Valid credentials
        [TestCase("admin", "wrongPassword", false)] // Invalid password
        [TestCase("wrongUser", "password", false)] // Invalid username + valid password
        [TestCase("", "", false)] // Empty username and empty password
        public void TestValidLogin(string username, string password, bool isValidLogin)
        {
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login"); // Base URL from config

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("button.btn.primaryActionBtn.imgBtn")));

            loginPage.EnterUsername(username);
            loginPage.EnterPassword(password);
            loginPage.ClickLoginButton();

            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login"); // Base URL from config

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("button.btn.primaryActionBtn.imgBtn")));

            loginPage.EnterUsername(username);
            loginPage.EnterPassword(password);
            loginPage.ClickLoginButton();

            if (isValidLogin)
            {
                // Validate successful login by checking for the dashboard URL
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("/dashboard"));
                Assert.IsTrue(driver.Url.Contains("/dashboard"), "Login failed when it should have succeeded.");

                // Validate the welcome message on the dashboard
                string welcomeMessage = dashboardPage.GetWelcomeMessageText();
                string BreadCrumbText = dashboardPage.GetBreadCrumbText();
                Assert.AreEqual("Welcome back to ERP+", welcomeMessage, "The welcome message did not match the expected value.");
                Assert.AreEqual("Dashboard", BreadCrumbText, "The welcome message did not match the expected value.");
            }
            else
            {
                // Validate login failure (assuming failure redirects back to login or shows error message)
                Assert.IsTrue(driver.Url.Contains("/login") || driver.PageSource.Contains("Invalid credentials"), "Login succeeded when it should have failed.");
            }

        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit(); // Ensure WebDriver is properly closed
        }
    }
}
