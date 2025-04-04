using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.IO;

namespace Tests
{
    [TestClass]
    public class BaseTest
    {

        protected IWebDriver? driver;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
            driver = BrowserFactory.GetDriver("chrome");
            LoggerManager.LogInfo("Browser initialized");
        }

        [TestCleanup]
        public void TearDown()
        {
            if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed && driver != null)
            {
                try
                {
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    string screenshotFile = Path.Combine(Directory.GetCurrentDirectory(),
                        $"screenshot_{TestContext.TestName}_{timestamp}.png");

                    File.WriteAllBytes(screenshotFile, screenshot.AsByteArray);
                    LoggerManager.LogError($"Test {TestContext.TestName} failed. Screenshot saved to {screenshotFile}");
                }
                catch (Exception ex)
                {
                    LoggerManager.LogError($"Error capturing screenshot: {ex.Message}");
                }
            }
            driver?.Quit();
            LoggerManager.LogInfo("Browser closed");
        }
    }
}
