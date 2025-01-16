using Allure.NUnit.Attributes;
using Allure.Net.Commons;
using ERPPlus.SeleniumTests.Config;
using ERPPlus.SeleniumTests.Drivers;
using ERPPlus.SeleniumTests.Pages;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers; // Required for ExpectedConditions
using SeleniumTests.Data;
using SeleniumTests.Helpers;
using SeleniumTests.Pages;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using TestContext = NUnit.Framework.TestContext;
using Allure.NUnit;
using SeleniumTests.Pages.Store;
using System;
using helperFunction = SeleniumTests.Helper.HelperFunction;

namespace QASErpPlusAutomation.Tests.Store
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Store")] // use this ties to module
    [AllureEpic("ERP-117")] // use this and ties to ticket number
    public class StoreCountryTests
    {
        private IWebDriver _driver;
        private StoreCountryPage _storeCountryPage;
        private WebDriverWait _wait;
        private LoginHelper _loginHelper;

        [SetUp]
        [AllureBefore("Starting Browser and Logging In")] // Describes the setup as part of the report
        public void SetUp()
        {
            _driver = DriverFactory.CreateDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));

            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login");

            _loginHelper = new LoginHelper(_driver, _wait);
            _loginHelper.SelectServer(AppConfig.ServerName);
            _loginHelper.PerformLogin(AppConfig.UserName, AppConfig.Password, false);

            helperFunction.WaitForPageToLoad(_wait);
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/store-stepper/store-country");
            helperFunction.WaitForPageToLoad(_wait);

            _storeCountryPage = new StoreCountryPage(_driver);
        }

        [Test]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Store Country 1")]
        public void TestCreateNewStoreCountry()
        {
            string _strCode, _strDesc, _strOtherDesc;
            _strCode = "MY8";
            _strDesc = _strCode + " DESC";
            _strOtherDesc = _strCode + " OTHER DESC";

            // Step 1: Wait for and click the "New" button
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _storeCountryPage.ClickNewButton();

            // Step 2: Enter values in the form fields
            _storeCountryPage.EnterCode(_strCode);
            _storeCountryPage.EnterDescription(_strDesc);
            _storeCountryPage.EnterOtherDescription(_strOtherDesc);
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Country", "1");
            // Step 3: Click the "Save" button
            _storeCountryPage.ClickSaveButton();

            // Step 4: Wait for the alert message to appear
            var alertElement = helperFunction.WaitForElementToBeVisible(_wait, By.CssSelector("div[role='alert']"));
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Country", "1");

            // Step 5: Extract and verify the alert text
            string alertText = alertElement.Text;
            alertText = alertText.ToUpper();
            Console.WriteLine("Alert Message: " + alertText);
            Assert.IsTrue(alertText.Contains("SUCCESS"), "The save operation was not successful.");

            // Optional: Wait for the URL to redirect (if needed)
            helperFunction.WaitForUrlToContain(_wait, "/store-stepper/store-country");
        }

        [Test]
        [AllureSeverity(SeverityLevel.critical)] // Define severity level
        [AllureOwner("KeithChu")] // Assign test owner
        [AllureStory("Language Switch Language 1")] // Assign story under feature
        public void TestSearchStoreCountry()
        {
            string _strCode, _strDesc, _strOtherDesc;
            _strCode = "MY8";
            _strDesc = _strCode + " DESC";
            _strOtherDesc = _strCode + " OTHER DESC";

            helperFunction.WaitForElementToBeClickable( _wait, By.CssSelector("button.primaryActionBtn"));
            _storeCountryPage.SearchStoreCountry(_strCode);
            helperFunction.WaitForTableToLoad(_wait);
            Assert.IsTrue(_driver.PageSource.Contains(_strCode));
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver.Quit();
            }
        }
    }
}
