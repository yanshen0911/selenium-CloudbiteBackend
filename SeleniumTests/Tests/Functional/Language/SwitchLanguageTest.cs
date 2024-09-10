using Allure.NUnit.Attributes; // Add this for Allure attributes
using Allure.Net.Commons; // Add this for Allure metadata
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
using TestContext = NUnit.Framework.TestContext;
using Allure.NUnit;

namespace SeleniumTests.Tests.Functional.Language
{
    [TestFixture]
    [AllureNUnit] // This enables Allure reporting for the entire test class
    [AllureEpic("Language Switch Tests")] // Grouping the tests under an epic
    [AllureFeature("BreadCrumb Language Switch")] // Feature-level grouping
    public class SwitchLanguageTest
    {
        private IWebDriver driver;
        private LoginPage loginPage;
        private Dashboard dashboardPage;
        private WebDriverWait wait;
        private TestHelper testHelper;
        private LanguageHelper languageHelper;

        [SetUp]
        [AllureBefore("Starting Browser and Logging In")] // Describes the setup as part of the report
        public void SetUp()
        {
            driver = DriverFactory.CreateDriver();
            loginPage = new LoginPage(driver);
            dashboardPage = new Dashboard(driver);
            testHelper = new TestHelper(driver);

            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login");
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("button.btn.primaryActionBtn.imgBtn")));

            loginPage.EnterUsername("admin");
            loginPage.EnterPassword("password");
            loginPage.ClickLoginButton();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("/dashboard"));
            Assert.IsTrue(driver.Url.Contains("/dashboard"), "Login failed when it should have succeeded.");

            languageHelper = new LanguageHelper(driver, wait);
        }

        [Test]
        [AllureSeverity(SeverityLevel.critical)] // Define severity level
        [AllureOwner("KeithChu")] // Assign test owner
        [AllureStory("Switch Language and Verify Breadcrumb Text")] // Assign story under feature
        [TestCaseSource(typeof(LanguageData), nameof(LanguageData.InvalidLanguageData))]
        public void VerifyBreadCrumbText(string languageCode, string expectedText, bool isValidText)
        {
            testHelper.LogTestResult($"Starting test for language: {languageCode}");

            if (languageCode == "invalid_code")
            {
                Assert.ThrowsException<NoSuchElementException>(() => dashboardPage.SwitchLanguage(languageCode),
                    $"Expected NoSuchElementException when switching to invalid language code: {languageCode}");
                return;
            }

            dashboardPage.SwitchLanguage(languageCode);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(dashboardPage.BreadCrumbLocator));

            var breadCrumbElement = driver.FindElement(dashboardPage.BreadCrumbLocator);
            string actualBreadCrumbText = RetryUntilSuccess(() => breadCrumbElement.Text);

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

        [TearDown]
        [AllureAfter("Closing Browser")] // Describes the teardown in the report
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                testHelper.TakeScreenshot(TestContext.CurrentContext.Test.Name);
            }

            driver.Quit();
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
                        throw;
                    }
                }
            }
            return null;
        }
    }
}
