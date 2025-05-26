using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using ERPPlus.SeleniumTests.Config;
using ERPPlus.SeleniumTests.Drivers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumTests.Helpers;
using SeleniumTests.Pages.Store;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using helperFunction = SeleniumTests.Helper.HelperFunction;

namespace QASErpPlusAutomation.Tests.Store
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Store - Store Group - Valid")] // use this ties to module
    [AllureEpic("ERP-117")] // use this and ties to ticket number
    public class StoreGroupTests_Valid
    {
        private IWebDriver _driver;
        private StoreGroupPage _StoreGroupPage;
        private WebDriverWait _wait;
        private LoginHelper _loginHelper;
        private SeleniumTests.Pages.Dashboard dashboardPage;


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
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/store-stepper/store-group-v2");
            helperFunction.WaitForPageToLoad(_wait);

            _StoreGroupPage = new StoreGroupPage(_driver);
            dashboardPage = new SeleniumTests.Pages.Dashboard(_driver);
        }

        [Test]
        [Category("StoreGroup")]
        [Order(1)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCase("TEST", "YS TEST DESC", "YS TEST OTHER DESC")]
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
            _StoreGroupPage.ClickNewButton();

            // Step 2: Enter values in the form fields
            _StoreGroupPage.EnterCode(_strCode);
            _StoreGroupPage.EnterDescription(_strDesc);
            _StoreGroupPage.EnterOtherDescription(_strOtherDesc);
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group", "1");
            // Step 3: Click the "Save" button
            _StoreGroupPage.ClickSaveButton();

            // Step 4: Wait for the alert message to appear
            var alertElement = helperFunction.WaitForElementToBeVisible(_wait, By.CssSelector("div[role='alert']"));
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group", "1");

            // Step 5: Extract and verify the alert text
            string alertText = alertElement.Text;
            alertText = alertText.ToUpper();
            Console.WriteLine("Alert Message: " + alertText);

            Assert.IsTrue(alertText.Contains("SUCCESS"), "The save operation was successful.");
        }


        [Test]
        [Category("StoreGroup")]
        [Order(2)]
        [AllureSeverity(SeverityLevel.normal)]
        //[AllureOwner("Team ERP+")]
        [AllureStory("Read_After_Create")]
        [TestCase("TEST")]
        [TestCase("YS TEST DESC")]
        [TestCase("YS TEST OTHER DESC")]
        public void Read_After_Create(string searchText)
        {
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.SearchStoreCountry(searchText);
            helperFunction.WaitForTableToLoad(_wait);
            
            // If not found, wait a bit and retry
            Thread.Sleep(1000);

            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group", "7");

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
        [Category("StoreGroup")]
        [Order(3)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Update")]
        [TestCase("TEST", "YS TEST DESC UPDATED", "YS TEST OTHER DESC UPDATED")]
        public void Update(string Code, string NewDesc, string NewOtherDesc)
        {
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.SearchStoreCountry(Code);
            helperFunction.WaitForTableToLoad(_wait);

            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector(".btn-edit-hover"));
            _StoreGroupPage.ClickEditButton(Code);

            _StoreGroupPage.EnterDescription(NewDesc);
            _StoreGroupPage.EnterOtherDescription(NewOtherDesc);
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Edit Store Country", "2");

            _StoreGroupPage.ClickSaveButton();

            var alertElement = helperFunction.WaitForElementToBeVisible(_wait, By.CssSelector("div[role='alert']"));
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Edit Store Country", "2");

            string alertText = alertElement.Text.ToUpper();
            Console.WriteLine("Alert Message: " + alertText);
            Assert.IsTrue(alertText.Contains("SUCCESS"), "The edit operation was not successful.");
        }

        [Test]
        [Category("StoreGroup")]
        [Order(4)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Read_After_Update")]
        [TestCase("TEST")]
        [TestCase("YS TEST DESC")]
        [TestCase("YS TEST OTHER DESC")]
        public void Read_After_Update(string searchText)
        {
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.SearchStoreCountry(searchText);
            helperFunction.WaitForTableToLoad(_wait);

            // If not found, wait a bit and retry
            Thread.Sleep(1000);

            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group", "7");

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
        [Category("StoreGroup")]
        [Order(5)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCase("T!@#$", "TEST DESC!@#$%^", "TEST OTHER DESC!@#$%^")]
        public void CreateSpecialCharacter(string Code, string Desc, string OtherDesc)
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
            _StoreGroupPage.ClickNewButton();

            // Step 2: Enter values in the form fields
            _StoreGroupPage.EnterCode(_strCode);
            _StoreGroupPage.EnterDescription(_strDesc);
            _StoreGroupPage.EnterOtherDescription(_strOtherDesc);
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group", "1");
            // Step 3: Click the "Save" button
            _StoreGroupPage.ClickSaveButton();

            // Step 4: Wait for the alert message to appear
            var alertElement = helperFunction.WaitForElementToBeVisible(_wait, By.CssSelector("div[role='alert']"));
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group", "1");

            // Step 5: Extract and verify the alert text
            string alertText = alertElement.Text;
            alertText = alertText.ToUpper();
            Console.WriteLine("Alert Message: " + alertText);

            Assert.IsTrue(alertText.Contains("SUCCESS"), "The save operation was successful.");
        }

        [Test]
        [Category("StoreGroup")]
        [Order(6)]
        [AllureSeverity(SeverityLevel.normal)]
        //[AllureOwner("Team ERP+")]
        [AllureStory("Read_After_Create")]
        [TestCase("T!@#$")]
        [TestCase("TEST DESC!@#$%^")]
        [TestCase("TEST OTHER DESC!@#$%^")]
        public void Read_After_CreateSpecialCharacter(string searchText)
        {
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.SearchStoreCountry(searchText);
            helperFunction.WaitForTableToLoad(_wait);

            // If not found, wait a bit and retry
            Thread.Sleep(1000);

            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group", "7");

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
        [Category("StoreGroup")]
        [Order(7)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Delete")]
        [TestCase("T!@#$", "TEST DESC!@#$%^", "TEST OTHER DESC!@#$%^")]
        public void DeleteSpecialCharacter(string code, string NewDesc, string NewOtherDesc)
        {
            // Step 1: Search for the record in the table
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.SearchStoreCountry(code);
            helperFunction.WaitForTableToLoad(_wait);

            // Step 2: Click the delete button for the matching code
            _StoreGroupPage.DeleteStoreCountryByCode(code);

            // Step 3: Confirm the deletion in the dialog
            _StoreGroupPage.ConfirmDelete(true);

            // Step 4: Wait for the success alert
            var alertElement = helperFunction.WaitForElementToBeVisible(_wait, By.CssSelector("div[role='alert']"));
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Delete Store Country");

            // Step 5: Verify the success message
            string alertText = alertElement.Text.ToUpper();
            Console.WriteLine("Alert Message: " + alertText);
            Assert.IsTrue(alertText.Contains("SUCCESS"), "The delete operation was not successful.");
        }



        [Test]
        [Category("StoreGroup")]
        [Order(8)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create")]
        [TestCase("TESTN1", "", "")]
        public void CreateNoDesc(string Code, string Desc, string OtherDesc)
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
            _StoreGroupPage.ClickNewButton();

            // Step 2: Enter values in the form fields
            _StoreGroupPage.EnterCode(_strCode);
            _StoreGroupPage.EnterDescription(_strDesc);
            _StoreGroupPage.EnterOtherDescription(_strOtherDesc);
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group", "1");
            // Step 3: Click the "Save" button
            _StoreGroupPage.ClickSaveButton();

            // Step 4: Wait for the alert message to appear
            var alertElement = helperFunction.WaitForElementToBeVisible(_wait, By.CssSelector("div[role='alert']"));
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group", "1");

            // Step 5: Extract and verify the alert text
            string alertText = alertElement.Text;
            alertText = alertText.ToUpper();
            Console.WriteLine("Alert Message: " + alertText);

            Assert.IsTrue(alertText.Contains("SUCCESS"), "The save operation was successful.");
        }



        [Test]
        [Category("StoreGroup")]
        [Order(9)]
        [AllureSeverity(SeverityLevel.normal)]
        //[AllureOwner("Team ERP+")]
        [AllureStory("Read_After_Create")]
        [TestCase("TESTN1")]
        [TestCase("")]
        [TestCase("")]
        public void Read_After_CreateNoDesc(string searchText)
        {
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.SearchStoreCountry(searchText);
            helperFunction.WaitForTableToLoad(_wait);

            // If not found, wait a bit and retry
            Thread.Sleep(1000);

            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group", "7");

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
        [Category("StoreGroup")]
        [Order(10)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Delete")]
        [TestCase("TESTN1", "", "")]
        public void DeleteNoDesc(string code, string NewDesc, string NewOtherDesc)
        {
            // Step 1: Search for the record in the table
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.SearchStoreCountry(code);
            helperFunction.WaitForTableToLoad(_wait);

            // Step 2: Click the delete button for the matching code
            _StoreGroupPage.DeleteStoreCountryByCode(code);

            // Step 3: Confirm the deletion in the dialog
            _StoreGroupPage.ConfirmDelete(true);

            // Step 4: Wait for the success alert
            var alertElement = helperFunction.WaitForElementToBeVisible(_wait, By.CssSelector("div[role='alert']"));
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Delete Store Country");

            // Step 5: Verify the success message
            string alertText = alertElement.Text.ToUpper();
            Console.WriteLine("Alert Message: " + alertText);
            Assert.IsTrue(alertText.Contains("SUCCESS"), "The delete operation was not successful.");
        }



        [Test]
        [Category("StoreGroup")]
        [Order(11)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Delete")]
        [TestCase("TEST", "YS TEST DESC UPDATED", "YS TEST OTHER DESC UPDATED")]
        public void Delete(string code, string NewDesc, string NewOtherDesc)
        {
            // Step 1: Search for the record in the table
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.SearchStoreCountry(code);
            helperFunction.WaitForTableToLoad(_wait);

            // Step 2: Click the delete button for the matching code
            _StoreGroupPage.DeleteStoreCountryByCode(code);

            // Step 3: Confirm the deletion in the dialog
            _StoreGroupPage.ConfirmDelete(true);

            // Step 4: Wait for the success alert
            var alertElement = helperFunction.WaitForElementToBeVisible(_wait, By.CssSelector("div[role='alert']"));
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Delete Store Country");

            // Step 5: Verify the success message
            string alertText = alertElement.Text.ToUpper();
            Console.WriteLine("Alert Message: " + alertText);
            Assert.IsTrue(alertText.Contains("SUCCESS"), "The delete operation was not successful.");
        }

        [Test]
        [Category("StoreGroup")]
        [Order(12)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Read_After_Delete")]
        [TestCase("TEST")]
        [TestCase("YS TEST DESC")]
        [TestCase("YS TEST OTHER DESC")]
        public void Read_After_Delete(string searchText)
        {
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.SearchStoreCountry(searchText);
            helperFunction.WaitForTableToLoad(_wait);

            // If not found, wait a bit and retry
            Thread.Sleep(1000);

            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group", "7");

            var infoContainer = _driver.FindElement(By.CssSelector("div.table-container"));

            Assert.IsTrue(infoContainer.Text.Contains("No Data Available"), $"Expected data for '{searchText}' not found in the results.");
        }



        [Test]
        [Category("StoreGroup")]
        [Order(13)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Create - Download Report")]
        public void DownloadReport()
        {
            // Step 1: Click the "Download" button
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.ClickDownloadButton();

            // Step 2: Wait for the success alert message
            var alertElement = helperFunction.WaitForElementToBeVisible(_wait, By.CssSelector("div[role='alert']"));
            helperFunction.TakeScreenshot(_driver, "Store", "Store", "Store Group", "DownloadSuccess");

            // Step 3: Validate the alert message
            string alertText = alertElement.Text.Trim().ToUpper();
            Console.WriteLine("Alert Message: " + alertText);

            Assert.IsTrue(alertText.Contains("SUCCESS"), "Expected success message not found after download.");
        }


        [Test]
        [Category("StoreGroup")]
        [Order(14)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Import - Upload CSV and Submit")]
        public void ImportCSVFile()
        {
            // Step 1: Click the "Upload" button
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.ClickUploadButton();

            // Step 2: Upload the CSV file (dropzone input should be present in HTML as a hidden <input type="file">)
            string filePath = Path.GetFullPath("C:\\Users\\ChooYanShen\\Desktop\\Store_Group_20250522_1747899060860.csv"); // replace with your actual file
            IWebElement fileInput = _driver.FindElement(By.CssSelector("input[type='file']"));
            fileInput.SendKeys(filePath);

            // Step 3: Optional wait for preview or uploaded state
            Thread.Sleep(2000); // Replace with explicit wait if there's a visible confirmation element

            // Step 4: Click the "Import" button
            IWebElement importBtn = _driver.FindElement(By.CssSelector("button.primaryActionBtn img[alt='save']"));
            importBtn.FindElement(By.XPath("//*[@id=\"mat-mdc-dialog-0\"]/div/div/app-upload-dialog/div/div[3]/button[2]")).Click(); // get parent button of the <img> and click it

            // Step 5: Wait for alert message or result
            var alertElement = helperFunction.WaitForElementToBeVisible(_wait, By.CssSelector("div[role='alert']"));
            string alertText = alertElement.Text.Trim().ToUpper();
            Console.WriteLine("Import Alert: " + alertText);

            // Step 6: Assert that import was successful
            Assert.IsTrue(alertText.Contains("SUCCESS") || alertText.Contains("IMPORT COMPLETED"), "CSV import failed.");
        }

        [Test]
        [Category("StoreGroup")]
        [Order(15)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Filter - Filter Store Group")]
        public void FilterFunction()
        {
            // === Declare test values ===
            string filterField = "Code";
            string matchType = "Exact Match";
            string filterValue = "SG";

            // === Step 1: Open filter panel ===
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.ClickFilterButton();

            // === Step 2: Wait for filter overlay ===
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("app-filter-overlay")));

            // === Step 3: Select dropdown filter criteria ===
            var firstDropdown = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='search_by_0_0']")));
            new SelectElement(firstDropdown).SelectByText(filterField);

            var secondDropdown = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='match_0_0']")));
            new SelectElement(secondDropdown).SelectByText(matchType);

            var inputField = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='input_field_0_0']")));
            inputField.Clear();
            inputField.SendKeys(filterValue);

            // === Step 4: Click Apply ===
            var applyFilterBtn = _wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath("//app-filter-overlay//button[contains(text(),'Search')]")));
            applyFilterBtn.Click();

            // === Step 5: Optional - Check if error alert appears ===
            try
            {
                WebDriverWait shortWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                var alert = shortWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div[role='alert']")));
                string alertText = alert.Text.Trim().ToLower();

                if (alertText.Contains("error") || alertText.Contains("failed"))
                {
                    Assert.Fail($"Filter failed: Alert message displayed - '{alert.Text}'");
                }
                else
                {
                    Console.WriteLine("Filter applied successfully or alert shown is not an error.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                // No alert means everything went fine — pass test silently
                Console.WriteLine("No alert appeared — assuming filter applied successfully.");
            }

        }


        [Test]
        [Category("StoreGroup")]
        [Order(16)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Filter - Filter and Reset Store Group")]
        public void FilterandResetFunction()
        {
            // === Declare test values ===
            string filterField = "Code";
            string matchType = "Exact Match";
            string filterValue = "SG";

            // === Step 1: Open filter panel ===
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.ClickFilterButton();

            // === Step 2: Wait for filter overlay ===
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("app-filter-overlay")));

            // === Step 3: Select dropdown filter criteria ===
            var firstDropdown = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='search_by_0_0']")));
            new SelectElement(firstDropdown).SelectByText(filterField);

            var secondDropdown = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='match_0_0']")));
            new SelectElement(secondDropdown).SelectByText(matchType);

            var resetBtn = _wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath("//app-filter-overlay//button[contains(text(),'Reset')]")));
            resetBtn.Click();

            Thread.Sleep(1000);

            firstDropdown = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='search_by_0_0']")));
            new SelectElement(firstDropdown).SelectByText(filterField);

            secondDropdown = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='match_0_0']")));
            new SelectElement(secondDropdown).SelectByText(matchType);

            var inputField = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='input_field_0_0']")));
            inputField.Clear();
            inputField.SendKeys(filterValue);

            // === Step 4: Click Apply ===
            var applyFilterBtn = _wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath("//app-filter-overlay//button[contains(text(),'Search')]")));
            applyFilterBtn.Click();

            // === Step 5: Optional - Check if error alert appears ===
            try
            {
                WebDriverWait shortWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                var alert = shortWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div[role='alert']")));
                string alertText = alert.Text.Trim().ToLower();

                if (alertText.Contains("error") || alertText.Contains("failed"))
                {
                    Assert.Fail($"Filter failed: Alert message displayed - '{alert.Text}'");
                }
                else
                {
                    Console.WriteLine("Filter applied successfully or alert shown is not an error.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                // No alert means everything went fine — pass test silently
                Console.WriteLine("No alert appeared — assuming filter applied successfully.");
            }

        }

        [Test]
        [Category("StoreGroup")]
        [Order(17)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Filter - Filter With Add New Store Group")]
        public void FilterAddNewGroup()
        {
            string filterLogic = "OR"; // or "AND"

            var testData = new[]
            {
        new { FilterField = "Code", MatchType = "Exact Match", FilterValue = "SG" },
        new { FilterField = "Description", MatchType = "Exact Match", FilterValue = "3.1415_STOPSIGN@" }
    };

            // Step 1: Open filter panel
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.ClickFilterButton();

            // Step 2: Wait for filter overlay
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("app-filter-overlay")));

            // Step 3: Click 'New Group' once to add a second group
            var addNewGroupBtn = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("//app-filter-overlay//button[contains(text(),'New Group')]")));
            addNewGroupBtn.Click();

            // Step 4: Wait for second input field to appear
            Thread.Sleep(1000); // temporary wait to let DOM settle, consider replacing with explicit wait if needed

            // Step 5: Fill both groups (0 and 1)
            for (int i = 0; i < testData.Length; i++)
            {
                var data = testData[i];
                string fieldId = $"search_by_{i}_0";
                string matchId = $"match_{i}_0";
                string inputId = $"input_field_{i}_0";

                try
                {
                    var fieldDropdown = new SelectElement(_wait.Until(d => d.FindElement(By.Id(fieldId))));
                    var matchDropdown = new SelectElement(_wait.Until(d => d.FindElement(By.Id(matchId))));
                    var inputField = _wait.Until(d => d.FindElement(By.Id(inputId)));

                    fieldDropdown.SelectByText(data.FilterField);
                    matchDropdown.SelectByText(data.MatchType);
                    inputField.Clear();
                    inputField.SendKeys(data.FilterValue);

                    Console.WriteLine($"Filled group {i}: {data.FilterField} - {data.MatchType} - {data.FilterValue}");
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine($"Element not found for group {i}: {fieldId}, {matchId}, {inputId}");
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine($"Timeout waiting for group {i} elements.");
                }
            }

            var logicButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath($"//app-filter-overlay//button[contains(text(),'{filterLogic}')]")));
            logicButton.Click();


            // Step 6: Click Apply
            var applyBtn = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("//app-filter-overlay//button[contains(text(),'Search')]")));
            applyBtn.Click();

            // Step 7: Optional - Check for error alerts
            try
            {
                var shortWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                var alert = shortWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div[role='alert']")));
                string alertText = alert.Text.Trim().ToLower();

                if (alertText.Contains("error") || alertText.Contains("failed"))
                {
                    Assert.Fail($"Filter failed: Alert message displayed - '{alert.Text}'");
                }
                else
                {
                    Console.WriteLine("Filter applied successfully or alert shown is not an error.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("No alert appeared — assuming filter applied successfully.");
            }
        }

        [Test]
        [Category("StoreGroup")]
        [Order(18)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Filter - Filter With Add New Filter Group")]
        public void FilterAddFilter()
        {
            string filterLogic = "OR"; // or "AND"

            var testData = new[]
            {
            new { FilterField = "Code", MatchType = "Exact Match", FilterValue = "SG" },
            new { FilterField = "Description", MatchType = "Exact Match", FilterValue = "3.1415_STOPSIGN@" }
            };

            // Step 1: Open filter panel
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.ClickFilterButton();

            // Step 2: Wait for filter overlay
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("app-filter-overlay")));

            // Step 3: Click 'New Group' once to add a second group
            var addNewFilterBtn = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("//app-filter-overlay//button[contains(text(),'Filter')]")));
            addNewFilterBtn.Click();

            // Step 4: Wait for second input field to appear
            Thread.Sleep(1000); // temporary wait to let DOM settle, consider replacing with explicit wait if needed

            // Step 5: Fill both groups (0 and 1)
            for (int i = 0; i < testData.Length; i++)
            {
                var data = testData[i];
                string fieldId = $"search_by_0_{i}";
                string matchId = $"match_0_{i}";
                string inputId = $"input_field_0_{i}";

                try
                {
                    var fieldDropdown = new SelectElement(_wait.Until(d => d.FindElement(By.Id(fieldId))));
                    var matchDropdown = new SelectElement(_wait.Until(d => d.FindElement(By.Id(matchId))));
                    var inputField = _wait.Until(d => d.FindElement(By.Id(inputId)));

                    fieldDropdown.SelectByText(data.FilterField);
                    matchDropdown.SelectByText(data.MatchType);
                    inputField.Clear();
                    inputField.SendKeys(data.FilterValue);

                    Console.WriteLine($"Filled group {i}: {data.FilterField} - {data.MatchType} - {data.FilterValue}");
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine($"Element not found for group {i}: {fieldId}, {matchId}, {inputId}");
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine($"Timeout waiting for group {i} elements.");
                }
            }

            var logicButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath($"//app-filter-overlay//button[contains(text(),'{filterLogic}')]")));
            logicButton.Click();


            // Step 6: Click Apply
            var applyBtn = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("//app-filter-overlay//button[contains(text(),'Search')]")));
            applyBtn.Click();

            // Step 7: Optional - Check for error alerts
            try
            {
                var shortWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                var alert = shortWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div[role='alert']")));
                string alertText = alert.Text.Trim().ToLower();

                if (alertText.Contains("error") || alertText.Contains("failed"))
                {
                    Assert.Fail($"Filter failed: Alert message displayed - '{alert.Text}'");
                }
                else
                {
                    Console.WriteLine("Filter applied successfully or alert shown is not an error.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("No alert appeared — assuming filter applied successfully.");
            }
        }


        [Test]
        [Category("StoreGroup")]
        [Order(19)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Filter - Filter With Add New Store Group")]
        public void FilterWithDeleteNewGroup()
        {
            string filterLogic = "OR"; // or "AND"

            var testData = new[]
            {
        new { FilterField = "Code", MatchType = "Exact Match", FilterValue = "SG" },
        new { FilterField = "Description", MatchType = "Exact Match", FilterValue = "3.1415_STOPSIGN@" }
    };

            // Step 1: Open filter panel
            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.ClickFilterButton();

            // Step 2: Wait for filter overlay
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("app-filter-overlay")));

            // Step 3: Click 'New Group' once to add a second group
            var addNewGroupBtn = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("//app-filter-overlay//button[contains(text(),'New Group')]")));
            addNewGroupBtn.Click();

            // Step 3: Click 'New Group' once to add a second group
            var removeNewGroupBtn = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("//app-filter-overlay//button[contains(text(),'Delete Group')]")));
            removeNewGroupBtn.Click();

            // Step 3: Click 'New Group' once to add a second group
            addNewGroupBtn = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("//app-filter-overlay//button[contains(text(),'New Group')]")));
            addNewGroupBtn.Click();

            // Step 4: Wait for second input field to appear
            Thread.Sleep(1000); // temporary wait to let DOM settle, consider replacing with explicit wait if needed

            // Step 5: Fill both groups (0 and 1)
            for (int i = 0; i < testData.Length; i++)
            {
                var data = testData[i];
                string fieldId = $"search_by_{i}_0";
                string matchId = $"match_{i}_0";
                string inputId = $"input_field_{i}_0";

                try
                {
                    var fieldDropdown = new SelectElement(_wait.Until(d => d.FindElement(By.Id(fieldId))));
                    var matchDropdown = new SelectElement(_wait.Until(d => d.FindElement(By.Id(matchId))));
                    var inputField = _wait.Until(d => d.FindElement(By.Id(inputId)));

                    fieldDropdown.SelectByText(data.FilterField);
                    matchDropdown.SelectByText(data.MatchType);
                    inputField.Clear();
                    inputField.SendKeys(data.FilterValue);

                    Console.WriteLine($"Filled group {i}: {data.FilterField} - {data.MatchType} - {data.FilterValue}");
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine($"Element not found for group {i}: {fieldId}, {matchId}, {inputId}");
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine($"Timeout waiting for group {i} elements.");
                }
            }

            var logicButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath($"//app-filter-overlay//button[contains(text(),'{filterLogic}')]")));
            logicButton.Click();


            // Step 6: Click Apply
            var applyBtn = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("//app-filter-overlay//button[contains(text(),'Search')]")));
            applyBtn.Click();

            // Step 7: Optional - Check for error alerts
            try
            {
                var shortWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                var alert = shortWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div[role='alert']")));
                string alertText = alert.Text.Trim().ToLower();

                if (alertText.Contains("error") || alertText.Contains("failed"))
                {
                    Assert.Fail($"Filter failed: Alert message displayed - '{alert.Text}'");
                }
                else
                {
                    Console.WriteLine("Filter applied successfully or alert shown is not an error.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("No alert appeared — assuming filter applied successfully.");
            }
        }


        [Test]
        [Category("StoreGroup")]
        [Order(20)]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Filter - Add New Group and Add New Filter")]
        public void FilterAddGroupThenAddFilter()
        {
            // Logic BETWEEN groups (top level)
            string groupLogicTop = "OR";

            // Logic BETWEEN filters inside each group
            string[] filterLogicPerGroup = { "OR", "OR" };

            var filtersByGroup = new Dictionary<int, List<(string Field, string Match, string Value)>>()
            {
                [0] = new List<(string, string, string)>
        {
            ("Code", "Exact Match", "SG"),
            ("Description", "Exact Match", "3.1415_STOPSIGN@")
        },
                [1] = new List<(string, string, string)>
        {
            ("Other Description", "Exact Match", "D01"),
            ("Description", "Exact Match", "3.1415_STOPSIGN@")
        }
            };

            helperFunction.WaitForElementToBeClickable(_wait, By.CssSelector("button.primaryActionBtn"));
            _StoreGroupPage.ClickFilterButton();

            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("app-filter-overlay")));

            foreach (var group in filtersByGroup)
            {
                int groupIndex = group.Key;
                var filters = group.Value;

                if (groupIndex > 0)
                {
                    var addGroupBtn = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                        By.XPath("//app-filter-overlay//button[contains(text(),'New Group')]")));
                    addGroupBtn.Click();
                    Thread.Sleep(1000);

                    // Debug: print current groups count
                    var groups = _driver.FindElements(By.XPath("//app-filter-overlay//form/div/div"));
                    Console.WriteLine($"Groups count after adding group {groupIndex}: {groups.Count}");
                }

                for (int filterIndex = 0; filterIndex < filters.Count; filterIndex++)
                {
                    if (filterIndex > 0)
                    {
                        // Use group-specific XPath for Add Filter button inside current group
                        var addFilterBtn = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                            By.XPath($"//app-filter-overlay//form/div/div[{groupIndex + 1}]//button[contains(text(),'Filter')]")));
                        addFilterBtn.Click();
                        Thread.Sleep(1000);

                        // Debug: print filters count inside group after adding filter
                        var filtersInGroup = _driver.FindElements(By.XPath($"//app-filter-overlay//form/div/div[{groupIndex + 1}]//div[contains(@class,'filter-row-class')]"));
                        Console.WriteLine($"Group {groupIndex} filters count after adding filter {filterIndex}: {filtersInGroup.Count}");
                    }

                    var filterData = filters[filterIndex];
                    string idSuffix = $"{groupIndex}_{filterIndex}";

                    try
                    {
                        var fieldDropdown = new SelectElement(_wait.Until(d => d.FindElement(By.Id($"search_by_{idSuffix}"))));
                        var matchDropdown = new SelectElement(_wait.Until(d => d.FindElement(By.Id($"match_{idSuffix}"))));
                        var inputField = _wait.Until(d => d.FindElement(By.Id($"input_field_{idSuffix}")));

                        fieldDropdown.SelectByText(filterData.Field);
                        matchDropdown.SelectByText(filterData.Match);
                        inputField.Clear();
                        inputField.SendKeys(filterData.Value);

                        Console.WriteLine($"Filled Group {groupIndex} - Filter {filterIndex}: {filterData.Field} - {filterData.Match} - {filterData.Value}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error filling Group {groupIndex} Filter {filterIndex}: {e.Message}");
                        Assert.Fail($"Failed at Group {groupIndex}, Filter {filterIndex}");
                    }
                }
            }

            // Click top-level GROUP logic button (e.g., AND, OR)
            try
            {
                var topLogicButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//app-filter-overlay//button[contains(text(),'{groupLogicTop}')]")));
                topLogicButton.Click();
                Console.WriteLine($"Top-level group logic button '{groupLogicTop}' clicked.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to click top-level group logic button: {e.Message}");
            }

            // Click logic buttons FOR EACH GROUP for FILTER logic (between filters inside the group)
            for (int i = 0; i < filtersByGroup.Count; i++)
            {
                string logic = filterLogicPerGroup.Length > i ? filterLogicPerGroup[i].ToUpper() : "OR"; // Default OR if missing
                int buttonIndex = logic == "AND" ? 1 : 2; // button[1] for AND, button[2] for OR

                string xpath = $"//app-filter-overlay//form/div/div[{i + 1}]/div/div/div[1]/button[{buttonIndex}]";

                try
                {
                    var button = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(xpath)));
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", button);
                    Thread.Sleep(200); // wait a bit

                    button.Click();

                    Console.WriteLine($"Clicked '{logic}' logic button for group {i} at XPath: {xpath}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to click logic button for group {i}: {e.Message}");
                    Assert.Fail($"Logic button click failed for group {i}.");
                }
            }

            // Step 5: Click Apply/Search button
            var applyBtn = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                By.XPath("//app-filter-overlay//button[contains(text(),'Search')]")));
            applyBtn.Click();

            // Step 6: Check for alert message (error or success)
            try
            {
                var shortWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                var alert = shortWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div[role='alert']")));
                string alertText = alert.Text.Trim().ToLower();

                if (alertText.Contains("error") || alertText.Contains("failed"))
                {
                    Assert.Fail($"Filter failed: Alert message - '{alert.Text}'");
                }
                else
                {
                    Console.WriteLine("Filter applied successfully or alert shown is not an error.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("No alert appeared — assuming filter applied successfully.");
            }
        }




        [TearDown]
        public void TearDown()
        {
            // Perform logout
            dashboardPage.Logout();
            
            // Validate logout by checking the login page URL
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("/login"));

            if (_driver != null)
            {
                _driver.Quit();
            }
        }
    }
}
