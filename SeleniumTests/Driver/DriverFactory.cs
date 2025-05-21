using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace ERPPlus.SeleniumTests.Drivers
{
    public static class DriverFactory
    {
        public static IWebDriver CreateDriver()
        {
            // Automatically download and use the correct ChromeDriver version
            new DriverManager().SetUpDriver(new ChromeConfig());

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            // No need to specify BinaryLocation unless using a custom Chrome build
            return new ChromeDriver(options);
        }
    }
}
