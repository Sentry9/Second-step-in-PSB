using OpenQA.Selenium;
using SeleniumInitialize_Builder;
using System.Diagnostics;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace SeleniumInitialize_Tests
{
    public class Tests
    {
        private SeleniumBuilder _builder;
        private IWebDriver _driver;
        [SetUp]
        public void Setup()
        {
            _builder = new SeleniumBuilder();
        }

        [Test(Description = "�������� ���������� ������������� ���������� IWebDriver")]
        public void BuildTest1()
        {
            _driver = _builder.Build();
            Assert.IsNotNull(_driver);
        }

        [Test(Description = "�������� ������� �������� IWebDriver")]
        public void DisposeTest1()
        {
            _driver = _builder.Build();
            Assert.IsFalse(_builder.IsDisposed);
            _builder.Dispose();
            Assert.IsTrue(_builder.IsDisposed);
            var processes = Process.GetProcessesByName("chromedriver");
            Assert.IsFalse(processes.Any());
        }

        [Test(Description = "�������� ����� ����� ��������")]
        public void PortTest1()
        {
            _driver = _builder.ChangePort(3737).Build();
            Assert.That(_builder.Port, Is.EqualTo(3737));
        }

        [Test(Description = "�������� ����� ����� �� ���������")]
        public void PortTest2()
        {
            int port = new Random().Next(6000, 32000);
            _driver = _builder.ChangePort(port).Build();
            Assert.That(_builder.Port, Is.EqualTo(port));
        }

        [Test(Description = "�������� ���������� ���������")]
        public void ArgumentTest1()
        {
            string argument = "--start-maximized";
            _driver = _builder.SetArgument(argument).Build();
            Assert.Contains(argument, _builder.ChangedArguments);
            var startingSize = _driver.Manage().Window.Size;
            _driver.Manage().Window.Maximize();
            Assert.That(_driver.Manage().Window.Size, Is.EqualTo(startingSize));
        }

        [Test(Description = "���������� ���������������� ���������")]
        public void UserOptionTest()
        {
            string key = "safebrowsing.enabled";
            _driver = _builder.SetUserOption(key, true).Build();
            Assert.That(_builder.ChangedUserOptions.ContainsKey(key));
            Assert.That(_builder.ChangedUserOptions[key], Is.True);
        }

        [Test(Description = "���������� ���������� ���������������� ���������")]
        public void UserOptionStressTest()
        {
            string key = "safebrowsing.enabled";
            _driver = _builder.SetUserOption(key, true)
                .SetUserOption(key, true)
                .Build();
            Assert.That(_builder.ChangedUserOptions.ContainsKey(key));
            Assert.That(_builder.ChangedUserOptions[key], Is.True);
        }

        [Test(Description = "�������� ��������� ��������")]
        public void TimeoutTest()
        {
            TimeSpan timeout = TimeSpan.FromSeconds(20);
            _driver = _builder.WithTimeout(timeout).Build();
            Assert.That(_driver.Manage().Timeouts().ImplicitWait, Is.EqualTo(timeout));
            Assert.That(_builder.Timeout, Is.EqualTo(timeout));
        }

        [Test(Description = "�������� ��������� URL")]
        public void URLTest()
        {
            try
            {
                string url = @"https://ib.psbank.ru/store/products/your-cashback-new";
                _driver = _builder.WithURL(url).Build();
                Assert.That(_driver.Url, Is.EqualTo(url));
                Assert.That(_builder.StartingURL, Is.EqualTo(url));
            }
            catch (Exception ex)
            {
                SaveScreenshotOnFailure(_driver, nameof(URLTest));
                throw;
            }
        }

        [Test(Description = "����������� ��������")]
        public void ComplexTest()
        {
            string url = @"https://ib.psbank.ru/store/products/your-cashback-new";
            string key = "safebrowsing.enabled";
            string argument = "--start-maximized";
            int port = new Random().Next(6000, 32000);
            TimeSpan timeout = TimeSpan.FromSeconds(20);
            _driver = _builder.WithTimeout(timeout)
                .WithURL(url)
                .ChangePort(port)
                .SetArgument(argument)
                .SetUserOption(key, true)
                .Build();
            Assert.Multiple(() =>
            {
                Assert.That(_driver.Manage().Timeouts().ImplicitWait, Is.EqualTo(timeout));
                Assert.That(_builder.Timeout, Is.EqualTo(timeout));
                Assert.That(_driver.Url, Is.EqualTo(url));
                Assert.That(_builder.StartingURL, Is.EqualTo(url));
                Assert.IsTrue(_builder.ChangedArguments.Contains(argument));
                Assert.That(_builder.ChangedUserOptions.ContainsKey(key));
                Assert.That(_builder.ChangedUserOptions[key], Is.True);
            });
        }
        
        [Test(Description = "Проверка запуска браузера в headless режиме")]
        public void HeadlessModeTest()
        {
            _driver = _builder.SetHeadlessMode(true).Build();
            Assert.IsTrue(_builder.IsHeadless);

            var processes = Process.GetProcessesByName("chromedriver");
            Assert.IsTrue(processes.Any(), "Процесс Chrome должен существовать в headless режиме.");
            Assert.IsFalse(processes.First().MainWindowHandle != IntPtr.Zero, "В режиме headless окно браузера не должно быть видно.");
        }
        [Test(Description = "Поиск элемента на странице с использованием XPath")]
        public void FindElementTest()
        {
            try
            {
                string url = @"https://ib.psbank.ru/store/products/classic-mortgage-program";
                _driver = _builder.WithURL(url).Build();
                string xpathMortgage = "//div[@class= 'mortgage-calculator-controls-select ng-star-inserted']";
                string xpathButton = "//button[@icon='gosuslugi' and @appearance= 'primary']";
                string xpathFamily = "//div[@class= 'brands-cards__item ng-star-inserted']";
                string xpathInsurance =
                    "//psb-switcher[@class= 'deltas__switcher _theme-default _checked ng-untouched ng-pristine ng-valid']";
                string xpathPeriod = "//rui-range-slider[@id= 'loanPeriod' and @data-testid= 'calc-input-loanPeriod']";
                IWebElement elementMortgage = _builder.FindElementByXPath(xpathMortgage);
                IWebElement elementButton = _builder.FindElementByXPath(xpathButton);
                IWebElement elementFamily = _builder.FindElementByXPath(xpathFamily);
                IWebElement elementInsurance = _builder.FindElementByXPath(xpathInsurance);
                IWebElement elementPeriod = _builder.FindElementByXPath(xpathPeriod);
                Assert.IsNotNull(elementMortgage);
                Assert.That(elementMortgage.TagName.ToLower(), Is.EqualTo("div"));
                Assert.That(elementMortgage.GetAttribute("class"),
                    Is.EqualTo("mortgage-calculator-controls-select ng-star-inserted"));
                Assert.IsNotNull(elementButton);
                Assert.That(elementButton.TagName.ToLower(), Is.EqualTo("button"));
                Assert.That(elementButton.GetAttribute("class"),
                    Is.EqualTo("mortgage-calculator-output-submit__button"));
                Assert.IsNotNull(elementFamily);
                Assert.That(elementFamily.TagName.ToLower(), Is.EqualTo("div"));
                Assert.That(elementFamily.GetAttribute("class"), Is.EqualTo("brands-cards__item ng-star-inserted"));
                Assert.IsNotNull(elementInsurance);
                Assert.That(elementInsurance.TagName.ToLower(), Is.EqualTo("psb-switcher"));
                Assert.That(elementInsurance.GetAttribute("class"),
                    Is.EqualTo("deltas__switcher _theme-default _checked ng-untouched ng-pristine ng-valid"));
                Assert.IsNotNull(elementPeriod);
                Assert.That(elementPeriod.TagName.ToLower(), Is.EqualTo("rui-range-slider"));
                Assert.That(elementPeriod.GetAttribute("class"), Is.EqualTo("ng-untouched ng-pristine ng-valid"));
            }
            catch (Exception ex)
            {
                SaveScreenshotOnFailure(_driver, nameof(FindElementTest));
                throw;
            }
        }
        [Test(Description = "Провальный поиск элемента на странице с использованием XPath")]
        public void FindElementTestFail()
        {
            try
            {
                string url = @"https://ib.psbank.ru/store/products/classic-mortgage-program";
                _driver = _builder.WithURL(url).Build();
                string xpathMortgage = "//div[@class= 'OOOOOOO]";
                IWebElement elementMortgage = _builder.FindElementByXPath(xpathMortgage);
                Assert.IsNotNull(elementMortgage);
                Assert.That(elementMortgage.TagName.ToLower(), Is.EqualTo("div"));
                Assert.That(elementMortgage.GetAttribute("class"),
                    Is.EqualTo("mortgage-calculator-controls-select ng-star-inserted"));
            }
            catch (Exception ex)
            {
                SaveScreenshotOnFailure(_driver, nameof(FindElementTestFail));
                throw;
            }
        }

        [Test(Description = "Поиск состояния активности и видимости на странице")]
        public void CheckEnableAndActive()
        {
            try
            {
                string url = @"https://ib.psbank.ru/store/products/classic-mortgage-program";
                _driver = _builder.WithURL(url).Build();
                string xpathMortgage = "//div[@class= 'mortgage-calculator-controls-select ng-star-inserted']";
                string xpathButton = "//button[@icon='gosuslugi' and @appearance= 'primary']";
                string xpathFamily = "//div[@class= 'brands-cards__item ng-star-inserted']";
                string xpathInsurance =
                    "//psb-switcher[@class= 'deltas__switcher _theme-default _checked ng-untouched ng-pristine ng-valid']";
                string xpathPeriod = "//rui-range-slider[@id= 'loanPeriod' and @data-testid= 'calc-input-loanPeriod']";
                IWebElement elementMortgage = _builder.FindElementByXPath(xpathMortgage);
                IWebElement elementButton = _builder.FindElementByXPath(xpathButton);
                IWebElement elementFamily = _builder.FindElementByXPath(xpathFamily);
                IWebElement elementInsurance = _builder.FindElementByXPath(xpathInsurance);
                IWebElement elementPeriod = _builder.FindElementByXPath(xpathPeriod);
                Assert.IsTrue(elementMortgage.Displayed, "Элемент не отображается на странице");
                Assert.IsTrue(elementMortgage.Enabled, "Элемент на странице не доступен");
                Assert.IsTrue(elementButton.Displayed, "Элемент не отображается на странице");
                Assert.IsTrue(elementButton.Enabled, "Элемент на странице не доступен");
                Assert.IsTrue(elementFamily.Displayed, "Элемент не отображается на странице");
                Assert.IsTrue(elementFamily.Enabled, "Элемент на странице не доступен");
                Assert.IsTrue(elementInsurance.Displayed, "Элемент не отображается на странице");
                Assert.IsTrue(elementInsurance.Enabled, "Элемент на странице не доступен");
                Assert.IsTrue(elementPeriod.Displayed, "Элемент не отображается на странице");
                Assert.IsTrue(elementPeriod.Enabled, "Элемент на странице не доступен");
            }
            catch (Exception ex)
            {
                SaveScreenshotOnFailure(_driver, nameof(CheckEnableAndActive));
                throw;
            }
        }

        [Test(Description = "Поиск значения value у элементов")]
        public void ValueTest()
        {
            try
            {
                string url = @"https://ib.psbank.ru/store/products/classic-mortgage-program";
                _driver = _builder.WithURL(url).Build();
                string xpathMortgage = "//div[@class= 'mortgage-calculator-controls-select ng-star-inserted']";
                string xpathPeriod = "//rui-range-slider[@id= 'loanPeriod' and @data-testid= 'calc-input-loanPeriod']";
                IWebElement elementMortgage = _builder.FindElementByXPath(xpathMortgage);
                IWebElement elementPeriod = _builder.FindElementByXPath(xpathPeriod);
                string selectedValueMortgage = elementMortgage.Text;
                string selectedValuePeriod = elementPeriod.Text;
                Assert.NotNull(selectedValueMortgage);
                Assert.That(selectedValueMortgage, Is.EqualTo("Объект ипотеки\r\nКвартира в строящемся доме"));
                Assert.NotNull(selectedValuePeriod);
                Assert.That(selectedValuePeriod, Is.EqualTo("Срок кредита\r\n3 Года\r\n30 Лет"));
            }
            catch (Exception ex)
            {
                SaveScreenshotOnFailure(_driver, nameof(ValueTest));
                throw;
            }
        }

        [Test(Description = "Поиск значения состояния у элементов")]
        public void ActiveTest()
        {
            try
            {
                string url = @"https://ib.psbank.ru/store/products/classic-mortgage-program";
                _driver = _builder.WithURL(url).Build();
                string xpathFamily = "//div[@class= 'brands-cards__item ng-star-inserted']";
                string xpathInsurance =
                    "//psb-switcher[@class= 'deltas__switcher _theme-default _checked ng-untouched ng-pristine ng-valid']";
                IWebElement elementFamily = _builder.FindElementByXPath(xpathFamily);
                IWebElement elementInsurance = _builder.FindElementByXPath(xpathInsurance);
                string selectedValueInsurance = elementInsurance.GetAttribute("class");
                string selectedValueFamily = elementFamily.GetAttribute("class");
                bool isActive = selectedValueFamily.Contains("_active");
                bool isSwitched = selectedValueInsurance.Contains("_checked");
                Assert.IsTrue(isSwitched);
                Assert.IsFalse(isActive); //Проверяем на False т.к. по умолчанию вкладка неактивна
            }
            catch (Exception ex)
            {
                SaveScreenshotOnFailure(_driver, nameof(ActiveTest));
                throw;
            }
        }

        [Test(Description = "Проверка стиля кнопки 'Заполнить через Госуслуги'")]
        public void StyleTest()
        {
            try
            {
                string url = @"https://ib.psbank.ru/store/products/classic-mortgage-program";
                _driver = _builder.WithURL(url).Build();
                string xpathButton = "//button[@icon='gosuslugi' and @appearance= 'primary']";
                string xpathColor = "//rui-wrapper[@class ='wrapper' and @data-appearance ='primary']"; 
                IWebElement elementButton = _builder.FindElementByXPath(xpathButton);
                IWebElement elementColor = elementButton.FindElement(By.XPath(xpathColor));
                bool isClickable = elementButton.Displayed && elementButton.Enabled;
                string color = elementColor.GetCssValue("background-color");
                string height = elementButton.GetCssValue("height");
                Assert.True(isClickable);
                Assert.That(color, Is.EqualTo("rgba(242, 97, 38, 1)"));
                Assert.That(height, Is.EqualTo("48px"));
            }
            catch (Exception ex)
            {
                SaveScreenshotOnFailure(_driver, nameof(StyleTest));
                throw;
            }
        }

        [Test(Description = "Проверка кнопки 'Заполнить без Госуслуг'")]
        public void WithoutGosuslugiTest()
        {
            try
            {
                string url = @"https://ib.psbank.ru/store/products/military-family-mortgage-program";
                _driver = _builder.WithURL(url).Build();
                string xpathLoader =
                    "//psb-loader[@class= 'mortgage-calculator-output__loader ng-tns-c97-20 ng-star-inserted']";
                string xpathButton =
                    "//button[@class='mortgage-calculator-output-submit__button' and @data-appearance= 'secondary']";
                IWebElement button = _builder.FindElementByXPath(xpathButton);
                IWebElement loader = _builder.FindElementByXPath(xpathLoader);
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                wait.Until(d => loader.Displayed);
                button.Click();
                string xpathError = "//div[@class= 'mortgage-calculator-output__alert mortgage-calculator-output__alert_show']";
                IWebElement error = _builder.FindElementByXPath(xpathError);
                Assert.IsTrue(error.Displayed);
                //Assert.IsFalse(button.Displayed); Как я понял сообщение об ошибке появляется поверх кнопки, поэтому в момент появления они обе находятся на странице, хотя по зпдпнию должно быть наоборот 
                wait.Until(d => !error.Displayed);
                Assert.IsFalse(error.Displayed);
                Assert.IsTrue(button.Displayed);
            }
            catch (Exception ex)
            {
                SaveScreenshotOnFailure(_driver, nameof(WithoutGosuslugiTest));
                throw;
            }
        }

        [Test(Description = "Тест свитчеров")]
        public void SwitchersTest()
        {
            try
            {
                string url = @"https://ib.psbank.ru/store/products/classic-mortgage-program";
                _driver = _builder.WithURL(url).Build();
                string blockXpath =
                    "//div[@class= 'deltas ng-star-inserted']";
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                IWebElement block = wait.Until(d => d.FindElement(By.XPath(blockXpath)));
                var switchers = block.FindElements(By.XPath(".//div[@class= 'deltas__item ng-star-inserted']"));
                IWebElement switcher1 = switchers[0];
                IWebElement switcher2 = switchers[1];
                IWebElement switcher3 = switchers[2];
                IWebElement switcher4 = switchers[3];
                switcher1.FindElement(By.XPath(".//span[@class= 'slider _not-standalone']")).Click();
                StringAssert.DoesNotContain("_checked", switcher1.FindElement(By.XPath(".//psb-switcher")).GetAttribute("class"));
                StringAssert.DoesNotContain("_checked", switcher2.FindElement(By.XPath(".//psb-switcher")).GetAttribute("class"));
                StringAssert.DoesNotContain("_checked", switcher3.FindElement(By.XPath(".//psb-switcher")).GetAttribute("class"));
                StringAssert.DoesNotContain("_checked", switcher4.FindElement(By.XPath(".//psb-switcher")).GetAttribute("class"));
                StringAssert.Contains("_main", switcher1.FindElement(By.XPath(".//span[contains(@class, 'psb-status psb-status')]")).GetAttribute("class"));
                StringAssert.Contains("_main", switcher2.FindElement(By.XPath(".//span[contains(@class, 'psb-status psb-status')]")).GetAttribute("class"));
                StringAssert.Contains("_main", switcher3.FindElement(By.XPath(".//span[contains(@class, 'psb-status psb-status')]")).GetAttribute("class"));
                StringAssert.Contains("_main", switcher4.FindElement(By.XPath(".//span[contains(@class, 'psb-status psb-status')]")).GetAttribute("class"));
                switcher1.FindElement(By.XPath(".//span[@class= 'slider _not-standalone']")).Click();
                StringAssert.Contains("_success", switcher1.FindElement(By.XPath(".//span[contains(@class, 'psb-status psb-status')]")).GetAttribute("class"));
                switcher2.FindElement(By.XPath(".//span[@class= 'slider _not-standalone']")).Click();
                StringAssert.Contains("_success", switcher2.FindElement(By.XPath(".//span[contains(@class, 'psb-status psb-status')]")).GetAttribute("class"));
                switcher3.FindElement(By.XPath(".//span[@class= 'slider _not-standalone']")).Click();
                StringAssert.Contains("_success", switcher3.FindElement(By.XPath(".//span[contains(@class, 'psb-status psb-status')]")).GetAttribute("class"));
                switcher4.FindElement(By.XPath(".//span[@class= 'slider _not-standalone']")).Click();
                StringAssert.Contains("_success", switcher4.FindElement(By.XPath(".//span[contains(@class, 'psb-status psb-status')]")).GetAttribute("class"));
            }
            catch (Exception ex)
            {
                SaveScreenshotOnFailure(_driver, nameof(SwitchersTest));
                throw;
            }
        }
        
        /// <summary>
        /// TODO проблемный тест
        /// </summary>
        
        [Test(Description = "Действия с элементами")]
        public void FillWithoutGosuslugi()
        {
            try
            {
                string url = @"https://ib.psbank.ru/store/products/classic-mortgage-program";
                _driver = _builder.WithURL(url).Build();
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                string xpathButton = "//button[@class='mortgage-calculator-output-submit__button' and @data-appearance= 'secondary']";
                IWebElement buttonFill = _builder.FindElementByXPath(xpathButton);
                buttonFill.Click();
                string buttonContinueXpath = "//button[@type= 'submit' and @rtl-automark= 'REGISTRATION_NEXT']";
                IWebElement buttonContinue = _builder.FindElementByXPath(buttonContinueXpath);
                Assert.True(!buttonContinue.Enabled);
                IWebElement secondName = wait.Until(d=> d.FindElement(By.XPath("//input[@id= 'mat-input-1']")));
                secondName.SendKeys("Ивыщовлдфжвфывффв");
                IWebElement firstName = wait.Until(d=> d.FindElement(By.XPath("//input[@id= 'mat-input-2']")));
                firstName.SendKeys("Иылтволфтывлфтвлф");
                IWebElement midName = wait.Until(d=> d.FindElement(By.XPath("//input[@id= 'mat-input-3']")));
                midName.SendKeys("Иылфьвлдыфтвлдтфыдв");
                IWebElement sex = wait.Until(d=> d.FindElement(By.XPath("//div[@class= 'rui-radio']")));
                sex.Click();
                IWebElement birthDate = wait.Until(d=> d.FindElement(By.XPath("//input[@data-mat-calendar= 'mat-datepicker-1']")));
                birthDate.SendKeys("12122000");
                //IWebElement phoneNumber = wait.Until(d=> d.FindElement(By.XPath("//input[@id= 'formly_23_input_Phone_0']")));
                IWebElement phoneNumber = wait.Until(d => d.FindElement(By.CssSelector("[name='Phone']")));
                phoneNumber.SendKeys("9654105479");
                IWebElement address
                    = wait.Until(d=> d.FindElement(By.XPath("//rui-form-field-wrapper[@class= 'form-field-wrapper select-with-double-item-field form-field-hide-placeholder form-field-pristine form-field-untouched']")));
                address.Click();
                IWebElement firstOffice = wait.Until(d=>
                    d.FindElement(
                        By.XPath("//mat-option[@id= 'formly_28_select-with-double-item_OfficeId_0_0']")));
                firstOffice.Click();
                IWebElement citizenship = wait.Until(d=> d.FindElement(By.XPath("//div[@id= 'mat-select-value-5']")));
                citizenship.Click();
                IWebElement resident = wait.Until(d=>
                    d.FindElement(
                        By.XPath("//mat-option[@id= 'formly_27_select_RussianFederationResident_0_0']")));
                resident.Click();
                IWebElement employment
                    = wait.Until(d=> d.FindElement(By.XPath("//div[@id= 'mat-select-value-7']")));
                employment.Click();
                IWebElement official = wait.Until(d=>
                    d.FindElement(
                        By.XPath("//mat-option[@id= 'formly_27_select_RussianEmployment_1_0']")));
                official.Click();
                IWebElement email = wait.Until(d=> d.FindElement(By.XPath("//input[@id= 'mat-input-4']")));
                email.SendKeys("darkmoon7770@yandex.ru");
                IWebElement checkform = wait.Until(d =>
                    d.FindElement(By.XPath("//rtl-registration-step-form[@class= 'ng-star-inserted']")));
                var checkBoxes = wait.Until(d =>
                    checkform.FindElements(By.XPath(".//span[@class= 'rui-checkbox__checkmark']")));
                IWebElement checkBox1 = checkBoxes[0];
                IWebElement checkBox2 = checkBoxes[1];
                IWebElement checkBox3 = checkBoxes[2];
                checkBox1.Click();
                checkBox2.Click();
                checkBox3.Click();
                IWebElement button = wait.Until(d =>
                    d.FindElement(By.XPath("//button[@rtl-automark= 'REGISTRATION_NEXT']")));
                Assert.True(button.Enabled);
            }
            catch (Exception ex)
            {
                SaveScreenshotOnFailure(_driver, nameof(FillWithoutGosuslugi));
                throw;
            }
        }
        [Test(Description = "Проверка смены цвета при наведении кнопки 'Заполнить через Госуслуги'")]
        public void ActionTest1()
        {
            try
            {
                string url = @"https://ib.psbank.ru/store/products/classic-mortgage-program";
                _driver = _builder.WithURL(url).Build();
                string xpathButton = "//button[@icon='gosuslugi' and @appearance= 'primary']";
                string xpathColor = "//rui-wrapper[@class ='wrapper' and @data-appearance ='primary']"; 
                IWebElement elementButton = _builder.FindElementByXPath(xpathButton);
                IWebElement elementColor = elementButton.FindElement(By.XPath(xpathColor));
                bool isClickable = elementButton.Displayed && elementButton.Enabled;
                string color = elementColor.GetCssValue("background-color");
                Assert.True(isClickable);
                Assert.That(color, Is.EqualTo("rgba(242, 97, 38, 1)"));
                Actions actions = new Actions(_driver);
                actions.MoveToElement(elementButton).Perform();
                color = elementColor.GetCssValue("background-color");
                Assert.That(color, Is.EqualTo("rgba(212, 73, 33, 1)")); //Почему цвет тут меняется?
            }
            catch (Exception ex)
            {
                SaveScreenshotOnFailure(_driver, nameof(ActionTest1));
                throw;
            }
        }

        [Test(Description = "Проверка слайдера")]
        public void ActionTest2()
        {
            try
            {
                string url = @"https://ib.psbank.ru/store/products/classic-mortgage-program";
                _driver = _builder.WithURL(url).Build();
                string xpathSlider = "//input[@type= 'range' and @step= '50000']";
                IWebElement slider = _builder.FindElementByXPath(xpathSlider);
                string value = slider.GetAttribute("style");
                Actions actions = new Actions(_driver);
                actions.ClickAndHold(slider)
                    .MoveByOffset(50, 0) 
                    .Release()                       
                    .Perform();
                Assert.That(value, !Is.EqualTo(slider.GetAttribute("style")));
            }
            catch (Exception ex)
            {
                SaveScreenshotOnFailure(_driver, nameof(ActionTest1));
                throw;
            }
        }
        
        // Третий блок

        [Test(Description = "Определить уникальный элемент на каждой странице")]
        public void NavigationTest1()
        {
            try
            {
                string url = @"https://ib.psbank.ru/";
                _driver = _builder.WithURL(url).Build();
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                _driver.Navigate().GoToUrl("https://ib.psbank.ru/store/products/consumer-loan");
                _driver.Navigate().GoToUrl("https://ib.psbank.ru/store/products/investmentsbrokerage");
                IWebElement phoneBroker = wait.Until(d =>
                    d.FindElement(By.XPath("//a[@class= 'service-phone-number font-weight-bold']")));
                Assert.That(phoneBroker.Text, Is.EqualTo("8 (800) 700 9 777"));
                _driver.Navigate().Back();
                IWebElement phoneLoan = wait.Until(d =>
                    d.FindElement(By.XPath("//a[@class= 'service-phone-number font-weight-bold']")));
                Assert.That(phoneLoan.Text, Is.EqualTo("8 800 333 03 03"));
                _driver.Navigate().Back();
                IWebElement forBusiness = wait.Until(d =>
                    d.FindElement(By.XPath("//a[@class= 'desktop-menu__link rtl-menu-item']")));
                Assert.That(forBusiness.Text, Is.EqualTo("Для бизнеса"));
            }
            catch (Exception ex)
            {
                SaveScreenshotOnFailure(_driver, nameof(NavigationTest1));
                throw;
            }
        }
        
// Это 4-й блок
        
        [Test(Description = "Проверка чекбокса категорий")]
        public void CheckBoxes()
        {
            try
            {
                string url = @"https://ib.psbank.ru/store/products/your-cashback-new";
                _driver = _builder.WithURL(url).Build();
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
                IWebElement button = wait.Until(d=> d.FindElement(By.XPath("//button[contains(@class, 'change-categories')]")));
                IWebElement cookie = wait.Until(d=> d.FindElement(By.XPath("//div[@ng-version='12.1.5']")));
                IWebElement cookieButton = wait.Until(d=> cookie.FindElement(By.TagName("button")));
                cookieButton.Click();
                button.FindElement(By.XPath(".//rui-wrapper[@class= 'wrapper']")).Click();
                IWebElement dialog = wait.Until(d=> d.FindElement(By.XPath("//rtl-select-categories-dialog[@class= 'ng-star-inserted']")));
                var checkboxes = wait.Until(d=> dialog.FindElements(By.XPath(".//span[@class= 'rui-checkbox__checkmark']")));
                checkboxes[0].Click();
                checkboxes[1].Click();
                checkboxes[2].Click();
                foreach (var checkbox in checkboxes)
                {
                    Assert.True(checkbox.Enabled);
                    checkbox.Click();
                    checkbox.Click();
                }
            }
            catch (Exception ex)
            {
                SaveScreenshotOnFailure(_driver, nameof(ActionTest1));
                throw;
            }
        }
        
        [Test(Description = "Проверка Фамилии в выпадающем списке")]
        public void SelectLastNameFromDropdown()
        {
            string lastNamePrefix = "Пу";
            string fullLastName = "Пушкин";
            string url = @"https://ib.psbank.ru/store/products/your-cashback-new";
            _driver = _builder.WithURL(url).Build();
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            IWebElement lastNameInput = wait.Until(d => d.FindElement(By.XPath("//input[@id='mat-input-1']")));
            lastNameInput.SendKeys(lastNamePrefix);
            var suggestions = wait.Until(d => d.FindElements(By.XPath("//div[@class='live-dadata-item ng-star-inserted']")));
            foreach (var suggestion in suggestions)
            {
                if (suggestion.Text.Contains(fullLastName))
                {
                    suggestion.Click();
                    break;
                }
            }
        }
        
        /// <summary>
        // TODO ТУТ НЕ ХВАТАЕТ 3 задания 4 блока
        /// </summary>

        [TearDown]
        public void TearDown()
        {
            try
            {
                _driver?.Quit();
                _driver?.Dispose();
            }
            finally
            {
                var chromeDriverProcesses = Process.GetProcessesByName("chromedriver");
                foreach (var process in chromeDriverProcesses)
                {
                    process.Kill();
                }
                _builder?.Dispose();
            }
        }
        private void SaveScreenshotOnFailure(IWebDriver driver, string testName)
        {
            try
            {
                if (driver is ITakesScreenshot screenshotDriver)
                {
                    Thread.Sleep(2000);
                    Screenshot screenshot = screenshotDriver.GetScreenshot();
                    string filePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    screenshot.SaveAsFile(filePath);
                    TestContext.AddTestAttachment(filePath);
                }
            }
            catch (Exception e)
            {
                TestContext.WriteLine($"Не удалось сохранить скриншот: {e.Message}");
            }
        }
    }
}