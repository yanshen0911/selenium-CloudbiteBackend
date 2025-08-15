using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTests.Pages.Customer


{
    public class CustomerPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Constructor
        public CustomerPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            PageFactory.InitElements(_driver, this);
        }

        // Elements
        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-customer > div > div.d-flex.gap-2.justify-content-start > div > ul > li.nav-item.ps-9.pt-3 > a")]
        private IWebElement FilterALLCategoryButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-customer > div > div.d-flex.gap-2.justify-content-start > div > ul > li:nth-child(2) > a")]
        private IWebElement FilterActiveCategoryButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-customer > div > div.d-flex.gap-2.justify-content-start > div > ul > li:nth-child(3) > a")]
        private IWebElement FilterInactiveCategoryButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-customer > div > div.d-flex.gap-2.justify-content-start > div > ul > li.ms-auto.align-items-center.pt-3.mobileHideFilter > div > div:nth-child(2) > span")]
        private IWebElement FilterPendingCategoryButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-customer > div > div.d-flex.gap-2.justify-content-start > div > ul > li.ms-auto.align-items-center.pt-3.mobileHideFilter > div > div:nth-child(3) > span")]
        private IWebElement FilterSuccessCategoryButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-customer > div > div.d-flex.gap-2.justify-content-start > div > ul > li.ms-auto.align-items-center.pt-3.mobileHideFilter > div > div:nth-child(4) > span")]
        private IWebElement FilterFailedCategoryButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-customer > div > div.card-header.border-0.pt-5 > div > div:nth-child(1) > a")]
        private IWebElement ImportButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_body > ngb-modal-window > div > div > app-upload-modal > div > div.modal-footer.justify-content-end.d-flex.ng-star-inserted > button.btn.btn-primary.mx-2")]
        private IWebElement UploadButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#kt_body > ngb-modal-window > div > div > app-upload-modal > div > div.modal-body.px-20 > div > div > div.d-flex.align-items-center > button")]
        private IWebElement DownloadButton { get; set; }


        [FindsBy(How = How.CssSelector, Using = "#kt_content_container > app-customer > div > div.card-header.border-0.pt-5 > div > div:nth-child(2) > a")]
        private IWebElement ExportButton { get; set; }


        [FindsBy(How = How.XPath, Using = "/html/body/app-layout/div/div/div/div/app-content/app-customer/div/div[3]/div/div[3]/a")]
        private IWebElement NewButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"kt_body\"]/ngb-modal-window/div/div/app-customer-modal/div/div[3]/div/div[2]/button")]
        private IWebElement ContinueButton { get; set; }

        // New Elements
        [FindsBy(How = How.XPath, Using = "//*[@id=\"kt_create_account_form\"]/div/app-step1/div/form/div/div/div[2]/input")]
        public IWebElement CustnameInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"kt_create_account_form\"]/div/app-step1/div/form/div/div/div[3]/input")]
        public IWebElement CustTinNumberInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"kt_create_account_form\"]/div/app-step1/div/form/div/div/div[4]/div[2]/input")]
        private IWebElement CustRegisterIDInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step1/div/form/div/div/div[5]/input")]
        private IWebElement CustsstInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step1/div/form/div/div/div[6]/input")]
        private IWebElement CustEmailInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step2/div/form/div/div/div[1]/input")]
        private IWebElement CustContactNumberInput { get; set; }
        

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-store-modal/div/div[2]/div/div/div[2]/div/app-step2s/div/div/form/div/div/div[3]/input")]
        private IWebElement BEemailInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step2/div/form/div/div/div[2]/div[1]/input")]
        private IWebElement CustomerCityInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step2/div/form/div/div/div[2]/div[3]/input")]
        private IWebElement CustPosCodeInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step2/div/form/div/div/div[4]/input")]
        private IWebElement CustAddress1Input { get; set; }
        
        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step2/div/form/div/div/div[5]/input")]
        private IWebElement CustAddress2Input { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step2/div/form/div/div/div[6]/input")]
        private IWebElement CustAddress3Input { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[2]/div/div/div[2]/div/app-step2/div/form/div/div/div[7]/input")]
        private IWebElement CustExternalCodeInput { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/ngb-modal-window/div/div/app-customer-modal/div/div[3]/div/div[2]/button")]
        private IWebElement SaveButton { get; set; }

        // Methods
        public void SearchCustomer(string searchText)
        {
            var searchBox = new WebDriverWait(_driver, TimeSpan.FromSeconds(5))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-customer/div/div[3]/h3/div/input")));

            searchBox.Clear();
            searchBox.SendKeys(searchText);

        }

        public void ClickNewButton()
        {
            var newButton = _wait.Until(ExpectedConditions.ElementToBeClickable(NewButton));
            newButton.Click();
        }

        public void ClickEditButton(string CustomerCode)
        {
            // Search by entering CustomerCode
            var searchInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//input[@type='text' and @placeholder='Search']")));
            searchInput.Clear();
            searchInput.SendKeys(CustomerCode);
            searchInput.SendKeys(Keys.Enter); // Trigger the AJAX search (if needed)

            // Wait for the matching row with CustomerCode to appear (dynamic load)
            string rowXpath = $"//table/tbody/tr[td[contains(normalize-space(), '{CustomerCode}')]]";
            var row = _wait.Until(driver =>
            {
                var rows = driver.FindElements(By.XPath(rowXpath));
                return rows.Count == 1 ? rows[0] : null;
            });

            // Inside that row, find the edit anchor in td[12]
            var editAnchor = row.FindElement(By.XPath(".//td[12]/div/a"));

            // Click the edit icon
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

        public void ClickDownloadTemplateButton()
        {
            DownloadButton.Click();
        }

        public void ClickExportButton()
        {
            ExportButton.Click();
        }

        // New Methods for Form Fields
        public void EnterCustname(string Custname)
        {
            CustnameInput.Clear();
            CustnameInput.SendKeys(Custname);
        }

        public void EnterCustTinNumber(string CustTinNumber)
        {
            CustTinNumberInput.Clear();
            CustTinNumberInput.SendKeys(CustTinNumber);
        }

       
        public void EnterCustsst(string Custsst)
        {
            CustsstInput.Clear();
            CustsstInput.SendKeys(Custsst);
        }

        public void EnterCustRegisterID(string CustRegisterID)
        {
            CustRegisterIDInput.Clear();
            CustRegisterIDInput.SendKeys(CustRegisterID);
        }

        public void EnterCustEmail(string CustEmail)
        {
            CustEmailInput.Clear();
            CustEmailInput.SendKeys(CustEmail);
        }

        public void EnterCustContactNumber(string BEContactNumber)
        {
            CustContactNumberInput.Clear();
            CustContactNumberInput.SendKeys(BEContactNumber);
        }
        

        public void EnterBEemail(string BEemail)
        {
            BEemailInput.Clear();
            BEemailInput.SendKeys(BEemail);
        }

        public void EnterCustomerCity(string CustCity)
        {
            CustomerCityInput.Clear();
            CustomerCityInput.SendKeys(CustCity);
        }

        public void EnterCustPosCode(string BEPosCode)
        {
            CustPosCodeInput.Clear();
            CustPosCodeInput.SendKeys(BEPosCode);
        }

        public void EnterCustAddress1(string CustAddress1)
        {
            CustAddress1Input.Clear();
            CustAddress1Input.SendKeys(CustAddress1);
        }

        public void EnterCustAddress2(string CustAddress2)
        {
            CustAddress2Input.Clear();
            CustAddress2Input.SendKeys(CustAddress2);
        }
        public void EnterCustAddress3(string CustAddress3)
        {
            CustAddress3Input.Clear();
            CustAddress3Input.SendKeys(CustAddress3);
        }

        public void EnterCustExternalCode(string CustExternalCode)
        {
            CustExternalCodeInput.Clear();
            CustExternalCodeInput.SendKeys(CustExternalCode);
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
