using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace PlanZajec.EdukacjaIntegration
{
    public class EdukacjaConnector
    {
        private IWebDriver driver;
        private IJavaScriptExecutor diverScriptExecutor{
            get { return (IJavaScriptExecutor)driver; }
        }
    private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        private string password;
        private string login;

        public EdukacjaConnector(string login, string password)
        {
            this.password = password;
            this.login = login;
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


        private string classOfNumericSubpagesContainer = "paging-numeric-span";

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

            //TODO: Obsługa błędów, gdy edukacja dodaje kartę "0"

            //GoForAllSubPages(); //Testing place
            BrowseZapisy();
            //GoForAllSubPages();
            //ReadPage();

            driver.FindElement(By.Name("wyloguj")).Click();
        }


        private void GoForAllSubPages()
        {
            var paggingContainers = driver.FindElements(By.ClassName(classOfNumericSubpagesContainer));
            //var numericSpans = paggingContainers[0].FindElements(By.XPath(".//*"));
            //ShowAllNumericsInPagging(numericSpans);

            //Poziom przeglądania tablel z numerycznymi paskami   
            for (int i = paggingContainers.Count- 1; i >=0; i--) //From end -> similar to deep search
            {
                //Poziom przeszukowania poszczególnych stron w tabeli numerów.
                BrowseAllNumericPageInThatPagingLevel(i);
            }
        }

        private static void ShowAllNumericsInPagging(ReadOnlyCollection<IWebElement> numericSpans)
        {
            for (int i = 1; i < numericSpans.Count; i++) //First is default
            {
                Console.WriteLine("Własnie przechodze do karty nr " + numericSpans[i].GetAttribute("value"));
            }
        }

        private void BrowseAllNumericPageInThatPagingLevel(int i)
        {
            IWebElement actualNumericSpan = GetNextPageInPaging(i, int.MinValue);
            int firstElementValue = int.Parse(actualNumericSpan.GetAttribute("value"));
            int lastValue;
            while (actualNumericSpan != null)
            {
                lastValue = int.Parse(actualNumericSpan.GetAttribute("value"));
                actualNumericSpan.Click();
                actualNumericSpan = GetNextPageInPaging(i, lastValue);
            }
        }


        public IWebElement GetNextPageInPaging(int deepPaggingContainers, int lastValue)
        {
            var paggingContainers = driver.FindElements(By.ClassName(classOfNumericSubpagesContainer));
            int webPaggingContainersCount = paggingContainers.Count;

            Assert.Less(deepPaggingContainers,webPaggingContainersCount);

            var numericSpans = paggingContainers[deepPaggingContainers].FindElements(By.XPath(".//*"));
            int index = 0;
            int actValueInSpan;
            if (numericSpans.Count == 0)
            {
                return null;
            }
            do
            {
                actValueInSpan = Int32.Parse(numericSpans[index].GetAttribute("value"));
            }
            while (actValueInSpan <= lastValue && ++index < numericSpans.Count);

            return index != numericSpans.Count ? numericSpans[index] : null;
        }

        private void BrowseZapisy()
        {
            driver.FindElement(By.Name("event_ZapisyPrzegladanieGrup")).Click();
            new SelectElement(driver.FindElement(By.Name("KryteriumFiltrowania"))).SelectByText(
                "Z planu studiów, do których słuchacz ma uprawnienia");
            driver.FindElement(By.XPath("(//a[contains(text(),'Kurs')])[3]")).Click();
        }

        private void ReadPage()
        {
            //TODO: Load to database
            string strona = driver.PageSource;
            foreach (var linia in strona.Split('\n'))
            {
                Console.WriteLine(linia);
            }
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

        /*
         Some good deleted functionals to use.
            private ReadOnlyCollection<IWebElement> FindElementsWithSpecyficInput(int inputType)
            {
                //eg.   driver.FindElement(By.XPath("//input[@value='4']")).Click();
                return driver.FindElements(By.XPath($"//input[@value='{inputType}']"));
            }

            //New solution without JS.
            private long GetWebPageInPagingCount(int whitchContainer)
            {
                string script = "return document.getElementsByClassName('" + classOfNumericSubpagesContainer + "')[" +
                                whitchContainer + "].children.length;";
                var cos = diverScriptExecutor.ExecuteScript(script);
                return (long) cos;
            }
            //return document.getElementsByClassName('KOLOROWA')[0].childNodes.length;
        */


    }
}
