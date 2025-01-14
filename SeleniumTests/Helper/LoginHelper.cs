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

        public void PerformLogin(string username, string password, bool isNavigateToLogin = true)
        {
            if (isNavigateToLogin)
            {
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login");
            }

            // Wait for the login button to be visible
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app-root/body/app-login/mat-drawer-container/mat-drawer-content/div/div[1]/div[2]/form/div[4]/button")));

            // Perform login actions
            _loginPage.EnterUsername(username);
            _loginPage.EnterPassword(password);
            _loginPage.ClickLoginButton();

            // Wait for the dashboard URL to confirm successful login
            _wait.Until(ExpectedConditions.UrlContains("/dashboard"));
        }


        public bool IsLoggedIn()
        {
            return _driver.Url.Contains("/dashboard");
        }

        public IList<string> GetValidationMessages()
        {
            return _loginPage.GetValidationMessages();
        }

        public void SelectServer(string serverName, bool isNavigateToLogin = true) 
        {
            // Navigate to login page
            if (isNavigateToLogin)
            {
                _driver.Navigate().GoToUrl(AppConfig.BaseUrl + "/login");
            }

            // Wait for login form and login button to be visible
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app-root/body/app-login/mat-drawer-container/mat-drawer-content/div/div[1]/div[2]/form/div[4]/button")));

            //Console.WriteLine(driver.PageSource);  // Prints the entire page's HTML to the console

            //// Or inspect the element directly
            //Console.WriteLine(LoginButton.Text);  // Print the text of the button (if accessible)

            //var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            //IWebElement loginButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("button.btn.primaryActionBtn.imgBtn")));


            _loginPage.ClickSelectServerButton();
            _loginPage.SelectServerByText(serverName);
            _loginPage.ClickSelectServerOKButton();
        }
    }
}
