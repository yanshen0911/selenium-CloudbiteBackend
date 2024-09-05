using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SeleniumTests.Tests
{
    public class ERPPlusUITests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(80));
        }

        //[Test]
        public void TestLogin()

        {
            driver.Navigate().GoToUrl("http://192.168.110.215/login");

            // Wait for the username and password fields
            var usernameField = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("username")));
            var passwordField = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("password")));

            // Enter credentials
            usernameField.SendKeys("admin");
            passwordField.SendKeys("password");

            var loginButton = driver.FindElement(By.CssSelector("button.btn.primaryActionBtn.imgBtn"));

            loginButton.Click();

            // Validate login success
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("/dashboard"));
            Assert.IsTrue(driver.Url.Contains("/dashboard"));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
