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
using System.Linq.Expressions;

namespace QASErpPlusAutomation.Tests.Store
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Store - Store Country - Valid")] // use this ties to module
    [AllureEpic("ERP-117")] // use this and ties to ticket number
    public class StoreCountryTests_Valid
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
            Thread.Sleep(1500);
            _loginHelper.SelectServer(AppConfig.ServerName);
            _loginHelper.PerformLogin(AppConfig.UserName, AppConfig.Password, false);

            helperFunction.WaitForPageToLoad(_wait);
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/store-stepper/store-country");
            helperFunction.WaitForPageToLoad(_wait);

            _storeCountryPage = new StoreCountryPage(_driver);
        }

        [Test]
        [Category("StoreCountry")]
        [Order(1)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Store Country Create")]
        [TestCase("QA TEST", "QA TEST DESC", "QA TEST OTHER DESC")]
        public void Create(string Code, string Desc,string OtherDesc)
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
        }

        [Test]
        [Category("StoreCountry")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.normal)]
        //[AllureOwner("Team ERP+")]
        [AllureStory("Store Store Country Search")]
        [TestCase("QA TEST")]
        [TestCase("QA TEST DESC")]
        [TestCase("QA TEST OTHER DESC")]
        public void Read_After_Create(string searchText)
        {
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _storeCountryPage.SearchStoreCountry(searchText);
            helperFunction.WaitForTableToLoad(_wait);
            
            // If not found, wait a bit and retry
            Thread.Sleep(1000);

            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Country", "7");

            var infoContainer = _driver.FindElement(By.CssSelector("div.table-container"));

            if (infoContainer.Text.Contains("No Data Available"))
            {
                Assert.Fail("No Data Available");
            }
            else
            {
                Assert.IsTrue(infoContainer.Text.Contains(searchText), $"Expected data for '{searchText}' not found in the results.");
            }
        }

        [Test]
        [Category("StoreCountry")]
        [Order(3)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Edit Store Country")]
        [TestCase("QA TEST", "QA TEST DESC UPDATED", "QA TEST OTHER DESC UPDATED")]
        public void Update(string Code, string NewDesc, string NewOtherDesc)
        {
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _storeCountryPage.SearchStoreCountry(Code);
            helperFunction.WaitForTableToLoad(_wait);

            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector(".btn-edit-hover"));
            _storeCountryPage.ClickEditButton(Code);

            _storeCountryPage.EnterDescription(NewDesc);
            _storeCountryPage.EnterOtherDescription(NewOtherDesc);
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Edit Store Country", "2");

            _storeCountryPage.ClickSaveButton();

            var alertElement = helperFunction.WaitForElementToBeVisible(_wait, By.CssSelector("div[role='alert']"));
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Edit Store Country", "2");

            string alertText = alertElement.Text.ToUpper();
            Console.WriteLine("Alert Message: " + alertText);
            Assert.IsTrue(alertText.Contains("SUCCESS"), "The edit operation was not successful.");
        }

        [Test]
        [Category("StoreCountry")]
        [Order(4)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Store Country Search")]
        [TestCase("QA TEST")]
        [TestCase("QA TEST DESC")]
        [TestCase("QA TEST OTHER DESC")]
        public void Read_After_Update(string searchText)
        {
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _storeCountryPage.SearchStoreCountry(searchText);
            helperFunction.WaitForTableToLoad(_wait);

            // If not found, wait a bit and retry
            Thread.Sleep(1000);

            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Country", "7");

            var infoContainer = _driver.FindElement(By.CssSelector("div.table-container"));

            if (infoContainer.Text.Contains("No Data Available"))
            {
                Assert.Fail("No Data Available");
            }
            else
            {
                Assert.IsTrue(infoContainer.Text.Contains(searchText), $"Expected data for '{searchText}' not found in the results.");
            }
        }

        [Test]
        [Category("StoreCountry")]
        [Order(5)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Delete Store Country")]
        [TestCase("QA TEST", "QA TEST DESC UPDATED", "QA TEST OTHER DESC UPDATED")]
        public void Delete(string code, string NewDesc, string NewOtherDesc)
        {
            // Step 1: Search for the record in the table
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _storeCountryPage.SearchStoreCountry(code);
            helperFunction.WaitForTableToLoad(_wait);

            // Step 2: Click the delete button for the matching code
            _storeCountryPage.DeleteStoreCountryByCode(code);

            // Step 3: Confirm the deletion in the dialog
            _storeCountryPage.ConfirmDelete(true);

            // Step 4: Wait for the success alert
            var alertElement = helperFunction.WaitForElementToBeVisible(_wait, By.CssSelector("div[role='alert']"));
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Delete Store Country");

            // Step 5: Verify the success message
            string alertText = alertElement.Text.ToUpper();
            Console.WriteLine("Alert Message: " + alertText);
            Assert.IsTrue(alertText.Contains("SUCCESS"), "The delete operation was not successful.");
        }

        [Test]
        [Category("StoreCountry")]
        [Order(6)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Store Country Search")]
        [TestCase("QA TEST")]
        [TestCase("QA TEST DESC")]
        [TestCase("QA TEST OTHER DESC")]
        public void Read_After_Delete(string searchText)
        {
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _storeCountryPage.SearchStoreCountry(searchText);
            helperFunction.WaitForTableToLoad(_wait);

            // If not found, wait a bit and retry
            Thread.Sleep(1000);

            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Country", "7");

            var infoContainer = _driver.FindElement(By.CssSelector("div.table-container"));

            Assert.IsTrue(infoContainer.Text.Contains("No Data Available"), $"Expected data for '{searchText}' not found in the results.");
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
