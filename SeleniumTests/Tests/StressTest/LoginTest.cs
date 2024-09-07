using ERPPlus.SeleniumTests.Config;
using ERPPlus.SeleniumTests.Drivers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Data;
using SeleniumTests.Helpers;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SeleniumTests.Tests.StressTest
{
    [TestFixture]
    public class StressLoginTests
    {
        private IWebDriver driver;
        private LoginHelper loginHelper;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = DriverFactory.CreateDriver(); // Get WebDriver instance from DriverFactory
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); // 10 seconds wait
            loginHelper = new LoginHelper(driver, wait); // Initialize login helper
            driver.Manage().Window.Maximize();
        }

        [Test, TestCaseSource(typeof(LoginData), nameof(LoginData.ValidLoginData))]
        public void StressTest_LoginWithValidCredentials(string username, string password)
        {
            int iterations = 100; // Define how many times you want to repeat the login

            for (int i = 0; i < iterations; i++)
            {
                // Perform login with valid credentials
                loginHelper.PerformLogin(username, password);

                // Assert login was successful
                Assert.IsTrue(loginHelper.IsLoggedIn(), $"Login failed on iteration {i + 1}");

                // Optional: Add a logout mechanism here before the next iteration
                driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/logout");
            }
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit(); // Ensure WebDriver is properly closed
        }
    }
}
