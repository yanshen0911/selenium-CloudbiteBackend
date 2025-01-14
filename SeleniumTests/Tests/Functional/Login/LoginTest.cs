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
    [AllureNUnit]  
    [AllureSuite("Login")] // use this ties to module
    [AllureEpic("ERP-79")] // use this and ties to ticket number
    public class LoginTest
    {
        private IWebDriver driver;
        private LoginPage loginPage;
        private WebDriverWait wait;
        private Dashboard dashboardPage;
        private LoginHelper loginHelper;

        [SetUp]
        [AllureStep("Setting up WebDriver")]
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
        [AllureStory("Login Login 1")]  // combo with field in excel, Module|Feature Test|Case
        [TestCase("QAS", "5162", true)]  // test data
        public void TestValidLogin(string username, string password, bool isValidLogin)
        {
            loginHelper.SelectServer("R&D SERVER 01 华语 - MALAYSIA");
            // Perform login using the helper
            loginHelper.PerformLogin(username, password, false);

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

        [Test]
        [AllureSeverity(SeverityLevel.normal)]  // Mark this test as normal
        [AllureStory("Login Login 2")] // combo with field in excel, Module|Feature Test|Case
        [TestCase("admin", "wrongPassword", false)] // test data
        public void TestInvalidLogin(string username, string password, bool isValidLogin)
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
                IList<string> validationMessages = loginHelper.GetValidationMessages();

                //// Assert login failed and user is still on the login page
                //Assert.IsFalse(loginHelper.IsLoggedIn(), "Login succeeded when it should have failed.");

                //Assert.IsTrue(validationMessages.Contains("User ID is required."), "Expected validation message 'User ID is required.' not found.");
                //Assert.IsTrue(validationMessages.Contains("Password is required."), "Expected validation message 'Password is required.' not found.");
                Assert.IsTrue(validationMessages.Contains("Server is required."), "Expected validation message 'Server is required.' not found.");
            }
        }

        [Test]
        [AllureSeverity(SeverityLevel.normal)] // Mark this test as normal
        [AllureStory("Login Login 3")] // combo with field in excel, Module|Feature Test|Case
        [TestCase("", "", false)]  // test data
        public void TestLoginWithEmptyFields(string username, string password, bool isValidLogin)
        {
            loginHelper.PerformLogin(username, password);

            if (isValidLogin)
            {
                // Assert login was successful
                Assert.IsTrue(loginHelper.IsLoggedIn(), "Login failed when it should have succeeded.");
            }
            else
            {
                // Assert login failed and validation errors are shown
                IList<string> validationMessages = loginHelper.GetValidationMessages();

                Assert.IsFalse(loginHelper.IsLoggedIn(), "Login succeeded when it should have failed.");

                // Validate that the "Server is required" message is shown
                Assert.IsTrue(validationMessages.Contains("User ID is required."), "Expected validation message 'User ID is required.' not found.");
                Assert.IsTrue(validationMessages.Contains("Password is required."), "Expected validation message 'Password is required.' not found.");
                //Assert.IsTrue(validationMessages.Contains("Server is required."), "Expected validation message 'Server is required.' not found.");
            }
        }

        [Test]
        [AllureSeverity(SeverityLevel.normal)] // Mark this test as normal
        [AllureStory("Login Login 4")] // combo with field in excel, Module|Feature Test|Case
        [TestCase("105 OR 1=1", "105 OR 1=1", false)]  // Empty username and empty password
        public void TestLoginWithSQLInjection(string username, string password, bool isValidLogin)
        {
            // Perform login using the helper
            loginHelper.PerformLogin(username, password);

            // Validate the login
            if (isValidLogin)
            {
                Assert.IsTrue(loginHelper.IsLoggedIn(), "Login failed when it should have succeeded.");
            }
            else
            {
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
