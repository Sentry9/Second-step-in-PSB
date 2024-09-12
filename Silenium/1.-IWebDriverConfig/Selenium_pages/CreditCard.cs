using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Selenium_pages;

public class CreditCard : DebitCard
{
    private IWebDriver _driver;
    private WebDriverWait _wait;
    public CreditCard(IWebDriver driver) : base(driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }
    private IWebElement employmentStatusField => _wait.Until(d=> d.FindElement(By.XPath("//div[@id= 'mat-select-value-5']"))); 
    
    public CreditCard FillEmploymentStatus(string employmentStatus)
    {
        employmentStatusField.Click();
        if (employmentStatus == "Y")
        {
            var status = _driver.FindElement(By.XPath("//mat-option[@id= 'formly_25_select_RussianEmployment_1_0']"));
            status.Click();
        }
        else
        {
            var status = _driver.FindElement(By.XPath("//mat-option[@id= 'formly_25_select_RussianEmployment_1_1']"));
            status.Click();
        }
        return this;
    }
    
    public CreditCard FillConsumerLoanForm(string lastName, string firstName, string middleName, string sex, string birthDate, string phoneNumber, string citizenship, string employmentStatus)
    {
        return (CreditCard)FillEmploymentStatus(employmentStatus)
            .FillLastName(lastName)
            .FillFirstName(firstName)
            .FillMiddleName(middleName)
            .FIllSex(sex)
            .FillBirthDate(birthDate)
            .FillCitizenShip(citizenship)
            .FillPhoneNumber(phoneNumber)
            .FillCheckBoxes()
            .ClickContinue();
    }
}