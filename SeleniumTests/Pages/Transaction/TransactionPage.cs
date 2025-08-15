using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTests.Pages.Transaction


{
    public class TransactionPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Constructor
        public TransactionPage(IWebDriver driver)
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

        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-store > div > div.d-flex.gap-2.justify-content-start > div > ul > li.nav-item.ps-9.pt-3 > a")]
        private IWebElement FilterALLCategoryButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-store > div > div.d-flex.gap-2.justify-content-start > div > ul > li:nth-child(2) > a")]
        private IWebElement FilterActiveCategoryButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-store > div > div.d-flex.gap-2.justify-content-start > div > ul > li:nth-child(3) > a")]
        private IWebElement FilterInactiveCategoryButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-store > div > div.d-flex.gap-2.justify-content-start > div > ul > li.ms-auto.align-items-center.pt-3.mobileHideFilter > div > div:nth-child(2) > span")]
        private IWebElement FilterPendingCategoryButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-store > div > div.d-flex.gap-2.justify-content-start > div > ul > li.ms-auto.align-items-center.pt-3.mobileHideFilter > div > div:nth-child(3) > span")]
        private IWebElement FilterSuccessCategoryButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-store > div > div.d-flex.gap-2.justify-content-start > div > ul > li.ms-auto.align-items-center.pt-3.mobileHideFilter > div > div:nth-child(4) > span")]
        private IWebElement FilterFailedCategoryButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".btn-print-hover")]
        private IWebElement PrintButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-store > div > div.card-header.border-0.pt-5 > div > div:nth-child(1) > a")]
        private IWebElement ImportButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_body > ngb-modal-window > div > div > app-upload-modal > div > div.modal-footer.justify-content-end.d-flex.ng-star-inserted > button.btn.btn-primary.mx-2")]
        private IWebElement UploadButton { get; set; }


        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-store > div > div.card-header.border-0.pt-5 > div > div:nth-child(2) > a")]
        private IWebElement ExportButton { get; set; }


        [FindsBy(How = How.XPath, Using = "//*[@id=\"kt_content_container\"]/app-store/div/div[3]/div/div[3]/a")]
        private IWebElement NewButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"kt_body\"]/ngb-modal-window/div/div/app-store-modal/div/div[3]/div/div[2]/button")]
        private IWebElement ContinueButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".btn-edit-hover")]
        private IWebElement EditButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".btn-delete-hover")]
        private IWebElement DeleteButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".mat-mdc-paginator-navigation-next")]
        private IWebElement NextPageButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".mat-mdc-paginator-navigation-previous")]
        private IWebElement PreviousPageButton { get; set; }

        // New Elements
        [FindsBy(How = How.XPath, Using = "//*[@id=\"kt_create_account_form\"]/div/app-step1s/div/form/div/div/div[1]/input")]
        public IWebElement BEnameInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/div/div[2]/div/app-step1s/div/form/div/div/div[2]/input")]
        public IWebElement BETinNumberInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"kt_create_account_form\"]/div/app-step1s/div/form/div/div/div[3]/div[2]/input")]
        private IWebElement BERegisterIDInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/div/div[2]/div/app-step1s/div/form/div/div/div[4]/input")]
        private IWebElement BEsstInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/div/div[2]/div/app-step1s/div/form/div/div/div[5]/input")]
        private IWebElement BETTRegisterNumberInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[2]/input")]
        private IWebElement BEContactNumberInput { get; set; }
        

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[3]/input")]
        private IWebElement BEemailInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[4]/div[1]/input")]
        private IWebElement BECityInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[4]/div[3]/input")]
        private IWebElement BEPosCodeInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[6]/input")]
        private IWebElement BEAddress1Input { get; set; }
        
        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[7]/input")]
        private IWebElement BEAddress2Input { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[8]/input")]
        private IWebElement BEAddress3Input { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[3]/div/div[2]/button")]
        private IWebElement SaveButton { get; set; }

        // Methods
        public void SearchBusinessEntity(string searchText)
        {
            var searchBox = new WebDriverWait(_driver, TimeSpan.FromSeconds(5))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='kt_filter_search']")));

            searchBox.Clear();
            searchBox.SendKeys(searchText);

        }

        public void ClickNewButton()
        {
            var newButton = _wait.Until(ExpectedConditions.ElementToBeClickable(NewButton));
            newButton.Click();
        }

        public void ClickEditButton(string BETinNumber)
        {
            // Search by entering BETinNumber
            var searchInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//input[@type='text' and @placeholder='Search']")));
            searchInput.Clear();
            searchInput.SendKeys(BETinNumber);
            searchInput.SendKeys(Keys.Enter); // Trigger the AJAX search (if needed)

            // Wait for the matching row with BETinNumber to appear (dynamic load)
            string rowXpath = $"//table/tbody/tr[td[contains(normalize-space(), '{BETinNumber}')]]";
            var row = _wait.Until(driver =>
            {
                var rows = driver.FindElements(By.XPath(rowXpath));
                return rows.Count == 1 ? rows[0] : null;
            });

            // Inside that row, find the edit icon's <a> in td[13]
            var editAnchor = row.FindElement(By.XPath("./td[13]//a[.//i[contains(@class, 'bi-pencil')]]"));

            // Wait for it to be clickable and click
            _wait.Until(ExpectedConditions.ElementToBeClickable(editAnchor)).Click();
        }



        public void DeleteStoreCountryByCode(string code)
        {
            // Locate the row by its code
            string xpath = $"//tr[normalize-space(td)='{code}']//button[contains(@class, 'btn-delete-hover')]";
            var deleteButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpath)));

            // Click the delete button
            deleteButton.Click();
        }


        public void ClickImportButton()
        {
            ImportButton.Click();
        }

        public void ClickUploadButton()
        {
            UploadButton.Click();
        }
        

        public void ClickExportButton()
        {
            ExportButton.Click();
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
        public void EnterBEname(string BEname)
        {
            BEnameInput.Clear();
            BEnameInput.SendKeys(BEname);
        }

        public void EnterBETinNumber(string BETinNumber)
        {
            BETinNumberInput.Clear();
            BETinNumberInput.SendKeys(BETinNumber);
        }

       
        public void EnterBEsst(string BEsst)
        {
            BEsstInput.Clear();
            BEsstInput.SendKeys(BEsst);
        }

        public void EnterBERegisterID(string BERegisterID)
        {
            BERegisterIDInput.Clear();
            BERegisterIDInput.SendKeys(BERegisterID);
        }

        public void EnterBETTRegisterNumber(string BETTRegisterNumber)
        {
            BETTRegisterNumberInput.Clear();
            BETTRegisterNumberInput.SendKeys(BETTRegisterNumber);
        }

        public void EnterBEContactNumber(string BEContactNumber)
        {
            BEContactNumberInput.Clear();
            BEContactNumberInput.SendKeys(BEContactNumber);
        }
        

        public void EnterBEemail(string BEemail)
        {
            BEemailInput.Clear();
            BEemailInput.SendKeys(BEemail);
        }

        public void EnterBECity(string BECity)
        {
            BECityInput.Clear();
            BECityInput.SendKeys(BECity);
        }

        public void EnterBEPosCode(string BEPosCode)
        {
            BEPosCodeInput.Clear();
            BEPosCodeInput.SendKeys(BEPosCode);
        }

        public void EnterBEAddress1(string BEAddress1)
        {
            BEAddress1Input.Clear();
            BEAddress1Input.SendKeys(BEAddress1);
        }

        public void EnterBEAddress2(string BEAddress2)
        {
            BEAddress2Input.Clear();
            BEAddress2Input.SendKeys(BEAddress2);
        }
        public void EnterBEAddress3(string BEAddress3)
        {
            BEAddress3Input.Clear();
            BEAddress3Input.SendKeys(BEAddress3);
        }
        public void ClickSaveButton()
        {
            var saveButton = _wait.Until(ExpectedConditions.ElementToBeClickable(SaveButton));
            saveButton.Click();
        }

        public void ClickContinueButton()
        {
            var continueButton = _wait.Until(ExpectedConditions.ElementToBeClickable(ContinueButton));
            continueButton.Click();
        }
        

        public void ClickFilterALLCategoryButton()
        {
            var filterAllButton = _wait.Until(ExpectedConditions.ElementToBeClickable(FilterALLCategoryButton));
            filterAllButton.Click();
        }

        public void ClickFilterActiveCategoryButton()
        {
            var filterActiveButton = _wait.Until(ExpectedConditions.ElementToBeClickable(FilterActiveCategoryButton));
            filterActiveButton.Click();
        }

        public void ClickFilterInactiveCategoryButton()
        {
            var filterInactiveButton = _wait.Until(ExpectedConditions.ElementToBeClickable(FilterInactiveCategoryButton));
            filterInactiveButton.Click();
        }

        public void ClickFilterPendingCategoryButton()
        {
            var filterPendingButton = _wait.Until(ExpectedConditions.ElementToBeClickable(FilterPendingCategoryButton));
            filterPendingButton.Click();
        }

        public void ClickFilterSuccessCategoryButton()
        {
            var filterSuccessButton = _wait.Until(ExpectedConditions.ElementToBeClickable(FilterSuccessCategoryButton));
            filterSuccessButton.Click();
        }

        public void ClickFilterFailedCategoryButton()
        {
            var filterFailedButton = _wait.Until(ExpectedConditions.ElementToBeClickable(FilterFailedCategoryButton));
            filterFailedButton.Click();
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

        public bool WaitForFileDownload(string folderPath, string filePrefix, TimeSpan timeout)
        {
            string todayDate = DateTime.Now.ToString("yyyyMMdd");
            string expectedBaseName = $"{filePrefix}_{todayDate}";

            // Capture existing files before export
            var existingFiles = new HashSet<string>(Directory.GetFiles(folderPath));

            var endTime = DateTime.Now + timeout;
            while (DateTime.Now < endTime)
            {
                var currentFiles = Directory.GetFiles(folderPath);
                foreach (var file in currentFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);

                    // File matches naming pattern and is NEW
                    if (fileName.StartsWith(expectedBaseName) && !existingFiles.Contains(file))
                    {
                        var fileInfo = new FileInfo(file);
                        if (fileInfo.Length > 0)
                            return true;
                    }
                }

                Thread.Sleep(500);
            }

            return false; // No new matching file found in time
        }


    }
}
