using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Selenium_pages;

public class DataVerification
{
    private IWebDriver _driver;
    private WebDriverWait _wait;
    
    public DataVerification(IWebDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }
    
    private ReadOnlyCollection<IWebElement> Fields =>
        _wait.Until(d => d.FindElements(By.XPath("//b[@class= 'confirm-section__value']")));

    private IWebElement lastNameDisplayed => Fields[0];
    private IWebElement firstNameDisplayed => Fields[1];
    private IWebElement middleNameDisplayed => Fields[2];
    private IWebElement birthDateDisplayed => Fields[3];
    private IWebElement phoneNumberDisplayed => Fields[4];
    
    public bool VerifyEnteredData(string lastName, string firstName, string middleName, string birthDate, string phoneNumber)
    {
        return lastNameDisplayed.Text.Equals(lastName) &&
               middleNameDisplayed.Text.Equals(middleName) &&
               firstNameDisplayed.Text.Equals(firstName) &&
               birthDateDisplayed.Text.Equals(birthDate) &&
               phoneNumberDisplayed.Text.Equals(phoneNumber);
    }
}