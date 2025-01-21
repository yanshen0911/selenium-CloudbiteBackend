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
    [AllureSuite("Store - Store Country - Invalid")] // use this ties to module
    [AllureEpic("ERP-117")] // use this and ties to ticket number
    public class StoreCountryTests_Invalid
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
        [AllureStory("Store Store Country Create Invalid")]
        [TestCase("MY8", "Malaysia", "Asia")]
        [TestCase("妈来西亚", "妈来西亚", "亚洲")]
        [TestCase("AAAAAAAAAA", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa12131asaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "sdffds12123asaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa908098a09a098a098a098a098a098a098as098dlkjasdlkjasldkjlkj")]
        [TestCase("#$#%^", "$^$W#^%", "$^$#^")]
        [TestCase("MYY", "MYYY", "MYYYY")]
        [TestCase("MY2", "MY22", "MY22")]
        [TestCase("MY3", "MY33", "MY333")]
        public void CreateNewStoreCountry(string Code, string Desc,string OtherDesc)
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
            SearchStoreCountry(_strCode);
        }

        [Test]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Edit Store Country")]
        [TestCase("MY8", "Malaysia NEW", "Asia NEW")]
        [TestCase("妈来西亚", "妈来西亚 NEW", "亚洲 NEW")]
        [TestCase("AAAAAAAAAA", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa12131asaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "sdffds12123asaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa908098a09a098a098a098a098a098a098as098dlkjasdlkjasldkjlkj")]
        [TestCase("#$#%^", "$^$W#^%", "$^$#^")]
        [TestCase("MYY", "MYYY NEW", "MYYYY NEW")]
        [TestCase("MY2", "MY22 NEW", "MY22 NEW")]
        [TestCase("MY3", "MY33 NEW", "MY333 NEW")]
        public void EditStoreCountry(string Code, string NewDesc, string NewOtherDesc)
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

            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _storeCountryPage.SearchStoreCountry(Code);
            helperFunction.WaitForTableToLoad(_wait);
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Country", "7");

            var infoContainer = _driver.FindElement(By.CssSelector("div.info-container"));

            string[] textsToCheck = { NewDesc, NewOtherDesc };

            if (infoContainer.Text.Contains("No Data Available"))
            {
                Console.WriteLine($"Search for '{Code}' resulted in no data.");
                Assert.IsFalse(infoContainer.Text.Contains(Code), $"Unexpectedly found data for '{Code}'.");
            }
            else
            {
                // Check if any of the specified texts (e.g., "NewDesc", "NewOtherDesc") are present
                if (textsToCheck.Any(text => infoContainer.Text.Contains(text)))
                {
                    Console.WriteLine($"Expected data found: {string.Join(", ", textsToCheck.Where(text => infoContainer.Text.Contains(text)))}");
                }
                else
                {
                    Assert.Fail($"Neither '{NewDesc}' nor '{NewOtherDesc}' were found in the results.");
                }
            }

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
        public void SearchStoreCountry(string searchText)
        {
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _storeCountryPage.SearchStoreCountry(searchText);
            helperFunction.WaitForTableToLoad(_wait);
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Country", "7");

            var infoContainer = _driver.FindElement(By.CssSelector("div.info-container"));

            if (infoContainer.Text.Contains("No Data Available"))
            {
                Console.WriteLine($"Search for '{searchText}' resulted in no data.");
                Assert.IsFalse(infoContainer.Text.Contains(searchText), $"Unexpectedly found data for '{searchText}'.");
            }
            else
            {
                Assert.IsTrue(infoContainer.Text.Contains(searchText), $"Expected data for '{searchText}' not found in the results.");
            }
        }

        [Test]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Delete Store Country")]
        [TestCase("MY8", "Malaysia", "Asia")]
        [TestCase("妈来西亚", "妈来西亚", "亚洲")]
        [TestCase("AAAAAAAAAA", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa12131asaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "sdffds12123asaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa908098a09a098a098a098a098a098a098as098dlkjasdlkjasldkjlkj")]
        [TestCase("#$#%^", "$^$W#^%", "$^$#^")]
        [TestCase("MYY", "MYYY", "MYYYY")]
        [TestCase("MY2", "MY22", "MY22")]
        [TestCase("MY3", "MY33", "MY333")]
        public void DeleteStoreCountry(string code, string NewDesc, string NewOtherDesc)
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

            // Step 6: Re-Search for the deleted record to ensure it is removed
            _storeCountryPage.SearchStoreCountry(code);
            helperFunction.WaitForTableToLoad(_wait);
            Assert.IsFalse(_driver.PageSource.Contains(code), "The store country was not deleted successfully.");
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Delete Store Country");


            // Step 2: Locate the 'info-container' section
            var infoContainer = _driver.FindElement(By.CssSelector("div.info-container"));

            // Step 3: Check for "No Data Available"
            if (infoContainer.Text.Contains("No Data Available"))
            {
                //if data no longer there, ok can success
                Console.WriteLine($"Search for '{code}' resulted in no data.");
                Assert.IsTrue(infoContainer.Text.Contains(code), $"Unexpectedly found data for '{code}'.");
            }
            else
            {
                //if data still there, then gg, delete failed
                Assert.IsFalse(infoContainer.Text.Contains(code), $"Expected data for '{code}' not found in the results.");
            }
        }

        //[Test]
        //[AllureSeverity(SeverityLevel.normal)]
        //[AllureStory("Store Country CRUD")]
        //[TestCase("MY8", "Malaysia", "Asia")]
        //[TestCase("妈来西亚", "妈来西亚", "亚洲")]
        //[TestCase("AAAAAAAAAA", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa12131asaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "sdffds12123asaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa908098a09a098a098a098a098a098a098as098dlkjasdlkjasldkjlkj")]
        //[TestCase("#$#%^", "$^$W#^%", "$^$#^")]
        //[TestCase("MYY", "MYYY", "MYYYY")]
        //[TestCase("MY2", "MY22", "MY22")]
        //[TestCase("MY3", "MY33", "MY333")]
        //public void StoreCountryCRUD_Full(string code, string NewDesc, string NewOtherDesc)
        //{
        //    TestCreateNewStoreCountry(code, NewDesc, NewOtherDesc);
        //    TestSearchStoreCountry(code);
        //    TestSearchStoreCountry(NewDesc);
        //    TestSearchStoreCountry(NewOtherDesc);
        //    TestEditStoreCountry(code, NewDesc + " " + DateTime.Now.ToString("HHmmssffff"), NewOtherDesc + " " + DateTime.Now.ToString("HHmmssffff"));
        //    TestEditStoreCountry(code, NewDesc + " " + DateTime.Now.ToString("HHmmssffff"), NewOtherDesc + " " + DateTime.Now.ToString("HHmmssffff"));
        //    TestDeleteStoreCountry(code, NewDesc, NewOtherDesc);
        //}

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
