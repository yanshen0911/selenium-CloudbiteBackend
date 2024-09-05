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
            return new ChromeDriver(options);
        }
    }
}
