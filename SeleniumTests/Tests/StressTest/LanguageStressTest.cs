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
using System.Collections.Concurrent;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SeleniumTests.Tests.StressTest
{
    [TestFixture]
    [AllureNUnit]  // Enable Allure reporting
    [AllureSuite("Stress Tests")]  // Group this class under "Stress Tests"
    [AllureEpic("Performance Testing")]  // Assign this class to the "Performance Testing" epic
    public class LanguageStressTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private Dashboard dashboardPage;
        private TestHelper testHelper;

        [SetUp]
        [AllureStep("Setting up WebDriver and initializing TestHelper")]
        public void SetUp()
        {
            driver = DriverFactory.CreateDriver(); // Create a new instance of WebDriver
            driver.Manage().Window.Maximize();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5)); // Initialize WebDriverWait with a 5-second timeout
            dashboardPage = new Dashboard(driver); // Initialize Dashboard page object
            testHelper = new TestHelper(driver); // Initialize TestHelper
        }

        // Stress test for multiple iterations using language switch
        [Test]
        [AllureSeverity(SeverityLevel.normal)]  // Mark this test as normal severity
        [AllureOwner("Keith Chu")]  // Specify the test owner
        [AllureFeature("Language Switch")]  // Assign this test to the "Language Switch" feature
        [AllureStory("Iteration Stress Test")]  // Story for iteration-based stress testing
        [TestCase("ja", "ダッシュボード", true)]  // True positive: Japanese switch succeeds
        [TestCase("en", "Dashboard", true)]  // True positive: English switch succeeds
        public void LanguageSwitch_IterationStressTest(string languageCode, string expectedBreadCrumb, bool isValidText)
        {
            testHelper.LogTestResult($"Starting stress test for language switching: {languageCode}");

            for (int i = 0; i < 5; i++) // Iterate 5 times for stress testing
            {
                driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login"); // Navigate to the login page
                PerformLogin("admin", "password"); // Perform login

                // Switch language and verify breadcrumb text
                SwitchLanguageAndVerifyBreadCrumb(languageCode, expectedBreadCrumb, isValidText);
            }

            testHelper.LogTestResult($"Finished stress test for language switching: {languageCode}");
        }

        // Parallel stress test using language switch
        [Test]
        [AllureSeverity(SeverityLevel.critical)]  // Mark this test as critical severity
        [AllureOwner("Keith Chu")]  // Specify the test owner
        [AllureFeature("Language Switch")]  // Assign this test to the "Language Switch" feature
        [AllureStory("Parallel Stress Test")]  // Story for parallel-based stress testing
        [TestCase("ja", "ダッシュボード", true)]  // True positive: Japanese switch succeeds
        [TestCase("en", "Dashboard", true)]  // True positive: English switch succeeds
        public void LanguageSwitch_ParallelStressTest(string languageCode, string expectedBreadCrumb, bool isValidText)
        {
            testHelper.LogTestResult($"Starting parallel stress test for language switching: {languageCode}");

            var results = new ConcurrentBag<bool>();

            Parallel.For(0, 10, i =>
            {
                IWebDriver localDriver = null; // Declare WebDriver for each thread

                try
                {
                    localDriver = DriverFactory.CreateDriver(); // Create a new WebDriver instance for each thread
                    localDriver.Manage().Window.Maximize();

                    var wait = new WebDriverWait(localDriver, TimeSpan.FromSeconds(5));
                    var dashboardPage = new Dashboard(localDriver);
                    var localTestHelper = new TestHelper(localDriver); // Initialize TestHelper for the local driver

                    // Navigate to the login page and perform login
                    localDriver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login");
                    PerformLogin(localDriver, wait, "admin", "password");

                    // Switch language and verify breadcrumb text
                    SwitchLanguageAndVerifyBreadCrumb(localDriver, wait, dashboardPage, languageCode, expectedBreadCrumb, isValidText);
                    results.Add(true); // Mark iteration as success
                }
                catch (Exception ex)
                {
                    testHelper.LogTestResult($"Parallel iteration {i} failed: {ex.Message}");
                    results.Add(false); // Mark iteration as failure
                }
                finally
                {
                    localDriver?.Quit(); // Ensure WebDriver is properly closed
                }
            });

            Assert.IsTrue(results.All(r => r), "Some parallel language switches failed.");

            testHelper.LogTestResult($"Finished parallel stress test for language switching: {languageCode}");
        }

        private void PerformLogin(IWebDriver localDriver, WebDriverWait wait, string username, string password)
        {
            var loginPage = new LoginPage(localDriver);
            loginPage.EnterUsername(username);
            loginPage.EnterPassword(password);
            loginPage.ClickLoginButton();

            // Wait for dashboard URL
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("/dashboard"));
        }

        private void SwitchLanguageAndVerifyBreadCrumb(IWebDriver localDriver, WebDriverWait wait, Dashboard dashboardPage, string languageCode, string expectedBreadCrumb, bool isValidText)
        {
            dashboardPage.SwitchLanguage(languageCode); // Switch language

            // Wait for breadcrumb to update
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(dashboardPage.BreadCrumbLocator));

            var actualBreadCrumb = dashboardPage.GetBreadCrumbText(); // Get breadcrumb text

            if (isValidText)
            {
                Assert.IsTrue(actualBreadCrumb == expectedBreadCrumb, $"Expected: {expectedBreadCrumb}, but got: {actualBreadCrumb}");
            }
            else
            {
                Assert.IsFalse(actualBreadCrumb == expectedBreadCrumb, $"Unexpected match for '{expectedBreadCrumb}' when switching to language: {languageCode}");
            }
        }

        private void PerformLogin(string username, string password)
        {
            var loginPage = new LoginPage(driver);
            loginPage.EnterUsername(username);
            loginPage.EnterPassword(password);
            loginPage.ClickLoginButton();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("/dashboard")); // Wait for dashboard URL
        }

        private void SwitchLanguageAndVerifyBreadCrumb(string languageCode, string expectedBreadCrumb, bool isValidText)
        {
            dashboardPage.SwitchLanguage(languageCode); // Switch language

            // Wait for breadcrumb to update
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(dashboardPage.BreadCrumbLocator));

            var actualBreadCrumb = dashboardPage.GetBreadCrumbText(); // Get breadcrumb text

            if (isValidText)
            {
                Assert.IsTrue(actualBreadCrumb == expectedBreadCrumb, $"Expected: {expectedBreadCrumb}, but got: {actualBreadCrumb}");
            }
            else
            {
                Assert.IsFalse(actualBreadCrumb == expectedBreadCrumb, $"Unexpected match for '{expectedBreadCrumb}' when switching to language: {languageCode}");
            }
        }

        [TearDown]
        [AllureStep("Closing browser after test")]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit(); // Ensure WebDriver is properly closed
            }
        }
    }
}
