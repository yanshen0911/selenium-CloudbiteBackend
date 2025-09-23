using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTests.Pages.Stores


{
    public class StoresPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Constructor
        public StoresPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            PageFactory.InitElements(_driver, this);
        }

        // Elements
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

        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-store > div > div.card-header.border-0.pt-5 > div > div:nth-child(1) > a")]
        private IWebElement ImportButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_body > ngb-modal-window > div > div > app-upload-modal > div > div.modal-footer.justify-content-end.d-flex.ng-star-inserted > button.btn.btn-primary.mx-2")]
        private IWebElement UploadButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_body > ngb-modal-window > div > div > app-upload-modal > div > div.modal-body.px-20 > div > div > div.d-flex.align-items-center > button")]
        private IWebElement DownloadButton { get; set; }


        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-store > div > div.card-header.border-0.pt-5 > div > div:nth-child(2) > a")]
        private IWebElement ExportButton { get; set; }


        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-store > div > div.card-header.border-0.pt-5 > div > div:nth-child(3) > a")]
        private IWebElement NewButton { get; set; }


        [FindsBy(How = How.XPath, Using = "//*[@id=\"kt_body\"]/ngb-modal-window/div/div/app-store-modal/div/div[3]/div/div[2]/button")]
        private IWebElement ContinueButton { get; set; }

        // New Elements
        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[2]/input")]
        public IWebElement StorenameInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[3]/div[1]/input")]
        public IWebElement StoreCityInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[3]/div[3]/input")]
        private IWebElement StorePostCodeInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/div/div[2]/div/app-step1s/div/form/div/div/div[4]/input")]
        private IWebElement BEsstInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[5]/input")]
        private IWebElement StoreAddress1Input { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[6]/input")]
        private IWebElement StoreAddress2Input { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/form/div/div/div[8]/input")]
        private IWebElement ExternalCodeInput { get; set; }
        

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

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[3]/button")]
        private IWebElement SaveButton { get; set; }

        // Methods
        public void SearchStore(string searchText)
        {
            var searchBox = new WebDriverWait(_driver, TimeSpan.FromSeconds(5))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"kt_filter_search\"]")));

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
            searchInput.SendKeys(Keys.Enter);

            // Wait for the row that contains the BETinNumber
            string rowXpath = $"//table/tbody/tr[td[contains(normalize-space(), '{BETinNumber}')]]";
            var row = _wait.Until(driver =>
            {
                var rows = driver.FindElements(By.XPath(rowXpath));
                return rows.Count == 1 ? rows[0] : null;
            });

            // Find the pencil icon inside that row (relative XPath)
            var editIcon = row.FindElement(By.XPath(".//i[contains(@class,'bi-pencil')]"));

            // Wait for it to be clickable and click
            _wait.Until(ExpectedConditions.ElementToBeClickable(editIcon)).Click();
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

        public void ClickDownloadTemplateButton()
        {
            DownloadButton.Click();
        }

        public void ClickExportButton()
        {
            ExportButton.Click();
        }

        // New Methods for Form Fields
        public void EnterStorename(string Storename)
        {
            StorenameInput.Clear();
            StorenameInput.SendKeys(Storename);
        }

        public void EnterStoreCity(string StoreCity)
        {
            StoreCityInput.Clear();
            StoreCityInput.SendKeys(StoreCity);
        }

       
        public void EnterBEsst(string BEsst)
        {
            BEsstInput.Clear();
            BEsstInput.SendKeys(BEsst);
        }

        public void EnterstrPostCode(string strPostCode)
        {
            StorePostCodeInput.Clear();
            StorePostCodeInput.SendKeys(strPostCode);
        }

        public void EnterStoreAddress1(string StoreAddress1)
        {
            StoreAddress1Input.Clear();
            StoreAddress1Input.SendKeys(StoreAddress1);
        }

        public void EnterStoreAddress2(string StoreAddress2)
        {
            StoreAddress2Input.Clear();
            StoreAddress2Input.SendKeys(StoreAddress2);
        }

        public void EnterExternalCode(string ExternalCode)
        {
            ExternalCodeInput.Clear();
            ExternalCodeInput.SendKeys(ExternalCode);
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
            var existingFiles = new HashSet<string>(Directory.GetFiles(folderPath));

            var endTime = DateTime.Now + timeout;
            while (DateTime.Now < endTime)
            {
                var currentFiles = Directory.GetFiles(folderPath);
                foreach (var file in currentFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);

                    // Match by prefix only; optionally check date if needed
                    if (fileName.StartsWith(filePrefix) && fileName.Contains(todayDate) && !existingFiles.Contains(file))
                    {
                        var fileInfo = new FileInfo(file);
                        if (fileInfo.Length > 0)
                            return true;
                    }
                }

                Thread.Sleep(500);
            }

            return false;
        }



    }
}
