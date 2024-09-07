using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;
using ERPPlus.SeleniumTests.Config;
using ERPPlus.SeleniumTests.Pages;

namespace SeleniumTests.Helpers
{
    public class LoginHelper
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private LoginPage _loginPage;

        public LoginHelper(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
            _loginPage = new LoginPage(_driver);
        }

        public void PerformLogin(string username, string password)
        {
            // Navigate to login page
            _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login");

            // Wait for login form to be visible
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button.btn.primaryActionBtn.imgBtn")));

            // Perform login actions
            _loginPage.EnterUsername(username);
            _loginPage.EnterPassword(password);
            _loginPage.ClickLoginButton();

            // Wait for the dashboard URL
            _wait.Until(ExpectedConditions.UrlContains("/dashboard"));
        }

        public bool IsLoggedIn()
        {
            return _driver.Url.Contains("/dashboard");
        }
    }
}
