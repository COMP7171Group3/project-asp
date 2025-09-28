using System.Collections.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Chrome;

namespace UITests;

public class UserTests
{
    
    private IWebDriver _driver;
    private int _randInt = new Random().Next();
    private string _testEmail;
    private string _testPw;
    
    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless"); // Run in headless mode for GitHub Actions
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArguments("--allow-insecure-localhost");
        options.AddAdditionalOption("acceptInsecureCerts", true);
        
        _testEmail = "test" + this._randInt + "@test.test";
        _testPw = "P@$$w0rd";

        _driver = new ChromeDriver(options);
    }
    
    
    [Test, Order(1)]
    public void SignupTest()
    {
        Actions actions = new Actions(_driver);
        
        
        _driver.Navigate().GoToUrl("http://localhost:5023/Identity/Account/Register"); // Adjust URL if needed
        IWebElement emailInputField = _driver.FindElement(By.Id("Input_Email"));
        IWebElement pwInputField = _driver.FindElement(By.Id("Input_Password"));
        IWebElement pwConfInputField = _driver.FindElement(By.Id("Input_ConfirmPassword"));
        IWebElement submitBtn = _driver.FindElement(By.Id("registerSubmit"));
        
        emailInputField.SendKeys(_testEmail);
        pwInputField.SendKeys(_testPw);
        pwConfInputField.SendKeys(_testPw);
        
        Console.WriteLine(emailInputField.GetAttribute("value"));
        Console.WriteLine(pwInputField.GetAttribute("value"));
        Console.WriteLine(pwConfInputField.GetAttribute("value"));
        
        actions.Click(submitBtn).Perform();
        
        WebDriverWait wdw = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
        
        IWebElement confirmEmailLink = wdw.Until(ExpectedConditions.ElementIsVisible(By.Id("confirm-link")));
        
        actions.Click(confirmEmailLink).Perform();
        
        Assert.That(_driver.Url.Contains("localhost:5023/Identity/Account/ConfirmEmail"));
    }
    
    [Test, Order(2)]
    public void LoginTest()
    {
        Actions actions = new Actions(_driver);
        
        
        _driver.Navigate().GoToUrl("http://localhost:5023/Identity/Account/Login"); // Adjust URL if needed
        IWebElement emailInputField = _driver.FindElement(By.Id("Input_Email"));
        IWebElement pwInputField = _driver.FindElement(By.Id("Input_Password"));
        IWebElement remCheckBox = _driver.FindElement(By.Id("Input_RememberMe"));
        IWebElement submitBtn = _driver.FindElement(By.Id("login-submit"));
        
        
        emailInputField.SendKeys(_testEmail);
        pwInputField.SendKeys(_testPw);
        
        Console.WriteLine(emailInputField.GetAttribute("value"));
        Console.WriteLine(pwInputField.GetAttribute("value"));
        
        actions.Click(remCheckBox).Perform();
        
        actions.Click(submitBtn).Perform();
        
        WebDriverWait wdw = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
        
        bool success = wdw.Until(ExpectedConditions.UrlMatches("http://localhost:5023/"));
        
        Console.WriteLine(_driver.Url);
        
        Assert.That(success);
    }
    
    [Test, Order(3)]
    public void DeleteAccountTest()
    {
        Actions actions = new Actions(_driver);
        
        _driver.Navigate().GoToUrl("http://localhost:5023/Identity/Account/Login"); // Adjust URL if needed
        IWebElement emailInputField = _driver.FindElement(By.Id("Input_Email"));
        IWebElement pwInputField = _driver.FindElement(By.Id("Input_Password"));
        IWebElement remCheckBox = _driver.FindElement(By.Id("Input_RememberMe"));
        IWebElement submitBtn = _driver.FindElement(By.Id("login-submit"));
        
        emailInputField.SendKeys(_testEmail);
        pwInputField.SendKeys(_testPw);
        
        Console.WriteLine(emailInputField.GetAttribute("value"));
        Console.WriteLine(pwInputField.GetAttribute("value"));
        
        actions.Click(remCheckBox).Perform();
        actions.Click(submitBtn).Perform();
        
        WebDriverWait wdw = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
        
        bool loginSuccess = wdw.Until(ExpectedConditions.UrlMatches("http://localhost:5023/"));

        if (!loginSuccess)
        {
            Assert.Fail("Something is wrong, could not log in.");
            return;
        }
        
        IWebElement userNavLink = null;
        try
        {
            userNavLink = new WebDriverWait(_driver, TimeSpan.FromSeconds(20)).Until(driver =>
            {
                try
                {
                    // Re-query the nav items on each poll
                    var navItems = driver.FindElements(By.ClassName("nav-item"));
                    foreach (var navItem in navItems)
                    {
                        // Immediately get a fresh reference for the child element
                        var navLink = navItem.FindElement(By.ClassName("nav-link"));
                        if (navLink.Text.Contains(_testEmail))
                        {
                            return navLink;
                        }
                    }
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    // Return null so the wait will continue polling
                    return null;
                }
            });
        }
        catch (WebDriverTimeoutException)
        {
            Assert.Fail("User nav link not found after waiting 20 seconds.");
        }

        actions.Click(userNavLink).Perform();
        
        WebDriverWait wdw3 = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
        IWebElement personalDataBtn = wdw3.Until(ExpectedConditions.ElementIsVisible(By.Id("personal-data")));
        
        actions.Click(personalDataBtn).Perform();

        IWebElement deleteBtn = _driver.FindElement(By.Id("delete"));
        
        actions.Click(deleteBtn).Perform();
        
        WebDriverWait wdw4 = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
        IWebElement confirmDeleteForm = wdw4.Until(ExpectedConditions.ElementIsVisible(By.Id("delete-user")));

        IWebElement deletePw = _driver.FindElement(By.Id("Input_Password"));
        deletePw.SendKeys(_testPw);

        IWebElement deleteConfBtn = confirmDeleteForm.FindElement(By.TagName("button"));
        
        actions.Click(deleteConfBtn).Perform();
        
        bool deleteSuccess = wdw.Until(ExpectedConditions.UrlMatches("http://localhost:5023/"));
        
        Assert.That(deleteSuccess);
    }

    [TearDown]
    public void Teardown()
    {
        _driver.Quit();
    }
    
}