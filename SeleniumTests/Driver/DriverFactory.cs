using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ERPPlus.SeleniumTests.Drivers
{
    public static class DriverFactory
    {
        public static IWebDriver CreateDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            // please make sure you have the EXACT chrome driver version downloaded and extracted
            // this one is manual downlaod at https://developer.chrome.com/docs/chromedriver/downloads
            // you need to map it with your current chrome version, and need to change from time to time
            
            string _ChromeBrowserPath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            string _ChromeDriverPath = @"D:\114.0.5735.90\chromedriver_win32";

            options.BinaryLocation = _ChromeBrowserPath; 
            IWebDriver driver = new ChromeDriver(_ChromeDriverPath, options);
            return driver;
        }
    }
}
