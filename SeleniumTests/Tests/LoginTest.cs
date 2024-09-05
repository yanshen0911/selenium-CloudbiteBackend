using ERPPlus.SeleniumTests.Config;
using ERPPlus.SeleniumTests.Drivers;
using ERPPlus.SeleniumTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;



namespace ERPPlus.SeleniumTests
{
    [TestFixture]
    public class LoginTest
    {
        private IWebDriver driver;
        private LoginPage loginPage;
        private WebDriverWait wait;
        [SetUp]
        public void SetUp()
        {
            driver = DriverFactory.CreateDriver(); // Get WebDriver instance from DriverFactory
            loginPage = new LoginPage(driver); // Initialize page object
            driver.Manage().Window.Maximize();

            // Initialize WebDriverWait with a timeout of 30 seconds
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        [Test]
        public void TestValidLogin()
        {
            driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login"); // Base URL from config

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("button.btn.primaryActionBtn.imgBtn")));

            loginPage.EnterUsername("admin");
            loginPage.EnterPassword("password");
            loginPage.ClickLoginButton();

            // Validate login success
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("/dashboard"));
            Assert.IsTrue(driver.Url.Contains("/dashboard"));

        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit(); // Ensure WebDriver is properly closed
        }
    }
}
