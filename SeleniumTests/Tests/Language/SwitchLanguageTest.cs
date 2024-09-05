using ERPPlus.SeleniumTests.Config;
using ERPPlus.SeleniumTests.Drivers;
using ERPPlus.SeleniumTests.Pages;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Pages;
using System;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using TestContext = NUnit.Framework.TestContext;

namespace SeleniumTests.Tests.Language
{
    [TestFixture]
    public class SwitchLanguageTest
    {
        private IWebDriver driver;
        private LoginPage loginPage;
        private Dashboard dashboardPage;
        private WebDriverWait wait;
        private TestHelper testHelper;

        [SetUp]
        public void SetUp()
        {
            driver = DriverFactory.CreateDriver(); // Get WebDriver instance from DriverFactory
            loginPage = new LoginPage(driver); // Initialize login page object
            dashboardPage = new Dashboard(driver); // Initialize dashboard page object
            testHelper = new TestHelper(driver); // Initialize the TestHelper

            driver.Manage().Window.Maximize();

            // Initialize WebDriverWait with a timeout of 30 seconds
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

            // Navigate to login and perform login first before testing
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
        [TestCase("en", "Dashboard", true)]  // True positive: English switch succeeds, correct text displayed
        [TestCase("zh", "Dashboard", false)]  // False positive: Chinese switch should not display English text
        [TestCase("invalid_code", "Dashboard", false)]  // True negative: Invalid language should not succeed
        [TestCase("ja", "ダッシュボード", true)]  // True positive: Japanese switch succeeds, correct text displayed
        [TestCase("ja", "Dashboard", false)]  // False negative: Japanese switch incorrectly displays English text
        public void VerifyBreadCrumbText(string languageCode, string expectedText, bool isValidText)
        {
            testHelper.LogTestResult($"Starting test for language: {languageCode}");

            dashboardPage.SwitchLanguage(languageCode);

            // Wait for the breadcrumb element to be present
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(dashboardPage.BreadCrumbLocator));

            // Get the actual breadcrumb text after language change
            string actualBreadCrumbText = dashboardPage.GetBreadCrumbText();

            // Assert using IsTrue and IsFalse for YES/NO operations
            if (isValidText)
            {
                // Assert that the actual text matches the expected text (True positive case)
                Assert.IsTrue(actualBreadCrumbText == expectedText, $"Expected '{expectedText}', but got '{actualBreadCrumbText}' for language: {languageCode}");
            }
            else
            {
                // Assert that the actual text does NOT match the expected text (True negative or False positive case)
                Assert.IsFalse(actualBreadCrumbText == expectedText, $"Unexpected match for '{expectedText}' when switching to language: {languageCode}");
            }

            testHelper.LogTestResult($"Finished test for language: {languageCode}");
        }

        [TearDown]
        public void TearDown()
        {
            // Capture a screenshot on failure
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                testHelper.TakeScreenshot(TestContext.CurrentContext.Test.Name);
            }

            driver.Quit(); // Ensure WebDriver is properly closed
        }
    }
}
