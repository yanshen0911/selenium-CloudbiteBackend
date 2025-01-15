using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using ERPPlus.SeleniumTests.Config;
using ERPPlus.SeleniumTests.Drivers;
using ERPPlus.SeleniumTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Pages;
using SeleniumTests.Pages.Store;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SeleniumTests.Tests.Functional.Login
{
    [TestFixture]
    [AllureNUnit]  // Enable Allure reporting
    [AllureSuite("Login")] // use this ties to module
    [AllureEpic("ERP-79")] // use this and ties to ticket number
    public class LogoutTest
    {
        private IWebDriver driver;
        private LoginPage loginPage;
        private Pages.Dashboard dashboardPage;
        private WebDriverWait wait;

        [SetUp]
        [AllureStep("Setting up WebDriver and logging in")]
        public void SetUp()
        {
            driver = DriverFactory.CreateDriver(); // Get WebDriver instance from DriverFactory
            loginPage = new LoginPage(driver); // Initialize login page object
            dashboardPage = new Pages.Dashboard(driver); // Initialize dashboard page object
            driver.Manage().Window.Maximize();

            // Initialize WebDriverWait with a timeout of 30 seconds
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

            // Navigate to login and perform login first before testing logout
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login");
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("button.btn.primaryActionBtn.imgBtn")));

            // Perform login with valid credentials
            loginPage.EnterUsername("admin");
            loginPage.EnterPassword("password");
            loginPage.ClickLoginButton();

            // Validate successful login by checking for the dashboard URL
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("/dashboard"));
            Assert.IsTrue(driver.Url.Contains("/dashboard"), "Login failed when it should have succeeded.");
        }

        [Test]
        [AllureSeverity(SeverityLevel.normal)]  // Mark this test as normal severity
        [AllureStory("Login Logout 5")] // combo with field in excel, Module|Feature Test|Case
        public void TestLogout()
        {
            // Perform logout
            dashboardPage.Logout();

            // Validate logout by checking the login page URL
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("/login"));
            Assert.IsTrue(driver.Url.Contains("/login"), "Logout failed, user is not redirected to login page.");
        }

        [TearDown]
        [AllureStep("Closing browser after test")]
        public void TearDown()
        {
            driver.Quit(); // Ensure WebDriver is properly closed
        }
    }
}
