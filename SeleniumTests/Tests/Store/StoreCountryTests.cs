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

namespace QASErpPlusAutomation.Tests.Store
{
    public class StoreCountryTests
    {
        private IWebDriver _driver;
        private StoreCountryPage _storeCountryPage;
        private LoginPage _loginPage;
        private WebDriverWait _wait;
        private TestHelper _testHelper;
        private LoginHelper _loginHelper;

        [SetUp]
        public void SetUp()
        {
            _driver = DriverFactory.CreateDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            _loginPage = new LoginPage(_driver);
            _testHelper = new TestHelper(_driver);

            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login");

            _loginHelper = new LoginHelper(_driver, _wait);
            _loginHelper.SelectServer("R&D SERVER 01 华语 - MALAYSIA");
            _loginHelper.PerformLogin(AppConfig.UserName, AppConfig.Password, false);

            WaitForPageToLoad();
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/store-stepper/store-country");
            WaitForPageToLoad();

            _storeCountryPage = new StoreCountryPage(_driver);
        }

        [Test]
        public void TestSearchStoreCountry()
        {
            string _strCode, _strDesc, _strOtherDesc;
            _strCode = "MY8";
            _strDesc = _strCode + " DESC";
            _strOtherDesc = _strCode + " OTHER DESC";

            WaitForElementToBeClickable(By.CssSelector("button.primaryActionBtn"));
            _storeCountryPage.SearchStoreCountry(_strCode);
            WaitForTableToLoad();
            Assert.IsTrue(_driver.PageSource.Contains(_strCode));
        }

        [Test]
        public void TestAddNewStoreCountry()
        {
            string _strCode, _strDesc, _strOtherDesc;
            _strCode = "MY8";
            _strDesc = _strCode + " DESC";
            _strOtherDesc = _strCode + " OTHER DESC";

            // Step 1: Wait for and click the "New" button
            WaitForElementToBeClickable(By.CssSelector("button.primaryActionBtn"));
            _storeCountryPage.ClickNewButton();

            // Step 2: Enter values in the form fields
            _storeCountryPage.EnterCode(_strCode);
            _storeCountryPage.EnterDescription(_strDesc);
            _storeCountryPage.EnterOtherDescription(_strOtherDesc);

            // Step 3: Click the "Save" button
            _storeCountryPage.ClickSaveButton();

            // Step 4: Wait for the alert message to appear
            var alertElement = WaitForElementToBeVisible(By.CssSelector("div[role='alert']"));

            // Step 5: Extract and verify the alert text
            string alertText = alertElement.Text;
            alertText = alertText.ToUpper();
            Console.WriteLine("Alert Message: " + alertText);
            Assert.IsTrue(alertText.Contains("SUCCESS"), "The save operation was not successful.");

            // Optional: Wait for the URL to redirect (if needed)
            WaitForUrlToContain("/store-stepper/store-country");
        }




        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver.Quit();
            }
        }

        // Custom Wait Methods
        private void WaitForPageToLoad()
        {
            WaitForAngularToLoad();
            //_wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            //_wait.Until(driver =>((IJavaScriptExecutor)driver).ExecuteScript("return window.getAllAngularTestabilities().findIndex(x => !x.isStable()) === -1"));
        }

        private void WaitForAngularToLoad()
        {
            _wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript(
                "return window.angular !== undefined && window.angular.getTestability(document.body).isStable()"));
        }


        private void WaitForTableToLoad()
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".mat-mdc-table")));
        }

        private void WaitForElementToBeClickable(By locator)
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        private void WaitForUrlToContain(string partialUrl)
        {
            _wait.Until(ExpectedConditions.UrlContains(partialUrl));
        }
        private void WaitForElementToDisappear(By locator)
        {
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
        }

        private IWebElement WaitForElementToBeVisible(By locator)
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }


    }
}
