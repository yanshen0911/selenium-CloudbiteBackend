using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using ERPPlus.SeleniumTests.Config;
using ERPPlus.SeleniumTests.Drivers;
using ERPPlus.SeleniumTests.Pages;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumTests.Helpers;
using SeleniumTests.Pages;
using SeleniumTests.Pages.Store;
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
        private Pages.Dashboard dashboardPage;
        private LoginHelper loginHelper;

        [SetUp]
        [AllureStep("Setting up WebDriver")]
        public void SetUp()
        {
            driver = DriverFactory.CreateDriver(); // Get WebDriver instance from DriverFactory
            loginPage = new LoginPage(driver); // Initialize login page object
            dashboardPage = new Pages.Dashboard(driver); // Initialize dashboard page object
            driver.Manage().Window.Maximize();

            // Initialize WebDriverWait with a timeout of 30 seconds
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

            // Initialize LoginHelper
            loginHelper = new LoginHelper(driver, wait);
        }

        [Test]
        [AllureSeverity(SeverityLevel.critical)]  // Mark this test as critical
        [AllureStory("Login Login 1")]  // combo with field in excel, Module|Feature Test|Case
        [TestCase("QASQA", "5162", "QA - FireBird 2.5 - MALAYSIA", true)]  // test data
        public void TestValidLogin(string username, string password, string serverName, bool isValidLogin)
        {
            loginHelper.SelectServer(serverName);
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
        [TestCase("QASQA", "wrongPassword", "QA - FireBird 2.5 - MALAYSIA")] // test data
        public void TestInvalidLogin(string username, string password, string serverName)
        {
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login");

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app-root/body/app-login/mat-drawer-container/mat-drawer-content/div/div[1]/div[2]/form/div[4]/button")));
            loginHelper.SelectServer(serverName);
            loginPage.EnterUsername(username);
            loginPage.EnterPassword(password);
            loginPage.ClickLoginButton();

            // Assert login failed and validation errors are shown
            IList<string> validationMessages = loginHelper.GetValidationMessages();

            //Assert.IsFalse(loginHelper.IsLoggedIn(), "Login succeeded when it should have failed.");
            // Validate that the "Server is required" message is shown
            Assert.IsTrue(validationMessages.Contains("User ID is required."), "Expected validation message 'User ID is required.' not found.");
            Assert.IsTrue(validationMessages.Contains("Password is required."), "Expected validation message 'Password is required.' not found.");
            Assert.IsTrue(validationMessages.Contains("Server is required."), "Expected validation message 'Server is required.' not found.");

            //currently the error message were shown in alert, pls raise ticket for soft dev
            // regardless of invalid user name or not, we are not supposed to show, instead, a generic error message should be show such as
            // "invalid user id or password, please try again"
        }

        [Test]
        [AllureSeverity(SeverityLevel.normal)] // Mark this test as normal
        [AllureStory("Login Login 3")] // combo with field in excel, Module|Feature Test|Case
        [TestCase("", "", false)]  // test data
        public void TestLoginWithEmptyFields(string username, string password, bool isValidLogin)
        {
            //if (isNavigateToLogin)
            //{
            //    _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login");
            //}

            //// Wait for the login button to be visible
            //_wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app-root/body/app-login/mat-drawer-container/mat-drawer-content/div/div[1]/div[2]/form/div[4]/button")));

            //// Perform login actions
            //_loginPage.EnterUsername(username);
            //_loginPage.EnterPassword(password);
            //_loginPage.ClickLoginButton();

            //// Wait for the dashboard URL to confirm successful login
            //_wait.Until(ExpectedConditions.UrlContains("/dashboard"));

            if (isValidLogin)
            {
                // Assert login was successful
                Assert.IsTrue(loginHelper.IsLoggedIn(), "Login failed when it should have succeeded.");
            }
            else
            {
                driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login");

                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app-root/body/app-login/mat-drawer-container/mat-drawer-content/div/div[1]/div[2]/form/div[4]/button")));

                loginPage.EnterUsername(username);
                loginPage.EnterPassword(password);
                loginPage.ClickLoginButton();

                // Assert login failed and validation errors are shown
                IList<string> validationMessages = loginHelper.GetValidationMessages();

                //Assert.IsFalse(loginHelper.IsLoggedIn(), "Login succeeded when it should have failed.");
                // Validate that the "Server is required" message is shown
                Assert.IsTrue(validationMessages.Contains("User ID is required."), "Expected validation message 'User ID is required.' not found.");
                Assert.IsTrue(validationMessages.Contains("Password is required."), "Expected validation message 'Password is required.' not found.");
                Assert.IsTrue(validationMessages.Contains("Server is required."), "Expected validation message 'Server is required.' not found.");
            }
        }

        [Test]
        [AllureSeverity(SeverityLevel.normal)] // Mark this test as normal
        [AllureStory("Login Login 4")] // combo with field in excel, Module|Feature Test|Case
        [TestCase("105 OR 1=1", "105 OR 1=1", "QA - FireBird 2.5 - MALAYSIA")]  // Empty username and empty password
        public void TestLoginWithSQLInjection(string username, string password, string serverName)
        {
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login");

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app-root/body/app-login/mat-drawer-container/mat-drawer-content/div/div[1]/div[2]/form/div[4]/button")));
            loginHelper.SelectServer(serverName);
            loginPage.EnterUsername(username);
            loginPage.EnterPassword(password);
            loginPage.ClickLoginButton();

            // Assert login failed and validation errors are shown
            IList<string> validationMessages = loginHelper.GetValidationMessages();

            //Assert.IsFalse(loginHelper.IsLoggedIn(), "Login succeeded when it should have failed.");
            // Validate that the "Server is required" message is shown
            Assert.IsTrue(validationMessages.Contains("User ID is required."), "Expected validation message 'User ID is required.' not found.");
            Assert.IsTrue(validationMessages.Contains("Password is required."), "Expected validation message 'Password is required.' not found.");
            Assert.IsTrue(validationMessages.Contains("Server is required."), "Expected validation message 'Server is required.' not found.");

            //currently the error message were shown in alert, pls raise ticket for soft dev
            // regardless of invalid user name or not, we are not supposed to show, instead, a generic error message should be show such as
            // "invalid user id or password, please try again"
        }

        [TearDown]
        [AllureStep("Closing browser after test completion")]
        public void TearDown()
        {
            driver.Quit(); // Ensure WebDriver is properly closed
        }
    }
}
