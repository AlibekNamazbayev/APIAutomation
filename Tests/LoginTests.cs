using Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Core;

namespace Tests
{
    [TestClass]
    public class LoginTests : BaseTest
    {
        [TestMethod]
        public void ValidLoginTest()
        {
        
            driver!.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

     
            var loginPage = new LoginPage(driver);
            loginPage.Login("tomsmith", "SuperSecretPassword!");

    
            string flash = loginPage.GetFlashMessage();
            Assert.IsTrue(flash.Contains("You logged into a secure area"), "Login was not successful");

            LoggerManager.LogInfo("ValidLoginTest executed successfully");
        }
    }
}
