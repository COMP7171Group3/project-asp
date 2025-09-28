using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace UITests
{
    public class HomePageTests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // Run in headless mode for GitHub Actions
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            driver = new ChromeDriver(options);
        }

        [Test]
        public void HomePageLoads()
        {
            driver.Navigate().GoToUrl("http://localhost:5023"); // Adjust URL if needed
            Assert.That(driver.Title.Contains("Home"));
            Console.WriteLine("ALL TESTS PASSING");
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}