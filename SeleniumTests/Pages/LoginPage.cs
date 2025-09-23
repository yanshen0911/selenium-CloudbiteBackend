using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace ERPPlus.SeleniumTests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver driver;
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        [FindsBy(How = How.CssSelector, Using = "#homePageEmail")]
        private IWebElement UsernameField;

        [FindsBy(How = How.CssSelector, Using = "#homePagePassword")]
        private IWebElement PasswordField;

        [FindsBy(How = How.CssSelector, Using = "#kt_login_password_reset_form > div.fv-row.mb-10 > input")]
        private IWebElement EmailField;

        [FindsBy(How = How.CssSelector, Using = "#loginButton")]
        private IWebElement LoginButton;

        [FindsBy(How = How.XPath, Using = "//app-login//form//a[contains(text(),'Forgot Password')]")]
        private IWebElement ForgotPassword;

        [FindsBy(How = How.CssSelector, Using = "#kt_password_reset_submit")]
        private IWebElement SubmitForgotPassword;

        public LoginPage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
            PageFactory.InitElements(_driver, this);
        }

        public void EnterUsername(string username)
        {
            UsernameField.Clear();
            UsernameField.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            PasswordField.Clear();
            PasswordField.SendKeys(password);
        }

        public void EnterEmail(string email)
        {
            EmailField.Clear();
            EmailField.SendKeys(email);
        }

        public void ClickLoginButton()
        {
            LoginButton.Click();
        }

        public void ClickForgotPassword()
        {
            ForgotPassword.Click();
        }

        public void ClickSubmitForgotPassword()
        {
            SubmitForgotPassword.Click();
        }

        public bool IsLoginButtonEnabled()
        {
            return LoginButton.Enabled;
        }

        public bool IsSubmitForgotPasswordEnabled()
        {
            return SubmitForgotPassword.Enabled;
        }


        public bool IsLoginSuccessful()
        {
            // Add logic to verify login, such as checking if a specific element is visible after login
            return driver.Url.Contains("/dashboard");
        }

        [FindsBy(How = How.CssSelector, Using = ".imgBtn.select-server-btn")]
        private IWebElement SelectServerButton;

        // Corrected OK and Cancel button selectors based on new HTML structure
        [FindsBy(How = How.CssSelector, Using = ".btn.primaryActionBtn.mr-5.imgBtn")]
        private IWebElement SelectServerOKButton;

        [FindsBy(How = How.CssSelector, Using = ".btn.secondaryActionBtn.mr-5.btn-cancel-hover.imgBtn")]
        private IWebElement SelectServerCancelButton;

        public void ClickSelectServerButton()
        {
            SelectServerButton.Click();
        }

        public void ClickSelectServerOKButton()
        {
            // Wait for the overlay to disappear and the OK button to be clickable
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".btn.primaryActionBtn.mr-5.imgBtn")));
            SelectServerOKButton.Click();
        }

        public void SelectServerByText(string serverText)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Wait for the server selection panel to be fully visible
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("mat-drawer.mat-drawer-opened")));

            // Retry logic to handle dynamic loading issues
            for (int attempt = 0; attempt < 3; attempt++)
            {
                try
                {
                    // Find all radio buttons and labels
                    IList<IWebElement> serverRadioButtons = driver.FindElements(By.CssSelector(".form-check-input"));
                    IList<IWebElement> serverLabels = driver.FindElements(By.CssSelector(".form-check-label"));

                    // Iterate through the labels to find the matching server
                    for (int i = 0; i < serverLabels.Count; i++)
                    {
                        string labelText = serverLabels[i].Text.Trim();

                        if (labelText.Equals(serverText.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"Server found: {labelText}. Clicking the radio button.");
                            serverRadioButtons[i].Click();
                            return;
                        }
                    }

                    // If not found, wait a bit and retry
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {attempt + 1} failed: {ex.Message}");
                }
            }

            throw new NoSuchElementException($"Server with text '{serverText}' was not found.");
        }


        [FindsBy(How = How.CssSelector, Using = ".invalid-feedback")]
        private IList<IWebElement> ValidationMessages;
        public IList<string> GetValidationMessages()
        {
            List<string> messages = new List<string>();

            foreach (var messageElement in ValidationMessages)
            {
                messages.Add(messageElement.Text);
            }

            return messages;
        }

    }
}
