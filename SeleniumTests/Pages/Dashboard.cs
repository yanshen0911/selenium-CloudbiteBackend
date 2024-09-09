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

        public By BreadCrumbLocator => By.ClassName("breadcrumb-container");
 
        [FindsBy(How = How.ClassName, Using = "breadcrumb-container")]
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

        // Method to switch the language based on the language code
        public void SwitchLanguage(string languageCode)
        {
            UserProfileDropDown.Click(); // Open user profile dropdown

            // Wait until the language dropdown is visible and clickable
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(LanguageDropDown));

            // Interact with the dropdown to change the language
            LanguageDropDown.Click();
            SelectElement selectLanguage = new SelectElement(LanguageDropDown);

            // Check if the languageCode exists in the dropdown options
            var availableOptions = selectLanguage.Options.Select(o => o.GetAttribute("value")).ToList();

            if (!availableOptions.Contains(languageCode))
            {
                throw new NoSuchElementException($"The language code '{languageCode}' is not available in the language dropdown.");
            }

            // If it exists, proceed with selecting the language
            selectLanguage.SelectByValue(languageCode);

            // Ensure that the DOM has refreshed after the language switch
            wait.Until(driver =>
            {
                // Wait for a specific element or some indicator that the language switch has finished
                return driver.FindElement(BreadCrumbLocator).Displayed;  // Or any reliable element indicating completion
            });
        }


    }
}
