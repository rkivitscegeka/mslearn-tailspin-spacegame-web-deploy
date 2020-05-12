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
    public class HomePageTest : BaseUITest
    {
        public HomePageTest(string browser) : base(browser)
        {      
        }

        // Download game
        [TestCase("download-btn", "pretend-modal")]
        // Screen image
        [TestCase("screen-01", "screen-modal")]
        // // Top player on the leaderboard
        [TestCase("profile-1", "profile-modal-1")]
        public void ClickLinkById_ShouldDisplayModalById(string linkId, string modalId)
        {
            // Skip the test if the driver could not be loaded.
            // This happens when the underlying browser is not installed.
            if (driver == null)
            {
                Assert.Ignore();
                return;
            }

            // Locate the link by its ID and then click the link.
            ClickElement(FindElement(By.Id(linkId)));

            // Locate the resulting modal.
            IWebElement modal = FindElement(By.Id(modalId));

            // Record whether the modal was successfully displayed.
            bool modalWasDisplayed = (modal != null && modal.Displayed);

            // Close the modal if it was displayed.
            if (modalWasDisplayed)
            {
                // Click the close button that's part of the modal.
                ClickElement(FindElement(By.ClassName("close"), modal));
                
                // Wait for the modal to close and for the main page to again be clickable.
                FindElement(By.TagName("body"));
            }

            // Assert that the modal was displayed successfully.
            // If it wasn't, this test will be recorded as failed.
            Assert.That(modalWasDisplayed, Is.True);
        }

        private IWebElement FindElement(By locator, IWebElement parent = null, int timeoutSeconds = 3)
        {
            // WebDriverWait enables us to wait for the specified condition to be true
            // within a given time period.
            return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds))
                .Until(c => {
                    IWebElement element = null;
                    // If a parent was provided, find its child element.
                    if (parent != null)
                    {
                        element = parent.FindElement(locator);
                    }
                    // Otherwise, locate the element from the root of the DOM.
                    else
                    {
                        element = driver.FindElement(locator);
                    }
                    // Return true once the element is displayed and able to receive user input.
                    return (element != null && element.Displayed && element.Enabled) ? element : null;
                });
        }

        private void ClickElement(IWebElement element)
        {
            // We expect the driver to implement IJavaScriptExecutor.
            // IJavaScriptExecutor enables us to execute JavaScript code during the tests.
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;

            // Through JavaScript, run the click() method on the underlying HTML object.
            js.ExecuteScript("arguments[0].click();", element);
        }
    }
}