using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using ERPPlus.SeleniumTests.Config;
using ERPPlus.SeleniumTests.Drivers;
using NUnit.Allure.Core;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Data;
using SeleniumTests.Helpers;
using AllureNUnitAttribute = Allure.NUnit.AllureNUnitAttribute;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SeleniumTests.Tests.StressTest
{
    [TestFixture]
    [AllureNUnit]  // Enable Allure reporting
    [AllureSuite("Stress Tests")]  // Group this class under "Stress Tests"
    [AllureEpic("Performance Testing")]  // Assign this class to the "Performance Testing" epic
    public class StressLoginTests
    {
        private IWebDriver driver;
        private LoginHelper loginHelper;
        private WebDriverWait wait;

        [SetUp]
        [AllureStep("Setting up WebDriver and initializing LoginHelper")]
        public void SetUp()
        {
            driver = DriverFactory.CreateDriver(); // Get WebDriver instance from DriverFactory
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); // 10 seconds wait
            loginHelper = new LoginHelper(driver, wait); // Initialize login helper
            driver.Manage().Window.Maximize();
        }

        [Test]
        [AllureSeverity(SeverityLevel.normal)]  // Mark this test as normal severity
        [AllureOwner("Keith Chu")]  // Specify the test owner
        [AllureFeature("Login Functionality")]  // Assign this test to the "Login Functionality" feature
        [AllureStory("Stress Testing - Multiple Iterations")]  // Story for stress testing login with valid credentials
        [TestCaseSource(typeof(LoginData), nameof(LoginData.ValidLoginData))]
        public void StressTest_LoginWithValidCredentials(string username, string password)
        {
            int iterations = 5; // Define how many times you want to repeat the login
            AllureLifecycle.Instance.WrapInStep(() =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    // Perform login with valid credentials
                    loginHelper.PerformLogin(username, password);

                    // Assert login was successful
                    Assert.IsTrue(loginHelper.IsLoggedIn(), $"Login failed on iteration {i + 1}");

                    // Optional: Add a logout mechanism here before the next iteration
                    driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/logout");
                }
            }, $"Stress Testing Login with {username}");
        }

        [TearDown]
        [AllureStep("Closing browser after test")]
        public void TearDown()
        {
            driver.Quit(); // Ensure WebDriver is properly closed
        }
    }
}
