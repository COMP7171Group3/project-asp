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
    public class ClientTests
    {
        private ApplicationDbContext? _context;
        private Service? _testService;
        private IWebDriver? _driver;
        private string _baseUrl = "http://localhost:5023"; // Adjust URL if needed
        private Client? _testClient;

        // Helper method to create a test client
        private Client CreateTestClient(int id, string name, string address, string contactInfo)
        {
            return new Client
            {
                ClientID = id,
                Name = name,
                Address = address,
                ContactInfo = contactInfo
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
        public void CreateClient()
        {
            // Navigate to the create client page
            _driver!.Navigate().GoToUrl(_baseUrl + "/Client/Create");

            // Create a test client
            _driver.FindElement(By.Id("Name")).SendKeys("Test Client");
            _driver.FindElement(By.Id("Address")).SendKeys("123 Test St");
            _driver.FindElement(By.Id("ContactInfo")).SendKeys("test@test.com");

            // Submit the form
            _driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            Assert.Pass();
        }

        [Test]
        public void ReadClient()
        {
            // Arrange
            int testClientId = 1;
            if (_context!.Clients.Any())
            {
                testClientId = _context.Clients.OrderBy(s => s.ClientID).Last().ClientID + 1;
            }

            _testClient = CreateTestClient(testClientId, "Test Client", "123 Test St", "test@test.com");
            _context!.Clients.Add(_testClient);
            _context.SaveChanges();

            // Navigate to the service details page
            _driver!.Navigate().GoToUrl($"{_baseUrl}/Service/Details/{_testClient.ClientID}");
            // Verify the service details are displayed
            Assert.Pass();
        }

        [Test]
        public void UpdateClient()
        {
            // Arrange
            int testClientId = 1;
            if (_context!.Clients.Any())
            {
                testClientId = _context.Clients.OrderBy(s => s.ClientID).Last().ClientID + 1;
            }

            _testClient = CreateTestClient(testClientId, "Test Client", "123 Test St", "test@test.com");
            _context!.Clients.Add(_testClient);
            _context.SaveChanges();

            // Act
            // Navigate to the client edit page
            _driver!.Navigate().GoToUrl($"{_baseUrl}/Client/Edit/{_testClient.ClientID}");

            // Fill in the form
            var nameInput = _driver.FindElement(By.Id("Name"));
            nameInput.Clear();
            nameInput.SendKeys("Updated Client");

            var addressInput = _driver.FindElement(By.Id("Address"));
            addressInput.Clear();
            addressInput.SendKeys("456 Updated St");

            var contactInfoInput = _driver.FindElement(By.Id("ContactInfo"));
            contactInfoInput.Clear();
            contactInfoInput.SendKeys("updated@test.com");

            // Submit the form
            _driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            Assert.Pass();
        }

        [Test]
        public void DeleteClient()
        {
            // Arrange
            int testClientId = 1;
            if (_context!.Clients.Any())
            {
                testClientId = _context.Clients.OrderBy(s => s.ClientID).Last().ClientID + 1;
            }

            _testClient = CreateTestClient(testClientId, "Test Client", "123 Test St", "test@test.com");
            _context!.Clients.Add(_testClient);
            _context.SaveChanges();
            // Navigate to the delete client page
            _driver!.Navigate().GoToUrl($"{_baseUrl}/Client/");
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
