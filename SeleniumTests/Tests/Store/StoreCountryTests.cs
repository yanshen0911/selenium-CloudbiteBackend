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
        [AllureStory("Store Store Country Create")]
        [TestCase("MY8", "Malaysia", "Asia")]
        [TestCase("妈来西亚", "妈来西亚", "亚洲")]
        [TestCase("AAAAAAAAAA", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa12131asaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "sdffds12123asaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa908098a09a098a098a098a098a098a098as098dlkjasdlkjasldkjlkj")]
        [TestCase("#$#%^", "$^$W#^%", "$^$#^")]
        public void TestCreateNewStoreCountry(string Code, string Desc,string OtherDesc)
        {
            string _strCode, _strDesc, _strOtherDesc;

            //_strCode = "MY8";
            //_strDesc = _strCode + " DESC";
            //_strOtherDesc = _strCode + " OTHER DESC";

            _strCode = Code;
            _strDesc = Desc;
            _strOtherDesc = OtherDesc;

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
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Edit Store Country")]
        [TestCase("MY8", "New Description", "New Other Description")]
        [TestCase("妈来西亚", "Updated Description", "Updated Other Description")]
        public void TestEditStoreCountry(string Code, string NewDesc, string NewOtherDesc)
        {
            // Step 1: Search for the existing store country by its code
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _storeCountryPage.SearchStoreCountry(Code);
            helperFunction.WaitForTableToLoad(_wait);

            // Step 2: Click the Edit button
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector(".btn-edit-hover"));
            _storeCountryPage.ClickEditButton();

            // Step 3: Ensure the Code field is not editable
            var isCodeEditable = _storeCountryPage.CodeInput.GetAttribute("readonly") != null;
            Assert.IsTrue(isCodeEditable, "Code field should not be editable.");

            // Step 4: Update the Description and Other Description fields
            _storeCountryPage.EnterDescription(NewDesc);
            _storeCountryPage.EnterOtherDescription(NewOtherDesc);
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Edit Store Country", "2");

            // Step 5: Click the Save button
            _storeCountryPage.ClickSaveButton();

            // Step 6: Wait for the alert message to appear
            var alertElement = helperFunction.WaitForElementToBeVisible(_wait, By.CssSelector("div[role='alert']"));
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Edit Store Country", "2");

            // Step 7: Verify the success message in the alert
            string alertText = alertElement.Text.ToUpper();
            Console.WriteLine("Alert Message: " + alertText);
            Assert.IsTrue(alertText.Contains("SUCCESS"), "The edit operation was not successful.");

            // Step 8: Wait for the redirection back to the store country list
            helperFunction.WaitForUrlToContain(_wait, "/store-stepper/store-country");

            // Step 9: Re-Search the edited store country and validate changes
            _storeCountryPage.SearchStoreCountry(Code);
            helperFunction.WaitForTableToLoad(_wait);

            // Step 10: Assert that the updated Description and Other Description are displayed in the table or details view
            Assert.IsTrue(_driver.PageSource.Contains(NewDesc), "Updated Description was not saved successfully.");
            Assert.IsTrue(_driver.PageSource.Contains(NewOtherDesc), "Updated Other Description was not saved successfully.");
        }

        [Test]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("KeithChu")] 
        [AllureStory("Store Store Country Search")]
        [TestCase("MY8")]
        [TestCase("Malaysia")]
        [TestCase("MALAYSIA")]
        [TestCase("malaysia")]
        [TestCase("AAAAAAAAAA")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa12131asaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        [TestCase("sdffds12123asaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa908098a09a098a098a098a098a098a098as098dlkjasdlkjasldkjlkj")]
        public void TestSearchStoreCountry(string searchText)
        {
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _storeCountryPage.SearchStoreCountry(searchText);
            helperFunction.WaitForTableToLoad(_wait);
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Country", "7");
            Assert.IsTrue(_driver.PageSource.Contains(searchText));
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
