using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;

namespace UITests
{
    [TestFixture("Chrome")]
    [TestFixture("Firefox")]
    /*
    [TestFixture("IE")]
    */
    public class BaseUITest {
        
        protected string browser;
        protected IWebDriver driver;

        public BaseUITest(string browser)
        {
            this.browser = browser;
        }

        [OneTimeSetUp]
        public void Setup()
        {
            try
            {
                // The NuGet package for each browser installs driver software
                // under the bin directory, alongside the compiled test code.
                // This tells the driver class where to find the underlying driver software.
                var cwd = Environment.CurrentDirectory;

                // Create the driver for the current browser.
                switch(browser)
                {
                  case "Chrome":
                    driver = new ChromeDriver(cwd);
                    break;
                  case "Firefox":
                    driver = new FirefoxDriver(cwd);
                    break;
                  case "IE":
                    driver = new InternetExplorerDriver(cwd);
                    break;
                  default:
                    throw new ArgumentException($"'{browser}': Unknown browser");
                }

                // Wait until the page is fully loaded on every page navigation or page reload.
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

                // Navigate to the site.
                // The site name is stored in the SITE_URL environment variable to make 
                // the tests more flexible.
                string url = Environment.GetEnvironmentVariable("SITE_URL");
                driver.Navigate().GoToUrl(url + "/");

                // Wait for the page to be completely loaded.
                new WebDriverWait(driver, TimeSpan.FromSeconds(1))
                    .Until(d => ((IJavaScriptExecutor) d)
                        .ExecuteScript("return document.readyState")
                        .Equals("complete"));
            }
            catch (DriverServiceNotFoundException)
            {
            }
            catch (WebDriverException)
            {
                Cleanup();
            }
        }
    
        [OneTimeTearDown]
        public void Cleanup()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }
    }

}