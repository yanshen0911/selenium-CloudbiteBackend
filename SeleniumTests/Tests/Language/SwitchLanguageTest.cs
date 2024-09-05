using ERPPlus.SeleniumTests.Config;
using ERPPlus.SeleniumTests.Drivers;
using ERPPlus.SeleniumTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Pages;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SeleniumTests.Tests.Language
{
    [TestFixture]
    public class SwitchLanguageTest
    {
        private IWebDriver driver;
        private LoginPage loginPage;
        private Dashboard dashboardPage;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = DriverFactory.CreateDriver(); // Get WebDriver instance from DriverFactory
            loginPage = new LoginPage(driver); // Initialize login page object
            dashboardPage = new Dashboard(driver); // Initialize dashboard page object
            driver.Manage().Window.Maximize();

            // Initialize WebDriverWait with a timeout of 30 seconds
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

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
        [TestCase("en", "Dashboard", true)]  // English valid case
        [TestCase("zh", "仪表板", true)]  // Chinese valid case
        [TestCase("ja", "ダッシュボード", true)]  // Japanese valid case
        [TestCase("en", "仪表板", false)]  // English breadcrumb should not be in Chinese
        [TestCase("zh", "Dashboard", false)]  // Chinese breadcrumb should not be in English
        [TestCase("ja", "仪表板", false)]  // Japanese breadcrumb should not be in Chinese
        public void VerifyBreadCrumbText(string languageCode, string expectedText, bool isValidText)
        {
            // Switch to the desired language
            dashboardPage.SwitchLanguage(languageCode);

            // Wait for the breadcrumb element to be present (no specific text wait)
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(dashboardPage.BreadCrumbLocator));

            // Get the actual breadcrumb text after language change
            string actualBreadCrumbText = dashboardPage.GetBreadCrumbText();

            if (isValidText)
            {
                // Positive case: The breadcrumb text should match the expected text
                Assert.AreEqual(expectedText, actualBreadCrumbText, $"Breadcrumb text did not match for language: {languageCode}");
            }
            else
            {
                // Negative case: The breadcrumb text should NOT match the expected text
                Assert.AreNotEqual(expectedText, actualBreadCrumbText, $"Breadcrumb text unexpectedly matched for language: {languageCode}");
            }
        }


        [TearDown]
        public void TearDown()
        {
            driver.Quit(); // Ensure WebDriver is properly closed
        }
    }
}
