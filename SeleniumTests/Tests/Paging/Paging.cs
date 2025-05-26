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
    [AllureSuite("Store - Store Group - Paging")] // use this ties to module
    [AllureEpic("ERP-117")] // use this and ties to ticket number

    public class Paging
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
        [AllureStory("Store Group Paging - Click Next and Verify Change")]
        public void TestPagingNextButtonAndVerify()
        {
            // Step 0: Wait for table to be visible
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[1]")));

            // Step 1: Capture current table HTML content
            string beforeHtml = tableElement.GetAttribute("innerHTML");

            // Step 2: Click the "Next" button
            var nextButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[2]/app-custom-paginator/div/div[3]/button[3]")));
            nextButton.Click();
            Console.WriteLine("✅ 'Next' button clicked.");

            Thread.Sleep(3000);

            // Step 3: Wait for table content to change
            _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[1]"));
                return updatedTable.GetAttribute("innerHTML") != beforeHtml;
            });

            // Step 4: Take screenshot
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group Paging", "AfterNextClick");

            Console.WriteLine("✅ Table content changed after clicking 'Next'.");
        }


        [Test]
        [Category("StoreGroup")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Group Paging - Click Previous and Verify Change")]
        public void TestPagingPreviousButtonAndVerify()
        {
            // Step 0: Wait for table to be visible
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[1]")));

            // Step 1: Capture current table HTML content
            string beforeHtml = tableElement.GetAttribute("innerHTML");

            // Step 2: Click the "Next" button
            var nextButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[2]/app-custom-paginator/div/div[3]/button[3]")));
            nextButton.Click();
            Console.WriteLine("✅ 'Next' button clicked.");

            Thread.Sleep(3000);

            // Step 2: Click the "Next" button
            var previousButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[2]/app-custom-paginator/div/div[3]/button[2]")));
            previousButton.Click();
            Console.WriteLine("✅ 'Previous' button clicked.");

            Thread.Sleep(3000);

            // Step 3: Wait for table content to change
            _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[1]"));
                return updatedTable.GetAttribute("innerHTML") != beforeHtml;
            });

            // Step 4: Take screenshot
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group Paging", "AfterNextClick");

            Console.WriteLine("✅ Table content changed after clicking 'Next'.");
        }


        [Test]
        [Category("StoreGroup")]
        [Order(3)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Group Paging - Click Last and Verify Change")]
        public void TestPagingLastButtonAndVerify()
        {
            // Step 0: Wait for table to be visible
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[1]")));

            // Step 1: Capture current table HTML content
            string beforeHtml = tableElement.GetAttribute("innerHTML");

            // Step 2: Click the "Last" button
            var lastButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[2]/app-custom-paginator/div/div[3]/button[4]")));
            lastButton.Click();
            Console.WriteLine("✅ 'Last' button clicked.");

            Thread.Sleep(3000);

            // Step 3: Wait for table content to change
            _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[1]"));
                return updatedTable.GetAttribute("innerHTML") != beforeHtml;
            });

            // Step 4: Take screenshot
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group Paging", "AfterNextClick");

            Console.WriteLine("✅ Table content changed after clicking 'Last'.");
        }

        [Test]
        [Category("StoreGroup")]
        [Order(4)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Group Paging - Click Next and Verify Change")]
        public void TestPagingFirstButtonAndVerify()
        {
            // Step 0: Wait for table to be visible
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[1]")));

            // Step 1: Capture current table HTML content
            string beforeHtml = tableElement.GetAttribute("innerHTML");

            // Step 2: Click the "Last" button
            var lastButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[2]/app-custom-paginator/div/div[3]/button[4]")));
            lastButton.Click();
            Console.WriteLine("✅ 'Last' button clicked.");

            Thread.Sleep(3000);

            var firstButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[2]/app-custom-paginator/div/div[3]/button[1]")));
            firstButton.Click();
            Console.WriteLine("✅ 'First' button clicked.");

            Thread.Sleep(3000);

            // Step 3: Wait for table content to change
            _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[1]"));
                return updatedTable.GetAttribute("innerHTML") != beforeHtml;
            });

            // Step 4: Take screenshot
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group Paging", "AfterNextClick");

            Console.WriteLine("✅ Table content changed after clicking 'First'.");
        }


        [Test]
        [Category("StoreGroup")]
        [Order(5)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Group Paging - Click Page Size Dropdown and Verify Table Update")]
        [TestCase("100")]
        public void TestItemsPerPageVerify(string pageSizeValue)
        {
            // Step 0: Wait for the table to be visible and capture its original HTML
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("//app-store-region-v2/div[2]/div[1]")));
            string beforeHtml = tableElement.GetAttribute("innerHTML");

            // Step 1: Locate the <select> dropdown
            var dropdownElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/div/app-store-region-v2/div[2]/div[2]/app-custom-paginator/div/div[2]/select")));

            // Step 2: Use Selenium's SelectElement to change the value
            var select = new SelectElement(dropdownElement);
            select.SelectByText(pageSizeValue); // or select.SelectByValue(pageSizeValue);
            Console.WriteLine($"✅ Selected page size: {pageSizeValue}");


            // Step 3: Wait for the table to update (HTML should change)
            _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath("//app-store-region-v2/div[2]/div[1]"));
                return updatedTable.GetAttribute("innerHTML") != beforeHtml;
            });

            Thread.Sleep(3000);

            // Step 4: Take screenshot
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group Paging", $"AfterPageSize_{pageSizeValue}");
            Console.WriteLine("✅ Table updated after selecting page size.");
        }


        [Test]
        [Category("StoreGroup")]
        [Order(6)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Store Group Paging - Input Page Number and Verify Table Update")]
        [TestCase("2")] // Assuming you're testing page number 2
        public void TestInputPageNumberVerify(string pageNumber)
        {

            // Step 0: Wait for the table to be visible and capture its original HTML
            var tableElement = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("//app-store-region-v2/div[2]/div[1]")));
            string beforeHtml = tableElement.GetAttribute("innerHTML");

            // Step 1: Find the input field and enter a new page number
            var inputField = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("//*[@id='pageInput']")));
            inputField.Clear();
            inputField.SendKeys(pageNumber);
            inputField.SendKeys(Keys.Enter); // Simulate pressing Enter to trigger paging
            Console.WriteLine($"✅ Entered page number: {pageNumber}");

            // Step 2: Wait for the table to update
            _wait.Until(driver =>
            {
                var updatedTable = driver.FindElement(By.XPath("//app-store-region-v2/div[2]/div[1]"));
                return updatedTable.GetAttribute("innerHTML") != beforeHtml;
            });

            Thread.Sleep(3000);

            // Step 3: Screenshot after update
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group Paging", $"AfterPageInput_{pageNumber}");
            Console.WriteLine("✅ Table updated after entering page number.");
        }



        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _driver.Quit();
        }
    }
}
