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

        // Update the locator for the BreadCrumb to a more specific XPath
        [FindsBy(How = How.XPath, Using = "/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/app-dashboard/div[1]/div/div")]
        private IWebElement BreadCrumb;

        // Locator for the welcome message div using XPath to target the h2 element with specific text
        [FindsBy(How = How.XPath, Using = "/html/body/app-root/body/app-main-layout/app-header/div/mat-sidenav-container/mat-sidenav-content/div/app-dashboard/div[2]/div/div/h2")]
        private IWebElement WelcomeMessageDiv;


        [FindsBy(How = How.XPath, Using = "/html/body/app-root/body/app-main-layout/app-header/div/mat-toolbar/div[4]/span")]
        private IWebElement UserProfileDropDown;

        [FindsBy(How = How.XPath, Using = "//*[@id=\"mat-menu-panel-0\"]/div/button/span/div")]
        private IWebElement LogOutButton;

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
            //UserProfileDropDown.Click(); // Open user profile dropdown
            //LogOutButton.Click(); // Click the logout button


            UserProfileDropDown.Click(); // Open user profile dropdown

            // Wait until the logout button is clickable
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(LogOutButton));

            LogOutButton.Click(); // Click the logout button

        }
    }
}
