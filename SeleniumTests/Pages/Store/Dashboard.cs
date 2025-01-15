using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTests.Pages.Store
{
    public class Dashboard
    {
        private readonly IWebDriver driver;

        public Dashboard(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public By BreadCrumbLocator => By.ClassName("breadcrumb-container");

        [FindsBy(How = How.ClassName, Using = "breadcrumb-container")]
        public IWebElement BreadCrumb;
    }
}
