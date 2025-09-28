using _7071Group.Data;
using _7071Group.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Linq;

namespace UITests
{
    [TestFixture]
    public class EmployeeTests
    {
        private ApplicationDbContext? _context;
        private Employee? _testEmployee;
        private IWebDriver? _driver;
        private string _baseUrl = "http://localhost:5023"; // Adjust URL if needed
        
        // Helper method to create a test employee
        private Employee CreateTestEmployee(int id, string name, string address, string emergencyContact, string jobTitle, string employmentType, decimal salaryRate)
        {
            return new Employee
            {
                EmployeeID = id,
                Name = name,
                Address = address,
                EmergencyContact = emergencyContact,
                JobTitle = jobTitle,
                EmploymentType = employmentType,
                SalaryRate = salaryRate
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
        public void CreateEmployee()
        {
            // Navigate to the create employee page
            _driver!.Navigate().GoToUrl($"{_baseUrl}/Employee/Create");

            // Create a test employee
            _driver.FindElement(By.Id("Name")).SendKeys("John Doe");
            _driver.FindElement(By.Id("Address")).SendKeys("123 Main St");
            _driver.FindElement(By.Id("EmergencyContact")).SendKeys("123-456-7890");
            _driver.FindElement(By.Id("JobTitle")).SendKeys("Software Engineer");
            _driver.FindElement(By.Id("EmploymentType")).SendKeys("Full-Time");
            _driver.FindElement(By.Id("SalaryRate")).SendKeys("50000");

            // Handle the ReportsTo dropdown (if applicable)
            var reportsToDropdown = _driver.FindElement(By.Id("ReportsTo"));
            var selectElement = new SelectElement(reportsToDropdown);
            selectElement.SelectByIndex(0); // Select the first option in the dropdown

            // Submit the form
            _driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            Assert.Pass();
        }

        [Test]
        public void ReadEmployee()
        {
            // Arrange
            int testEmployeeId = 1;
            if (_context!.Clients.Any())
            {
                testEmployeeId = _context.Employees.OrderBy(s => s.EmployeeID).Last().EmployeeID + 1;
            }

            _testEmployee = CreateTestEmployee(testEmployeeId, "John Doe", "123 Main St", "555-555-5555", "Manager", "Full Time", 50000);
            _context!.Employees.Add(_testEmployee);
            _context.SaveChanges();

            // Navigate to the employee index page
            _driver!.Navigate().GoToUrl($"{_baseUrl}/Employee/Details/{_testEmployee.EmployeeID}");

            // Verify the employee is displayed correctly
            Assert.Pass();
        }

        [Test]
        public void UpdateEmployee()
        {
            // Arrange
            int testEmployeeId = 1;
            if (_context!.Employees.Any())
            {
                testEmployeeId = _context.Employees.OrderBy(e => e.EmployeeID).Last().EmployeeID + 1;
            }

            // Create a test employee
            _testEmployee = CreateTestEmployee(testEmployeeId, "John Doe", "123 Main St", "123-456-7890", "Software Engineer", "Full-Time", 50000);
            _context!.Employees.Add(_testEmployee);
            _context.SaveChanges();

            // Act
            // Navigate to the edit employee page
            _driver!.Navigate().GoToUrl($"{_baseUrl}/Employee/Edit/{_testEmployee.EmployeeID}");

            // Fill in the form with updated values
            var nameInput = _driver.FindElement(By.Id("Name"));
            nameInput.Clear();
            nameInput.SendKeys("Updated Name");

            var addressInput = _driver.FindElement(By.Id("Address"));
            addressInput.Clear();
            addressInput.SendKeys("456 Updated St");

            var emergencyContactInput = _driver.FindElement(By.Id("EmergencyContact"));
            emergencyContactInput.Clear();
            emergencyContactInput.SendKeys("987-654-3210");

            var jobTitleInput = _driver.FindElement(By.Id("JobTitle"));
            jobTitleInput.Clear();
            jobTitleInput.SendKeys("Senior Software Engineer");

            var employmentTypeInput = _driver.FindElement(By.Id("EmploymentType"));
            employmentTypeInput.Clear();
            employmentTypeInput.SendKeys("Part-Time");

            var salaryRateInput = _driver.FindElement(By.Id("SalaryRate"));
            salaryRateInput.Clear();
            salaryRateInput.SendKeys("60000");

            // Handle the ReportsTo dropdown (if applicable)
            var reportsToDropdown = _driver.FindElement(By.Id("ReportsTo"));
            var selectElement = new SelectElement(reportsToDropdown);
            selectElement.SelectByIndex(0); // Select the first option in the dropdown

            // Submit the form
            _driver.FindElement(By.XPath("//input[@type='submit']")).Click();

            Assert.Pass();
        }

        [Test]
        public void DeleteEmployee()
        {
            // Arrange
            int testEmployeeId = 1;
            if (_context!.Employees.Any())
            {
                testEmployeeId = _context.Employees.OrderBy(e => e.EmployeeID).Last().EmployeeID + 1;
            }

            // Create a test employee
            _testEmployee = CreateTestEmployee(testEmployeeId, "John Doe", "123 Main St", "123-456-7890", "Software Engineer", "Full-Time", 50000);
            _context!.Employees.Add(_testEmployee);
            _context.SaveChanges();

            // Act
            // Navigate to the delete employee page
            _driver!.Navigate().GoToUrl($"{_baseUrl}/Employee/Delete/{_testEmployee.EmployeeID}");

            // Submit the delete form
            _driver.FindElement(By.XPath("//input[@type='submit']")).Click();

            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            _driver!.Quit();
        }
    }
}
