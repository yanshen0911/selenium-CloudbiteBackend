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
    [AllureSuite("Store - Store Group - Invalid")] // use this ties to module
    [AllureEpic("ERP-117")] // use this and ties to ticket number

    public class StoreGroupTests_Invalid
    {
        private IWebDriver _driver;
        private StoreGroupPage _StoreGroupPage;
        private WebDriverWait _wait;
        private LoginHelper _loginHelper;
        private SeleniumTests.Pages.Dashboard dashboardPage;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _driver = DriverFactory.CreateDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login");

            _loginHelper = new LoginHelper(_driver, _wait);
            _loginHelper.SelectServer(AppConfig.ServerName);
            _loginHelper.PerformLogin(AppConfig.UserName, AppConfig.Password, false);

            helperFunction.WaitForPageToLoad(_wait);
        }

        [SetUp]
        public void SetUp()
        {
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/store-stepper/store-group-v2");
            helperFunction.WaitForPageToLoad(_wait);
            _StoreGroupPage = new StoreGroupPage(_driver);
        }

        [Test]
        [Category("StoreGroup")]
        [Order(1)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Store Group Create")]
        [TestCase("TESTD", "QA TEST DESC", "QA TEST OTHER DESC")]
        public void CreateDuplicate(string Code, string Desc, string OtherDesc)
        {
            string _strCode = Code;
            string _strDesc = Desc;
            string _strOtherDesc = OtherDesc;

            for (int i = 1; i <= 2; i++)
            {
                Console.WriteLine($"🔄 Attempt #{i}: {(i == 1 ? "Insert New" : "Duplicate Check")}");

                // Step 1: Click the "New" button
                helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
                _StoreGroupPage.ClickNewButton();

                // Step 2: Enter form values
                _StoreGroupPage.EnterCode(_strCode);
                _StoreGroupPage.EnterDescription(_strDesc);
                _StoreGroupPage.EnterOtherDescription(_strOtherDesc);
                helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Country", $"Attempt{i}");

                // Step 3: Click Save
                _StoreGroupPage.ClickSaveButton();

                // Step 4: Wait for alert
                var alertElement = helperFunction.WaitForElementToBeVisible(_wait, By.CssSelector("div[role='alert']"));
                helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Country", $"Alert{i}");

                // Step 5: Extract alert message
                string alertText = alertElement.Text.ToUpper();
                Console.WriteLine($"🧾 Alert Message (Attempt {i}): {alertText}");

                if (i == 1)
                {
                    // First attempt: Expect successful save
                    Assert.IsTrue(alertText.Contains("SUCCESS") || alertText.Contains("CREATED"),
                        "❌ First insert failed unexpectedly.");
                }
                else
                {
                    // Second attempt: Expect duplicate error
                    Assert.IsTrue(alertText.Contains("ALREADY EXIST") || alertText.Contains("DUPLICATE"),
                        "❌ Duplicate error message not shown on second attempt.");
                }
            }
        }


        [Test]
        [Category("StoreGroup")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Store Group Create - Mandatory Field Validation")]
        [TestCase("", "QA TEST DESC", "QA TEST OTHER DESC")]
        public void CreateMandatory(string Code, string Desc, string OtherDesc)
        {
            string _strCode = Code;
            string _strDesc = Desc;
            string _strOtherDesc = OtherDesc;

            // Step 1: Click "New"
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.ClickNewButton();

            // Step 2: Fill in form fields
            _StoreGroupPage.EnterCode(_strCode);
            _StoreGroupPage.EnterDescription(_strDesc);
            _StoreGroupPage.EnterOtherDescription(_strOtherDesc);
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group", "BeforeValidation");

            // Step 3: Locate Save button
            var saveButton = _driver.FindElement(By.CssSelector("button.primaryActionBtn"));

            // Step 4: Check if 'disabled' attribute exists
            bool isDisabled = saveButton.GetAttribute("disabled") != null;

            Console.WriteLine("Save button disabled attribute present: " + isDisabled);
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group", "SaveButtonState");

            // Step 5: Assert Save button is disabled when Code is empty
            Assert.IsTrue(isDisabled, "Save button should be disabled when 'Code' field is empty, but it was enabled.");
        }


        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _driver.Quit();
        }
    }
}
