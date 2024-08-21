using OpenQA.Selenium;
using SeleniumInitialize_Builder;
using System.Diagnostics;

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
            string url = @"https://ib.psbank.ru/store/products/your-cashback-new";
            _driver = _builder.WithURL(url).Build();
            Assert.That(_driver.Url, Is.EqualTo(url));
            Assert.That(_builder.StartingURL, Is.EqualTo(url));
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
    }
}