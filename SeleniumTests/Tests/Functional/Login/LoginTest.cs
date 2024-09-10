using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using ERPPlus.SeleniumTests.Drivers;
using ERPPlus.SeleniumTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Helpers;
using SeleniumTests.Pages;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SeleniumTests.Tests.Functional.Login
{
    [TestFixture]
    [AllureNUnit]  // Enable Allure Reporting
    [AllureSuite("Login Tests")]  // Group this class of tests under "Login Tests"
    [AllureEpic("User Authentication")]  // Assign this class to the "User Authentication" feature
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

            // Initialize LoginHelper
            loginHelper = new LoginHelper(driver, wait);
        }

        [Test]
        [AllureSeverity(SeverityLevel.critical)]  // Mark this test as critical
        [AllureOwner("Keith Chu")]  // Specify the test owner
        [AllureFeature("Valid Login")]  // Feature related to valid logins
        [AllureStory("Login with valid credentials")]  // Specific story for this test
        [TestCase("admin", "password", true)]  // Valid credentials
        [AllureFeature("Invalid Login")]
        [AllureStory("Login with invalid password")]  // Specific story for this test
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

        [TearDown]
        [AllureStep("Closing browser after test completion")]
        public void TearDown()
        {
            driver.Quit(); // Ensure WebDriver is properly closed
        }
    }
}
