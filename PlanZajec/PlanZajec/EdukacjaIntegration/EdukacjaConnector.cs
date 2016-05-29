using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace PlanZajec.EdukacjaIntegration
{
    class EdukacjaConnector
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        //TODO
        private string password;
        private string login;

        public EdukacjaConnector()
        {
            //TODO pobieranie lokalne informacji o loginie i haśle.
        }

        [SetUp]
        public void SetupTest()
        {
            driver = new FirefoxDriver();
            baseURL = "https://edukacja.pwr.wroc.pl";
            verificationErrors = new StringBuilder();
        }

        public void Run()
        {
            SetupTest();
            TheTextowyTest();
            TeardownTest();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void TheTextowyTest()
        {
            driver.Navigate().GoToUrl(baseURL + "/EdukacjaWeb/studia.do");
            Assert.AreEqual("", driver.FindElement(By.CssSelector("td.LOGOWANIE_GRAF")).Text);
            driver.FindElement(By.Name("login")).Clear();
            driver.FindElement(By.Name("login")).SendKeys(login);
            driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.CssSelector("input.BUTTON_ZALOGUJ")).Click();
            Assert.AreEqual("", driver.FindElement(By.CssSelector("td.ZALOGOWANY_GRAF")).Text);
            driver.FindElement(By.LinkText("Zapisy")).Click();

            driver.FindElement(By.Name("event_ZapisyPrzegladanieGrup")).Click();
            new SelectElement(driver.FindElement(By.Name("KryteriumFiltrowania"))).SelectByText("Z planu studiów, do których słuchacz ma uprawnienia");
            driver.FindElement(By.XPath("(//a[contains(text(),'Kurs')])[3]")).Click();


            string strona = driver.PageSource;
            foreach (var linia in strona.Split('\n'))
            {
                Console.WriteLine(linia);
            }

            driver.FindElement(By.Name("wyloguj")).Click();
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}
