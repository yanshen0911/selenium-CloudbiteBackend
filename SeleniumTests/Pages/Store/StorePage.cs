using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTests.Pages.Store
{
    public class StorePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Constructor
        public StorePage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
            PageFactory.InitElements(driver, this);
        }

        // Locators
        private By SearchInput => By.CssSelector("#action_search");
        private By SearchButton => By.CssSelector("button.btn-search-hover");
        private By NewButton => By.CssSelector("button.primaryActionBtn");
        private By EditButtons => By.CssSelector("button.btn-edit-hover");
        private By DeleteButtons => By.CssSelector("button.btn-delete-hover");
        private By AlertMessage => By.CssSelector("div[role='alert']");
        private By TableRows => By.CssSelector("table.mat-mdc-table tbody tr");
        private By PaginatorNextButton => By.CssSelector(".mat-mdc-paginator-navigation-next");
        private By PaginatorPreviousButton => By.CssSelector(".mat-mdc-paginator-navigation-previous");

        // Methods

        // Search for a store
        public void SearchStore(string searchText)
        {
            var searchInput = _wait.Until(ExpectedConditions.ElementIsVisible(SearchInput));
            searchInput.Clear();
            searchInput.SendKeys(searchText);

            var searchButton = _wait.Until(ExpectedConditions.ElementToBeClickable(SearchButton));
            searchButton.Click();
        }

        // Click the "New" button to create a store
        public void ClickNewButton()
        {
            var newButton = _wait.Until(ExpectedConditions.ElementToBeClickable(NewButton));
            newButton.Click();
        }

        // Click the "Edit" button for a specific store based on its code
        public void ClickEditButton(string code)
        {
            var rows = _wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(TableRows));
            foreach (var row in rows)
            {
                if (row.Text.Contains(code))
                {
                    var editButton = row.FindElement(EditButtons);
                    editButton.Click();
                    return;
                }
            }
            throw new NoSuchElementException($"Edit button for code '{code}' not found.");
        }

        // Click the "Delete" button for a specific store based on its code
        public void ClickDeleteButton(string code)
        {
            var rows = _wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(TableRows));
            foreach (var row in rows)
            {
                if (row.Text.Contains(code))
                {
                    var deleteButton = row.FindElement(DeleteButtons);
                    deleteButton.Click();
                    return;
                }
            }
            throw new NoSuchElementException($"Delete button for code '{code}' not found.");
        }

        // Confirm a delete operation
        public void ConfirmDelete(bool confirm)
        {
            var alertMessage = _wait.Until(ExpectedConditions.ElementIsVisible(AlertMessage));
            if (confirm)
            {
                alertMessage.FindElement(By.CssSelector("button.confirm-button")).Click();
            }
            else
            {
                alertMessage.FindElement(By.CssSelector("button.cancel-button")).Click();
            }
        }

        // Get all rows of the table
        public List<IWebElement> GetAllRows()
        {
            return _wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(TableRows)).ToList();
        }

        // Pagination - Go to the next page
        public void GoToNextPage()
        {
            var nextButton = _wait.Until(ExpectedConditions.ElementToBeClickable(PaginatorNextButton));
            nextButton.Click();
        }

        // Pagination - Go to the previous page
        public void GoToPreviousPage()
        {
            var previousButton = _wait.Until(ExpectedConditions.ElementToBeClickable(PaginatorPreviousButton));
            previousButton.Click();
        }

        // Wait for the alert message and return its text
        public string GetAlertMessage()
        {
            var alertElement = _wait.Until(ExpectedConditions.ElementIsVisible(AlertMessage));
            return alertElement.Text.Trim();
        }
    }
}

