using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using ERPPlus.SeleniumTests;

namespace ERPPlus.SeleniumTests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver driver;

        [FindsBy(How = How.Id, Using = "username")]
        private IWebElement UsernameField;

        [FindsBy(How = How.Id, Using = "password")]
        private IWebElement PasswordField;

        //var loginButton = driver.FindElement(By.CssSelector("button.btn.primaryActionBtn.imgBtn"));
        [FindsBy(How = How.CssSelector, Using = "button.btn.primaryActionBtn.imgBtn")]
        private IWebElement LoginButton;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public void EnterUsername(string username)
        {
            UsernameField.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            PasswordField.SendKeys(password);
        }

        public void ClickLoginButton()
        {
            LoginButton.Click();
        }

        public bool IsLoginSuccessful()
        {
            // Add logic to verify login, such as checking if a specific element is visible after login
            return driver.Url.Contains("/dashboard");
        }
    }
}
