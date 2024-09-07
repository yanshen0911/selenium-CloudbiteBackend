using ERPPlus.SeleniumTests.Config;
using ERPPlus.SeleniumTests.Drivers;
using ERPPlus.SeleniumTests.Pages;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Data;
using SeleniumTests.Helpers;
using SeleniumTests.Pages;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;



namespace SeleniumTests.Tests.Functional.Login
{
    [TestFixture]
    public class LoginTest
    {
        private IWebDriver driver;
        private LoginPage loginPage;
        private WebDriverWait wait;
        private Dashboard dashboardPage;
        private LoginHelper loginHelper;
        [SetUp]
        public void SetUp()
        {
            driver = DriverFactory.CreateDriver(); // Get WebDriver instance from DriverFactory
            loginPage = new LoginPage(driver); // Initialize login page object
            dashboardPage = new Dashboard(driver); // Initialize dashboard page object
            driver.Manage().Window.Maximize();

            // Initialize WebDriverWait with a timeout of 30 seconds
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
        }

        // Data-driven test using NUnit TestCase attribute
        [Test]
        [TestCase("admin", "password", true)]  // Valid credentials
        [TestCase("admin", "wrongPassword", false)] // Invalid password
        [TestCase("wrongUser", "password", false)] // Invalid username + valid password
        [TestCase("", "", false)] // Empty username and empty password
        public void TestValidLogin(string username, string password, bool isValidLogin)
        {
            // Perform login using the helper
            loginHelper.PerformLogin(username, password);

            // Validate the login
            if (isValidLogin)
            {
                // Assert login was successful
                Assert.IsTrue(loginHelper.IsLoggedIn(), "Login failed when it should have succeeded.");
            }
            else
            {
                // Assert login failed and user is still on the login page
                Assert.IsFalse(loginHelper.IsLoggedIn(), "Login succeeded when it should have failed.");
            }
        }

        // Use test case data from TestData.cs
        [Test, TestCaseSource(typeof(LoginData), nameof(LoginData.MixedLoginData))]
        public void TestValidLogin_WithData(string username, string password, bool isValidLogin)
        {
            // Perform login using the helper
            loginHelper.PerformLogin(username, password);

            // Validate the login
            if (isValidLogin)
            {
                // Assert login was successful
                Assert.IsTrue(loginHelper.IsLoggedIn(), "Login failed when it should have succeeded.");
            }
            else
            {
                // Assert login failed and user is still on the login page
                Assert.IsFalse(loginHelper.IsLoggedIn(), "Login succeeded when it should have failed.");
            }
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit(); // Ensure WebDriver is properly closed
        }
    }
}
