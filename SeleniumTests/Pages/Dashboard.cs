using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace SeleniumTests.Pages
{
    public class Dashboard
    {
        private readonly IWebDriver driver;

        public Dashboard(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }
        // Define the locator as a By object instead of just IWebElement
        public By BreadCrumbLocator => 
            By.XPath("/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/app-dashboard/div[1]/div/div");

        // Update the locator for the BreadCrumb to a more specific XPath
        [FindsBy(How = How.XPath, 
            Using = "/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/app-dashboard/div[1]/div/div")]
        public IWebElement BreadCrumb;

        // Locator for the welcome message div using XPath to target the h2 element with specific text
        [FindsBy(How = How.XPath, Using = "/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/app-dashboard/div[2]/div/div/h2")]
        private IWebElement WelcomeMessageDiv;


        [FindsBy(How = How.XPath, Using = "/html/body/app-root/body/app-main-layout/app-header/div/mat-toolbar/div[4]/span")]
        private IWebElement UserProfileDropDown;

        [FindsBy(How = How.XPath, Using = "//*[@id=\"mat-menu-panel-0\"]/div/button/span/div")]
        private IWebElement LogOutButton;


        [FindsBy(How = How.Id, Using = "selLanguage")]
        private IWebElement LanguageDropDown;

        // Method to get BreadCrumb text
        public string GetBreadCrumbText()
        {
            return BreadCrumb.Text;
        }

        // Method to get the welcome message text
        public string GetWelcomeMessageText()
        {
            return WelcomeMessageDiv.Text;
        }

        // Method to perform logout
        public void Logout()
        {

            UserProfileDropDown.Click(); // Open user profile dropdown

            // Wait until the logout button is clickable
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(LogOutButton));

            LogOutButton.Click(); // Click the logout button

        }

        //// Method to switch the language based on the language code
        //public void SwitchLanguage(string languageCode)
        //{

        //    UserProfileDropDown.Click(); // Open user profile dropdown

        //    // Wait until the dropdown is visible and clickable
        //    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(LanguageDropDown));

        //    // Create a SelectElement to interact with the dropdown
        //    LanguageDropDown.Click();
        //    SelectElement selectLanguage = new SelectElement(LanguageDropDown);
        //    LanguageDropDown.Click();

        //    selectLanguage.SelectByValue(languageCode); 
        
        //}

        // Method to switch the language based on the language code
        public void SwitchLanguage(string languageCode)
        {
            // Open the language dropdown
            LanguageDropDown.Click();

            // Wait for the dropdown to be visible and clickable
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("selLanguage")));

            // Open the dropdown for languages
            SelectElement languageSelect = new SelectElement(LanguageDropDown);

            // Select the appropriate language by value (language code)
            languageSelect.SelectByValue(languageCode);

            // Ensure the page updates by waiting for the language switch to take effect
            wait.Until(d => d.FindElement(BreadCrumbLocator).Text != string.Empty); // Ensure breadcrumb changes after language switch
        }
    }
}
