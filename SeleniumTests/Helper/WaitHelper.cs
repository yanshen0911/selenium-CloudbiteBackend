using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace ERPPlus.SeleniumTests.Helpers
{
    public static class WaitHelper
    {
        public static IWebElement WaitForElement(IWebDriver driver, By locator, int timeoutInSeconds = 30)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }
    }
}
