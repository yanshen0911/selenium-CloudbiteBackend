using ERPPlus.SeleniumTests.Config;
using ERPPlus.SeleniumTests.Drivers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Helpers;
using SeleniumTests.Data;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTests.Tests.StressTest
{
    [TestFixture]
    public class LanguageStressTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private LanguageHelper languageHelper;

        [SetUp]
        public void SetUp()
        {
            driver = DriverFactory.CreateDriver(); // Get WebDriver instance from DriverFactory
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); // Set WebDriver wait time
            languageHelper = new LanguageHelper(driver, wait); // Initialize language helper
            driver.Manage().Window.Maximize();
        }

        // Use test data from LanguageData.cs
        [Test, TestCaseSource(typeof(LanguageData), nameof(LanguageData.ValidLanguageData))]
        public void StressTest_SwitchLanguageMultipleTimes(string languageCode, string expectedBreadCrumb)
        {
            int iterations = 100; // Define how many times you want to switch languages

            for (int i = 0; i < iterations; i++)
            {
                // Switch between languages in each iteration
                languageHelper.SwitchLanguage(languageCode);

                // Wait for the page to update with the selected language
                wait.Until(ExpectedConditions.TextToBePresentInElementLocated(
                    By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/app-dashboard/div[1]/div/div"),
                    expectedBreadCrumb));

                // Log success for each iteration
                Console.WriteLine($"Iteration {i + 1}: Language '{languageCode}' switched successfully to breadcrumb '{expectedBreadCrumb}'.");
            }
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit(); // Ensure WebDriver is properly closed
        }
    }
}
