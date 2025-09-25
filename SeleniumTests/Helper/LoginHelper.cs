using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;
using CloudbiteBackend.SeleniumTests.Config;
using CloudbiteBackend.SeleniumTests.Pages;

namespace SeleniumTests.Helpers
{
    public class LoginHelper
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private LoginPage _loginPage;

        private string _welcomeScreenPath = "/management/dashboard/sales-db";
        public LoginHelper(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
            _loginPage = new LoginPage(driver, wait); // ✅ Must match
        }

        public void PerformLogin(string username, string password, bool isNavigateToLogin = true)
        {
            if (isNavigateToLogin)
            {
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/auth/login");
            }


            // Perform login actions
            _loginPage.EnterUsername(username);
            _loginPage.EnterPassword(password);
            _loginPage.ClickLoginButton();

            // Wait for the dashboard URL to confirm successful login
            _wait.Until(ExpectedConditions.UrlContains(_welcomeScreenPath));
        }


        public bool IsLoggedIn()
        {
            return _driver.Url.Contains(_welcomeScreenPath);
        }

        public IList<string> GetValidationMessages()
        {
            return _loginPage.GetValidationMessages();
        }

    }
}
