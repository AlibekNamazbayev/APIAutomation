using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;

namespace Core
{
    public class BrowserFactory
    {
        public static IWebDriver GetDriver(string browserType)
        {
            return browserType.ToLower() switch
            {
                "chrome" => new ChromeDriver(),
                "firefox" => new FirefoxDriver(),
                _ => throw new ArgumentException($"Unsupported browser: {browserType}")
            };
        }
    }
}
