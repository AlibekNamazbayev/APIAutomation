using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Business
{
    public class LoginPage
    {
        private readonly IWebDriver driver;
        private readonly By usernameField = By.Id("username");
        private readonly By passwordField = By.Id("password");
        private readonly By loginButton = By.CssSelector("button.radius");
        private readonly By flashMessage = By.Id("flash");

        public LoginPage(IWebDriver driver) => this.driver = driver;

        public void Login(string username, string password)
        {
            driver.FindElement(usernameField).Clear();
            driver.FindElement(usernameField).SendKeys(username);
            driver.FindElement(passwordField).Clear();
            driver.FindElement(passwordField).SendKeys(password);
            driver.FindElement(loginButton).Click();
        }

        public string GetFlashMessage()
        {

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var element = wait.Until(drv => drv.FindElement(flashMessage));
            return element.Text;
        }
    }
}
