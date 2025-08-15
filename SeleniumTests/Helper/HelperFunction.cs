using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace SeleniumTests.Helper
{
    public static class HelperFunction
    {
        // Custom Wait Methods
        public static void WaitForPageToLoad(WebDriverWait wait)
        {
            WaitForAngularToLoad(wait);
            //_wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            //_wait.Until(driver =>((IJavaScriptExecutor)driver).ExecuteScript("return window.getAllAngularTestabilities().findIndex(x => !x.isStable()) === -1"));
        }

        public static void WaitForAngularToLoad(WebDriverWait wait)
        {
            wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript(
                "return window.angular !== undefined && window.angular.getTestability(document.body).isStable()"));
        }


        public static void WaitForTableToLoad(WebDriverWait wait)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".mat-mdc-table")));
        }

        public static void WaitForBETableToLoad(WebDriverWait wait)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-business-entity/div/div[4]/div/div[1]/div/table/tbody/tr")));

        }

        public static void WaitForSTRTableToLoad(WebDriverWait wait)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-store/div/div[3]/div/div[1]/div/table/tbody/tr")));

        }

        public static void WaitForCustTableToLoad(WebDriverWait wait)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-customer/div/div[4]/div/div[1]/div/table/tbody/tr")));

        }

        public static void WaitForLogTableToLoad(WebDriverWait wait)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-store/div/div[3]/div/div[1]/div/table/tbody/tr")));

        }

        public static void WaitForUserTableToLoad(WebDriverWait wait)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/app-layout/div/div/div/div/app-content/app-user/form/div/app-user-table/div/div/div[3]/div/div[1]/div/table/tbody/tr")));

        }

        public static void WaitForElementToBeClickable(WebDriverWait wait, By locator)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        public static void WaitForUrlToContain(WebDriverWait wait, string partialUrl)
        {
            wait.Until(ExpectedConditions.UrlContains(partialUrl));
        }
        public static void WaitForElementToDisappear(WebDriverWait wait, By locator)
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
        }

        public static IWebElement WaitForElementToBeVisible(WebDriverWait wait, By locator)
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        public static void TakeScreenshot(
            IWebDriver driver,
            string folderPath = "Screenshots",
            string module = "ModuleOne",
            string feature = "FeatureOne",
            string testCase = "1")
        {
            try
            {
                // Ensure folder path has a prefix and exists
                folderPath = "screenshot_" + folderPath.Trim();
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Get the current URL and sanitize it for valid filename usage
                string currentUrl = driver.Url;
                string sanitizedUrl = SanitizeUrlForFilename(currentUrl);

                // Generate a unique filename with timestamp, module, feature, test case, and URL
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmm_ss_ffff");

                string filePath = Path.Combine(
                    folderPath,
                    $"{sanitizedUrl}.{module}.{feature}.{testCase}.{timestamp}.png"
                );

                // Take and save the screenshot
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(filePath);

                Console.WriteLine($"Screenshot saved: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to take screenshot: {ex.Message}");
                throw; // Rethrow the exception for visibility in the calling method
            }
        }

        // Helper method to sanitize URL for filename usage
        private static string SanitizeUrlForFilename(string url)
        {
            // Replace invalid filename characters with '_'
            var invalidChars = Path.GetInvalidFileNameChars();
            string sanitizedUrl = string.Join("_", url.Split(invalidChars))
                .Replace("https://", "")
                .Replace("http://", "")
                .Replace("/", "_")
                .Replace("\\", "_");

            // Truncate long sanitized URLs for brevity in filenames
            return sanitizedUrl.Length > 50 ? sanitizedUrl.Substring(0, 50) : sanitizedUrl;
        }

    }
}
