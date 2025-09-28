using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace UITests
{
    public class NavLinkTests
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
        public void AllNavLinksWork()
        {
            driver.Navigate().GoToUrl("http://localhost:5023"); // Adjust URL if needed

            // Collect all navigation links
            var navLinks = driver.FindElements(By.CssSelector(".navbar-nav .nav-link:not(.dropdown-toggle)"));
            var dropdownLinks = driver.FindElements(By.CssSelector(".dropdown-menu .dropdown-item"));

            // Combine all links into a single list
            var allLinks = new List<IWebElement>();
            allLinks.AddRange(navLinks);
            allLinks.AddRange(dropdownLinks);

            for (int i = 0; i < allLinks.Count; i++)
            {
                var link = allLinks[i];

                // Get the link text
                var linkText = link.Text;

                // Click the link
                link.Click();

                // Assert that the title contains the link text
                Assert.That(driver.Title.Contains(linkText));

                // Navigate back to the home page
                driver.Navigate().Back();

                // Re-find the dropdown and navigation links to avoid stale element exceptions
                navLinks = driver.FindElements(By.CssSelector(".navbar-nav .nav-link:not(.dropdown-toggle)"));
                dropdownLinks = driver.FindElements(By.CssSelector(".dropdown-menu .dropdown-item"));

                allLinks = new List<IWebElement>();
                allLinks.AddRange(navLinks);
                allLinks.AddRange(dropdownLinks);
            }
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}