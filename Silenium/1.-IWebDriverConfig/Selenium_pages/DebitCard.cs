using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Selenium_pages;

public class DebitCard
{
    private IWebDriver _driver;
    private WebDriverWait _wait;
    
    public DebitCard(IWebDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }
    private IWebElement LastNameField => _wait.Until(d=> d.FindElement(By.XPath("//input[@id='mat-input-1']")));
    private IWebElement FirstNameField => _wait.Until(d=> d.FindElement(By.XPath("//input[@id='mat-input-2']")));
    private IWebElement MiddleNameField => _wait.Until(d=> d.FindElement(By.XPath("//input[@id='mat-input-3']")));
    private IWebElement SexMField => _wait.Until(d=> d.FindElement(By.XPath("//div[contains(text(), 'Мужской')]")));
    private IWebElement SexFField => _wait.Until(d=> d.FindElement(By.XPath("//div[contains(text(), 'Женский')]")));
    private IWebElement BirthDateField => _wait.Until(d=> d.FindElement(By.XPath("//input[@data-mat-calendar= 'mat-datepicker-1']")));
    private IWebElement PhoneField => _wait.Until(d=> d.FindElement(By.XPath("//input[@name= 'Phone']")));
    private IWebElement CitizenshipField => _wait.Until(d=> d.FindElement(By.XPath("//mat-select[@name= 'RussianFederationResident']")));
    private ReadOnlyCollection<IWebElement> CheckBoxes => _wait.Until(d => d.FindElements(By.XPath("//span[@class= 'rui-checkbox__checkmark']")));
    private IWebElement ContinueButton => _wait.Until(d=> d.FindElement(By.XPath("//button[@rtl-automark= 'REGISTRATION_NEXT']")));

    private IWebElement Coockie =>
        _wait.Until(d => d.FindElement(By.XPath("//div[@class= 'block ng-star-inserted']")));
    
    public DebitCard FillLastName(string lastName)
    {
        LastNameField.Clear();
        LastNameField.SendKeys(lastName);
        return this;
    }

    public DebitCard FillFirstName(string firstName)
    {
        FirstNameField.Clear();
        FirstNameField.SendKeys(firstName);
        return this;
    }

    public DebitCard FillMiddleName(string middleName)
    {
        MiddleNameField.Clear();
        MiddleNameField.SendKeys(middleName);
        return this;
    }

    public DebitCard FIllSex(string sex)
    {
        if (sex == "M")
        {
            SexMField.Click();
        }

        if (sex == "F")
        {
            SexFField.Click();
        }

        return this;
    }

    public DebitCard FillBirthDate(string birthDate)
    {
        BirthDateField.Clear();
        BirthDateField.Click();
        BirthDateField.SendKeys(birthDate);
        return this;
    }

    public DebitCard FillPhoneNumber(string phoneNumber)
    {
        PhoneField.Clear();
        PhoneField.Click();
        PhoneField.SendKeys(phoneNumber);
        return this;
    }

    public DebitCard FillCitizenShip(string citizenship)
    {
        CitizenshipField.Click();
        if (citizenship == "РФ")
        {
            var resident = _driver.FindElement(By.XPath("//mat-option[contains(@id, 'RussianFederationResident') and @data-test-id= 'select-option-0']"));
            resident.Click();
        }
        else
        {
            var resident = _driver.FindElement(By.XPath("//mat-option[contains(@id, 'RussianFederationResident') and and @data-test-id= 'select-option-0']"));
            resident.Click();
        }
        return this;
    }

    public DebitCard FillCheckBoxes()
    {
        foreach (var checkBox in CheckBoxes)
        {
            checkBox.Click();
        }
        return this;
    }

    public DebitCard AcceptCoockie()
    {
        var coockieButton = Coockie.FindElement(By.XPath(".//button"));
        coockieButton.Click();
        return this;
    }

    public DebitCard ClickContinue()
    {
        ContinueButton.Click();
        return this;
    }

    public DebitCard FillForm(string lastName, string firstName, string middleName, string sex, string birthDate, string phoneNumber, string citizenship)
    {
        return AcceptCoockie()
            .FillLastName(lastName)
            .FillFirstName(firstName)
            .FillMiddleName(middleName)
            .FIllSex(sex)
            .FillBirthDate(birthDate)
            .FillPhoneNumber(phoneNumber)
            .FillCitizenShip(citizenship)
            .FillCheckBoxes()
            .ClickContinue();
    }
}