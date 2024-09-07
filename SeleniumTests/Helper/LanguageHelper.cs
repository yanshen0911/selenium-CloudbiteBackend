using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Pages;

namespace SeleniumTests.Helpers
{
    public class LanguageHelper
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private Dashboard _dashboardPage;

        public LanguageHelper(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
            _dashboardPage = new Dashboard(_driver);
        }

        public void SwitchLanguage(string languageCode)
        {
            // Switch the language using the method from the Dashboard page object
            _dashboardPage.SwitchLanguage(languageCode);

            // Wait for the breadcrumb text to change after switching the language
            _wait.Until(ExpectedConditions.TextToBePresentInElement(_dashboardPage.BreadCrumb, "expectedTextAfterSwitch"));
        }
    }
}
