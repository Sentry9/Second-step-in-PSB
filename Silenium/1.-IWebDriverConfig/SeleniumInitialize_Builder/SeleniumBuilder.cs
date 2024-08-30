using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V119.DOMSnapshot;
using OpenQA.Selenium.Support.UI;

namespace SeleniumInitialize_Builder
{
    public class SeleniumBuilder : IDisposable
    {
        private IWebDriver WebDriver { get; set; }
        private WebDriverWait _wait;
        public int Port { get; private set; }
        public bool IsDisposed { get; private set; }
        public bool IsHeadless { get; private set; }
        public List<string> ChangedArguments { get; private set; } = new List<string>();
        public Dictionary<string, object> ChangedUserOptions { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public string StartingURL { get; private set; }

        private ChromeDriverService _service;
        private ChromeOptions _options;

        public SeleniumBuilder()
        {
            _options = new ChromeOptions();
            ChangedUserOptions = new Dictionary<string, object>();
        }
        
        public IWebDriver Build()
        {
            if (_service == null)
            {
                WebDriver = new ChromeDriver(_options);
            }
            else
            {
                WebDriver = new ChromeDriver(_service, _options);
            }
            _wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(10));
            if (Timeout != TimeSpan.Zero)
            {
                WebDriver.Manage().Timeouts().ImplicitWait = Timeout;
            }
            if (!string.IsNullOrEmpty(StartingURL))
            {
                WebDriver.Navigate().GoToUrl(StartingURL);
            }
            return WebDriver;
        }

        public void Dispose()
        {
            WebDriver?.Quit();
            WebDriver = null;
            var chromeDriverProcesses = Process.GetProcessesByName("chromedriver");
            foreach (var process in chromeDriverProcesses)
            {
                process.Kill();
            }

            IsDisposed = true;
        }
        
        public SeleniumBuilder ChangePort(int port)
        {
            _service = ChromeDriverService.CreateDefaultService();
            _service.Port = port;
            Port = port;
            return this;
        }

        public SeleniumBuilder SetArgument(string argument)
        {
            _options.AddArgument(argument);
            ChangedArguments.Add(argument);
            return this;
        }

        public SeleniumBuilder SetUserOption(string option, object value) 
        {
            _options.AddUserProfilePreference(option, value);
            ChangedUserOptions[option] = value;
            return this;
        }
        public SeleniumBuilder SetHeadlessMode(bool headless)
        {
            if (headless)
            {
                _options.AddArgument("--headless");
            }
            IsHeadless = headless;
            return this;
        }
        
        public SeleniumBuilder WithTimeout(TimeSpan timeout)
        {
            Timeout = timeout;
            return this;
        }

        public SeleniumBuilder WithURL(string url)
        {
            StartingURL = url;
            return this;
        }

        public IWebElement FindElementByXPath(string xpath)
        {
            if (WebDriver == null)
            {
                throw new InvalidOperationException("WebDriver не инициализирован. Сначала вызовите Build().");
            }

            return _wait.Until(driver => driver.FindElement(By.XPath(xpath)));;
        }
    }
}