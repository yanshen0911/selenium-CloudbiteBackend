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
        public StorePage(IWebDriver driver)
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

        [FindsBy(How = How.CssSelector, Using = "body > app-management > div > mat-sidenav-container > mat-sidenav-content > div:nth-child(2) > app-store > div > div > app-store-group > div.footerMarginTop > button")]
        private IWebElement ExportButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "body > app-management > div > mat-sidenav-container > mat-sidenav-content > div:nth-child(2) > app-store > div > div > app-store-zone > div.footerMarginTop > button")]
        private IWebElement ExportLocationButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "body > app-management > div > mat-sidenav-container > mat-sidenav-content > div:nth-child(2) > app-store > div > div > app-store-setup > div.footerMarginTop > button")]
        private IWebElement ExportStoreButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "body > app-management > div > mat-sidenav-container > mat-sidenav-content > div:nth-child(2) > app-store > div > div > app-store-group > p-toolbar > div > div.p-toolbar-group-right.ng-star-inserted > button")]
        private IWebElement NewGroupButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#mat-tab-content-0-0 > div > p-toolbar > div > div.p-toolbar-group-right.ng-star-inserted > button")]
        private IWebElement NewCountryButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#mat-tab-content-0-1 > div > p-toolbar > div > div.p-toolbar-group-right.ng-star-inserted > button")]
        private IWebElement NewStateButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#mat-tab-content-0-2 > div > p-toolbar > div > div.p-toolbar-group-right.ng-star-inserted > button")]
        private IWebElement NewCityButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "body > app-management > div > mat-sidenav-container > mat-sidenav-content > div:nth-child(2) > app-store > div > div > app-store-setup > p-toolbar > div > div.p-toolbar-group-right.ng-star-inserted > button")]
        private IWebElement NewStoreButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#mat-tab-content-0-2 > div > p-toolbar > div > div.p-toolbar-group-right.ng-star-inserted > button")]
        private IWebElement NewTerminalButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"kt_body\"]/ngb-modal-window/div/div/app-store-modal/div/div[3]/div/div[2]/button")]
        private IWebElement ContinueButton { get; set; }

        // New Elements
        [FindsBy(How = How.XPath, Using = "//*[@id=\"groupIdStoreGroup\"]")]
        public IWebElement GroupcodeInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"zoneIdStoreCountry\"]")]
        public IWebElement CountrycodeInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"regionIdStoreState\"]")]
        public IWebElement StatecodeInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"areaIdStoreCity\"]")]
        public IWebElement CitycodeInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"idStoreSetup\"]")]
        public IWebElement StorecodeInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"groupDesc1StoreGroup\"]")]
        public IWebElement GroupDescInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"zoneDesc1StoreCountry\"]")]
        public IWebElement CountryDescInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"regionDesc1StoreState\"]")]
        public IWebElement StateDescInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"areaDesc1StoreCity\"]")]
        public IWebElement CityDescInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"desc1StoreSetup\"]")]
        public IWebElement StoreNameInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"desc2StoreSetup\"]")]
        public IWebElement StoreDescInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"ptcStoreSetup\"]")]
        public IWebElement ContactPersonInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"add1StoreSetup\"]")]
        public IWebElement Address1Input { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"add2StoreSetup\"]")]
        public IWebElement Address2Input { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"add3StoreSetup\"]")]
        public IWebElement Address3Input { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"add4StoreSetup\"]")]
        public IWebElement Address4Input { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"postCodeStoreSetup\"]")]
        private IWebElement PostCodeInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"emailStoreSetup\"]")]
        private IWebElement StoreEmailInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"phoneNo1StoreSetup\"]")]
        private IWebElement StorePhoneNumberInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"eatmolMerchantStoreSetup\"]")]
        private IWebElement EatmolMerchantIDInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"phoneNumber\"]")]
        private IWebElement FooterPhoneNumberInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"email\"]")]
        private IWebElement FooterEmailInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"footer\"]")]
        private IWebElement FooterMessageInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"terminalIdCheck\"]")]
        private IWebElement TerminalIDInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"terminalDesc1Check\"]")]
        private IWebElement TerminalDescInput { get; set; }

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
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"storeGroupSearch\"]")));

            searchBox.Clear();
            searchBox.SendKeys(searchText);

        }

        public void SearchStoreCountry(string searchText)
        {
            var searchBox = new WebDriverWait(_driver, TimeSpan.FromSeconds(5))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"storeCountrySearch\"]")));

            searchBox.Clear();
            searchBox.SendKeys(searchText);

        }

        public void SearchStoreState(string searchText)
        {
            var searchBox = new WebDriverWait(_driver, TimeSpan.FromSeconds(5))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"storeStateSearch\"]")));

            searchBox.Clear();
            searchBox.SendKeys(searchText);

        }

        public void SearchStoreCity(string searchText)
        {
            var searchBox = new WebDriverWait(_driver, TimeSpan.FromSeconds(5))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"storeCitySearch\"]")));

            searchBox.Clear();
            searchBox.SendKeys(searchText);

        }

        public void SearchStore_Detail(string searchText)
        {
            var searchBox = new WebDriverWait(_driver, TimeSpan.FromSeconds(5))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"storeSetupSearch\"]")));

            searchBox.Clear();
            searchBox.SendKeys(searchText);

        }

        public void ClickNewGroupButton()
        {
            var newGroupButton = _wait.Until(ExpectedConditions.ElementToBeClickable(NewGroupButton));
            newGroupButton.Click();
        }
        public void ClickNewCountryButton()
        {
            var newCountryButton = _wait.Until(ExpectedConditions.ElementToBeClickable(NewCountryButton));
            newCountryButton.Click();
        }

        public void ClickNewStateButton()
        {
            var newStateButton = _wait.Until(ExpectedConditions.ElementToBeClickable(NewStateButton));
            newStateButton.Click();
        }

        public void ClickNewCityButton()
        {
            var newCityButton = _wait.Until(ExpectedConditions.ElementToBeClickable(NewCityButton));
            newCityButton.Click();
        }

        public void ClickNewStoreButton()
        {
            var newStoreButton = _wait.Until(ExpectedConditions.ElementToBeClickable(NewStoreButton));
            newStoreButton.Click();
        }


        public void ClickNewTerminalButton()
        {
            var newTerminalButton = _wait.Until(ExpectedConditions.ElementToBeClickable(NewTerminalButton));
            newTerminalButton.Click();
        }

        public void ClickEditButton(string Groupcode)
        {
            // Search by entering Groupcode
            var searchInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//*[@id=\"storeGroupSearch\"]")));
            searchInput.Clear();
            searchInput.SendKeys(Groupcode);
            searchInput.SendKeys(Keys.Enter);

            // Dynamic XPath to find a row containing Groupcode
            string rowXpath = $"//table/tbody/tr[td[contains(normalize-space(.), '{Groupcode}')]]";

            var row = _wait.Until(driver =>
            {
                var rows = driver.FindElements(By.XPath(rowXpath));
                return rows.FirstOrDefault(); // return the first match
            });


            // Find the edit icon inside column 4 of this row
            var editIcon = row.FindElement(By.XPath(".//td[4]//img[contains(@class,'store-setup-edit11')]"));

            // Wait for it to be clickable and then click
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(editIcon)).Click();


        }

        public void ClickEditCountryButton(string Countrycode)
        {
            // Search by entering Countrycode
            var searchInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//*[@id=\"storeCountrySearch\"]")));
            searchInput.Clear();
            searchInput.SendKeys(Countrycode);
            searchInput.SendKeys(Keys.Enter);

            // Dynamic XPath to find a row containing Countrycode
            string rowXpath = $"//table/tbody/tr[td[contains(normalize-space(.), '{Countrycode}')]]";

            var row = _wait.Until(driver =>
            {
                var rows = driver.FindElements(By.XPath(rowXpath));
                return rows.FirstOrDefault(); // return the first match
            });


            // Find the edit icon inside column 4 of this row
            var editIcon = row.FindElement(By.XPath(".//td[6]//img[contains(@class,'store-setup-edit11')]"));

            // Wait for it to be clickable and then click
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(editIcon)).Click();


        }


        public void ClickEditStateButton(string Statecode)
        {
            // Search by entering Statecode
            var searchInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//*[@id=\"storeStateSearch\"]")));
            searchInput.Clear();
            searchInput.SendKeys(Statecode);
            searchInput.SendKeys(Keys.Enter);

            // Dynamic XPath to find a row containing Statecode
            string rowXpath = $"//table/tbody/tr[td[contains(normalize-space(.), '{Statecode}')]]";

            var row = _wait.Until(driver =>
            {
                var rows = driver.FindElements(By.XPath(rowXpath));
                return rows.FirstOrDefault(); // return the first match
            });


            // Find the edit icon inside column 4 of this row
            var editIcon = row.FindElement(By.XPath(".//td[5]//img[contains(@class,'store-setup-edit11')]"));

            // Wait for it to be clickable and then click
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(editIcon)).Click();


        }


        public void ClickEditCityButton(string Citycode)
        {
            // Search by entering Citycode
            var searchInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//*[@id=\"storeCitySearch\"]")));
            searchInput.Clear();
            searchInput.SendKeys(Citycode);
            searchInput.SendKeys(Keys.Enter);

            // Dynamic XPath to find a row containing Citycode
            string rowXpath = $"//table/tbody/tr[td[contains(normalize-space(.), '{Citycode}')]]";

            var row = _wait.Until(driver =>
            {
                var rows = driver.FindElements(By.XPath(rowXpath));
                return rows.FirstOrDefault(); // return the first match
            });


            // Find the edit icon inside column 4 of this row
            var editIcon = row.FindElement(By.XPath(".//td[5]//img[contains(@class,'store-setup-edit11')]"));

            // Wait for it to be clickable and then click
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(editIcon)).Click();


        }

        public void ClickEditStoreButton(string Storecode)
        {
            // Search by entering Citycode
            var searchInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//*[@id=\"storeSetupSearch\"]")));
            searchInput.Clear();
            searchInput.SendKeys(Storecode);
            searchInput.SendKeys(Keys.Enter);

            // Dynamic XPath to find a row containing Citycode
            string rowXpath = $"//table/tbody/tr[td[contains(normalize-space(.), '{Storecode}')]]";

            var row = _wait.Until(driver =>
            {
                var rows = driver.FindElements(By.XPath(rowXpath));
                return rows.FirstOrDefault(); // return the first match
            });


            // Find the edit icon inside column 4 of this row
            var editIcon = row.FindElement(By.XPath(".//td[10]//img[contains(@class,'store-setup-edit11')]"));

            // Wait for it to be clickable and then click
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(editIcon)).Click();


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

        public void ClickExportLocationButton()
        {
            ExportLocationButton.Click();
        }

        public void ClickExportStoreButton()
        {
            ExportStoreButton.Click();
        }

        // New Methods for Form Fields
        public void EnterGroupcode(string Groupcode)
        {
            GroupcodeInput.Clear();
            GroupcodeInput.SendKeys(Groupcode);
        }

        public void EnterCountrycode(string Countrycode)
        {
            CountrycodeInput.Clear();
            CountrycodeInput.SendKeys(Countrycode);
        }
        public void EnterStatecode(string Statecode)
        {
            StatecodeInput.Clear();
            StatecodeInput.SendKeys(Statecode);
        }

        public void EnterCitycode(string Citycode)
        {
            CitycodeInput.Clear();
            CitycodeInput.SendKeys(Citycode);
        }

        public void EnterStorecode(string Storecode)
        {
            StorecodeInput.Clear();
            StorecodeInput.SendKeys(Storecode);
        }
        public void EnterGroupDesc(string GroupDesc)
        {
            GroupDescInput.Clear();
            GroupDescInput.SendKeys(GroupDesc);
        }

        public void EnterCountryDesc(string CountryDesc)
        {
            CountryDescInput.Clear();
            CountryDescInput.SendKeys(CountryDesc);
        }

        public void EnterStateDesc(string StateDesc)
        {
            StateDescInput.Clear();
            StateDescInput.SendKeys(StateDesc);
        }
        public void EnterCityDesc(string CityDesc)
        {
            CityDescInput.Clear();
            CityDescInput.SendKeys(CityDesc);
        }

        public void EnterStoreName(string StoreName)
        {
            StoreNameInput.Clear();
            StoreNameInput.SendKeys(StoreName);
        }

        public void EnterStoreDesc(string StoreDesc)
        {
            StoreDescInput.Clear();
            StoreDescInput.SendKeys(StoreDesc);
        }
        public void EnterStoreContactPerson(string Contactperson)
        {
            ContactPersonInput.Clear();
            ContactPersonInput.SendKeys(Contactperson);
        }

        public void EnterStore1Address(string Address1)
        {
            Address1Input.Clear();
            Address1Input.SendKeys(Address1);
        }

        public void EnterStore2Address(string Address2)
        {
            Address2Input.Clear();
            Address2Input.SendKeys(Address2);
        }

        public void EnterStore3Address(string Address3)
        {
            Address3Input.Clear();
            Address3Input.SendKeys(Address3);
        }

        public void EnterStore4Address(string Address4)
        {
            Address4Input.Clear();
            Address4Input.SendKeys(Address4);
        }

        public void EnterStorePostCode(string PostCode)
        {
            PostCodeInput.Clear();
            PostCodeInput.SendKeys(PostCode);
        }

        public void EnterStoreEmail(string StoreEmail)
        {
            StoreEmailInput.Clear();
            StoreEmailInput.SendKeys(StoreEmail);
        }

        public void EnterStorePhoneNumber(string StoreEmail)
        {
            StorePhoneNumberInput.Clear();
            StorePhoneNumberInput.SendKeys(StoreEmail);
        }

        public void EnterEatmolMerchantID(string EatmolID)
        {
            EatmolMerchantIDInput.Clear();
            EatmolMerchantIDInput.SendKeys(EatmolID);
        }

        public void EnterFooterPhoneNumber(string FooterPhoneNumber)
        {
            FooterPhoneNumberInput.Clear();
            FooterPhoneNumberInput.SendKeys(FooterPhoneNumber);
        }

        public void EnterFooterEmail(string FooterEmail)
        {
            FooterEmailInput.Clear();
            FooterEmailInput.SendKeys(FooterEmail);
        }

        public void EnterFooterMessage(string FooterMessage)
        {
            FooterMessageInput.Clear();
            FooterMessageInput.SendKeys(FooterMessage);
        }

        public void EnterTerminalID(string TerminalID)
        {
            TerminalIDInput.Clear();
            TerminalIDInput.SendKeys(TerminalID);
        }

        public void EnterTerminalDesc(string TerminalDesc)
        {
            TerminalDescInput.Clear();
            TerminalDescInput.SendKeys(TerminalDesc);
        }

        public void SetCheckboxState(bool isChecked)
        {
            var checkbox = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(
                By.XPath("//*[@id=\"groupActiveStoreGroup\"]")
            ));

            if (checkbox.Selected != isChecked)
            {
                checkbox.Click();
            }
        }

        public void SetCheckboxStoreState(bool isChecked)
        {
            var checkbox = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(
                By.XPath("//*[@id=\"activeStoreSetup\"]")
            ));

            if (checkbox.Selected != isChecked)
            {
                checkbox.Click();
            }
        }

        public void SetCountryCheckboxState(bool isChecked)
        {
            var checkbox = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(
                By.XPath("//*[@id=\"storeActiveStoreCountry\"]")
            ));

            if (checkbox.Selected != isChecked)
            {
                checkbox.Click();
            }
        }

        public void SetEReceiptCheckboxState(bool isChecked)
        {
            var checkbox = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(
                By.XPath("//*[@id=\"enableEReceipt\"]")
            ));

            if (checkbox.Selected != isChecked)
            {
                checkbox.Click();
            }
        }

        public void SetEReceiptEmailCheckboxState(bool isChecked)
        {
            var checkbox = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(
                By.XPath("//*[@id=\"sendViaEmail\"]")
            ));

            if (checkbox.Selected != isChecked)
            {
                checkbox.Click();
            }
        }

        public void SetEReceiptGenerateEmailCheckboxState(bool isChecked)
        {
            var checkbox = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(
                By.XPath("//*[@id=\"generatePDF\"]")
            ));

            if (checkbox.Selected != isChecked)
            {
                checkbox.Click();
            }
        }


        public void SetTerminalStatusCheckboxState(bool isChecked)
        {
            var checkbox = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(
                By.XPath("//*[@id=\"terminalActiveCheck\"]")
            ));

            if (checkbox.Selected != isChecked)
            {
                checkbox.Click();
            }
        }

        public void EnterBEsst(string BEsst)
        {
            BEsstInput.Clear();
            BEsstInput.SendKeys(BEsst);
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
            var existingFiles = new HashSet<string>(Directory.GetFiles(folderPath));

            var endTime = DateTime.Now + timeout;
            while (DateTime.Now < endTime)
            {
                var currentFiles = Directory.GetFiles(folderPath);
                foreach (var file in currentFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);

                    // Match by prefix only; optionally check date if needed
                    if (fileName.StartsWith(filePrefix) && !existingFiles.Contains(file))
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
