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
using System;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using TestContext = NUnit.Framework.TestContext;

namespace SeleniumTests.Tests.Functional.Language
{
    [TestFixture]
    public class SwitchLanguageTest
    {
        private IWebDriver driver;
        private LoginPage loginPage;
        private Dashboard dashboardPage;
        private WebDriverWait wait;
        private TestHelper testHelper;
        private LanguageHelper languageHelper;

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

            languageHelper = new LanguageHelper(driver, wait);

        }

        //[Test]
        //[TestCase("en", "Dashboard", true)]  // True positive: English switch succeeds, correct text displayed
        //[TestCase("zh", "Dashboard", false)]  // False positive: Chinese switch should not display English text
        //[TestCase("invalid_code", "Dashboard", false)]  // True negative: Invalid language should not succeed
        //[TestCase("ja", "Dashboard", false)]  // False negative: Japanese switch incorrectly displays English text
        //[TestCase("ja", "ダッシュボード", true)]  // True positive: Japanese switch succeeds, correct text displayed
        //[Test, TestCaseSource(typeof(LanguageData), nameof(LanguageData.MixedLanguageData))]
        [Test, TestCaseSource(typeof(LanguageData), nameof(LanguageData.InvalidLanguageData))]
        public void VerifyBreadCrumbText(string languageCode, string expectedText, bool isValidText)
        {
            testHelper.LogTestResult($"Starting test for language: {languageCode}");

            if (languageCode == "invalid_code")
            {
                // Expecting an exception for invalid language code
                Assert.ThrowsException<NoSuchElementException>(() => dashboardPage.SwitchLanguage(languageCode), 
                    $"Expected NoSuchElementException when switching to invalid language code: {languageCode}");
                return; // Exit the test early since the invalid language is expected to fail
            }

            // Switch the language
            dashboardPage.SwitchLanguage(languageCode);

            // Wait for the breadcrumb element to be present after the language change
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(dashboardPage.BreadCrumbLocator));

            // After the page refresh, re-locate the breadcrumb element
            var breadCrumbElement = driver.FindElement(dashboardPage.BreadCrumbLocator);

            // Retry mechanism for the breadcrumb text in case of stale element
            string actualBreadCrumbText = RetryUntilSuccess(() =>
            {
                return breadCrumbElement.Text;
            });

            // Assert the correctness of the breadcrumb text
            if (isValidText)
            {
                Assert.IsTrue(actualBreadCrumbText == expectedText, $"Expected '{expectedText}', but got '{actualBreadCrumbText}' for language: {languageCode}");
            }
            else
            {
                Assert.IsFalse(actualBreadCrumbText == expectedText, $"Unexpected match for '{expectedText}' when switching to language: {languageCode}");
            }

            testHelper.LogTestResult($"Finished test for language: {languageCode}");
        }

        public string RetryUntilSuccess(Func<string> action, int retryCount = 3)
        {
            for (int attempt = 0; attempt < retryCount; attempt++)
            {
                try
                {
                    return action();
                }
                catch (StaleElementReferenceException)
                {
                    if (attempt == retryCount - 1)
                    {
                        throw;  // Rethrow the exception if retries are exhausted
                    }
                }
            }
            return null;
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
