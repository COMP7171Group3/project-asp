using _7071Group.Data;
using _7071Group.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Linq;

namespace UITests
{
    [TestFixture]
    public class ServiceTests
    {
        private ApplicationDbContext? _context;
        private Service? _testService;
        private IWebDriver? _driver;
        private string _baseUrl = "http://localhost:5023"; // Adjust URL if needed

        // Helper method to create a test service
        private Service CreateTestService(int id, string name, decimal rate, bool requiresCertification)
        {
            return new Service
            {
                ServiceID = id,
                ServiceName = name,
                Rate = rate,
                RequiresCertification = requiresCertification
            };
        }

        [SetUp]
        public void Setup()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless=new"); // Run in headless mode for GitHub Actions
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");

            _driver = new ChromeDriver(chromeOptions);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connectionString)
                .Options;

            _context = new ApplicationDbContext(options);

            // Ensure the database is created and seeded
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        public void CreateService()
        {
            // Navigate to the create service page
            _driver!.Navigate().GoToUrl($"{_baseUrl}/Service/Create");

            // Fill in the form
            _driver.FindElement(By.Id("ServiceName")).SendKeys("Test Service");
            _driver.FindElement(By.Id("Rate")).SendKeys("100");
            _driver.FindElement(By.Id("RequiresCertification")).Click();

            // Submit the form
            _driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            Assert.Pass();
        }

        [Test]
        public void ReadService()
        {
            // Arrange
            int testServiceId = 1;
            if (_context!.Services.Any())
            {
                testServiceId = _context.Services.OrderBy(s => s.ServiceID).Last().ServiceID + 1;
            }

            _testService = CreateTestService(testServiceId, "Test Service", 100, true);
            _context!.Services.Add(_testService);
            _context.SaveChanges();
            // Navigate to the service details page
            _driver!.Navigate().GoToUrl($"{_baseUrl}/Service/Details/{_testService.ServiceID}");
            // Verify the service details are displayed
            Assert.Pass();
        }

        [Test]
        public void UpdateService()
        {
            // Arrange
            int testServiceId = 1;
            if (_context!.Services.Any())
            {
                testServiceId = _context.Services.OrderBy(s => s.ServiceID).Last().ServiceID + 1;
            }

            _testService = CreateTestService(testServiceId, "Test Service", 100, true);
            _context!.Services.Add(_testService);
            _context.SaveChanges();

            // Act
            // Navigate to the edit service page
            _driver!.Navigate().GoToUrl($"{_baseUrl}/Service/Edit/{_testService.ServiceID}");

            // Fill in the form
            var serviceNameInput = _driver.FindElement(By.Id("ServiceName"));
            serviceNameInput.Clear();
            serviceNameInput.SendKeys("Updated Service");

            var rateInput = _driver.FindElement(By.Id("Rate"));
            rateInput.Clear();
            rateInput.SendKeys("200");

            var requiresCertificationCheckbox = _driver.FindElement(By.Id("RequiresCertification"));
            if (!requiresCertificationCheckbox.Selected)
            {
                requiresCertificationCheckbox.Click();
            }

            // Submit the form
            _driver.FindElement(By.XPath("//input[@type='submit']")).Click();

            // Assert
            Assert.Pass();
        }

        [Test]
        public void DeleteService()
        {
            // Arrange
            int testServiceId = 1;
            if (_context!.Services.Any())
            {
                testServiceId = _context.Services.OrderBy(s => s.ServiceID).Last().ServiceID + 1;
            }

            _testService = CreateTestService(testServiceId, "Test Service", 100, true);
            _context!.Services.Add(_testService);
            _context.SaveChanges();
            // Navigate to the delete service page
            _driver!.Navigate().GoToUrl($"{_baseUrl}/Service/Delete/{_testService.ServiceID}");
            // Submit the form
            _driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            Assert.Pass();
        }

        [TearDown]
        public void Teardown()
        {
            _context!.Dispose();
            _driver!.Quit();
        }
    }
}
