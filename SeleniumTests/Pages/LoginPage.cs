using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace ERPPlus.SeleniumTests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver driver;

        [FindsBy(How = How.Id, Using = "userID")]
        private IWebElement UsernameField;

        [FindsBy(How = How.Id, Using = "password")]
        private IWebElement PasswordField;


        [FindsBy(How = How.XPath, Using = "/html/body/app-root/body/app-login/mat-drawer-container/mat-drawer-content/div/div[1]/div[2]/form/div[4]/button")]
        private IWebElement LoginButton;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
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

        public void ClickLoginButton()
        {
            LoginButton.Click();
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
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            //wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".form-check-input")));

            //container-header
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("container-wrapper")));

            // Find all the radio button elements
            IList<IWebElement> serverRadioButtons = driver.FindElements(By.CssSelector(".form-check-input"));

            // Find all the corresponding labels
            IList<IWebElement> serverLabels = driver.FindElements(By.CssSelector(".form-check-label"));

            // Iterate through the labels and select the matching one
            for (int i = 0; i < serverLabels.Count; i++)
            {
                if (serverLabels[i].Text.Trim().Equals(serverText.Trim()))
                {
                    serverRadioButtons[i].Click();
                    break;
                }
            }
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
