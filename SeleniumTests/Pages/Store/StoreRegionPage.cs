using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTests.Pages.Store
{
    public class StoreRegionPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Constructor
        public StoreRegionPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            PageFactory.InitElements(_driver, this);
        }

        // Elements
        [FindsBy(How = How.Id, Using = "action_search")]
        private IWebElement SearchInput { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".btn-search-hover")]
        private IWebElement SearchButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".btn-filter-hover")]
        private IWebElement FilterButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".btn-print-hover")]
        private IWebElement PrintButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".btn-upload-hover")]
        private IWebElement UploadButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".btn-download-hover")]
        private IWebElement DownloadButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".btn.primaryActionBtn")]
        private IWebElement NewButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".btn-edit-hover")]
        private IWebElement EditButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".btn-delete-hover")]
        private IWebElement DeleteButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".mat-mdc-paginator-navigation-next")]
        private IWebElement NextPageButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".mat-mdc-paginator-navigation-previous")]
        private IWebElement PreviousPageButton { get; set; }

        // New Elements
        [FindsBy(How = How.Id, Using = "code")]
        public IWebElement CodeInput { get; set; }

        [FindsBy(How = How.Id, Using = "desc")]
        private IWebElement DescriptionInput { get; set; }

        [FindsBy(How = How.Id, Using = "otherDesc")]
        private IWebElement OtherDescriptionInput { get; set; }

        [FindsBy(How = How.CssSelector, Using = "button.btn.primaryActionBtn.imgBtn")]
        private IWebElement SaveButton { get; set; }

        // Methods
        public void SearchStoreCountry(string searchTerm)
        {
            SearchInput.Clear();
            SearchInput.SendKeys(searchTerm);
            SearchButton.Click();
        }

        public void ClickNewButton()
        {
            var newButton = _wait.Until(ExpectedConditions.ElementToBeClickable(NewButton));
            newButton.Click();
        }

        public void ClickEditButton(string code)
        {
            // Locate the row by its code
            string xpath = $"//tr[normalize-space(td)='{code}']//button[contains(@class, 'btn-edit-hover')]";
            var deleteButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpath)));

            // Click the delete button
            deleteButton.Click();
        }

        public void DeleteStoreCountryByCode(string code)
        {
            // Locate the row by its code
            string xpath = $"//tr[normalize-space(td)='{code}']//button[contains(@class, 'btn-delete-hover')]";
            var deleteButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpath)));

            // Click the delete button
            deleteButton.Click();
        }


        public void ClickUploadButton()
        {
            UploadButton.Click();
        }

        public void ClickDownloadButton()
        {
            DownloadButton.Click();
        }

        public void ClickNextPage()
        {
            if (NextPageButton.Enabled)
            {
                NextPageButton.Click();
            }
        }

        public void ClickPreviousPage()
        {
            if (PreviousPageButton.Enabled)
            {
                PreviousPageButton.Click();
            }
        }

        public void PrintStoreCountryList()
        {
            PrintButton.Click();
        }

        // New Methods for Form Fields
        public void EnterCode(string code)
        {
            CodeInput.Clear();
            CodeInput.SendKeys(code);
        }

        public void EnterDescription(string description)
        {
            DescriptionInput.Clear();
            DescriptionInput.SendKeys(description);
        }

        public void EnterOtherDescription(string otherDescription)
        {
            OtherDescriptionInput.Clear();
            OtherDescriptionInput.SendKeys(otherDescription);
        }

        public void ClickSaveButton()
        {
            var saveButton = _wait.Until(ExpectedConditions.ElementToBeClickable(SaveButton));
            saveButton.Click();
        }

        public void ConfirmDelete(bool confirm)
        {
            // Wait for the confirmation dialog to appear
            var dialogContainer = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".dialog-container")));

            if (confirm)
            {
                // Click the "OK" button to confirm the deletion
                var okButton = dialogContainer.FindElement(By.CssSelector("button.btn.primaryActionBtn"));
                okButton.Click();
            }
            else
            {
                // Click the "Cancel" button to cancel the deletion
                var cancelButton = dialogContainer.FindElement(By.CssSelector("button.btn.secondaryActionBtn"));
                cancelButton.Click();
            }
        }

    }
}
