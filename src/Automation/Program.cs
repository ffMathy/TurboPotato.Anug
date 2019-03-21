using FluffySpoon.Automation.Web;
using FluffySpoon.Automation.Web.JQuery;
using FluffySpoon.Automation.Web.Puppeteer;
using FluffySpoon.Automation.Web.Selenium;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Opera;
using PuppeteerSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Automation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddJQueryDomSelector();

            serviceCollection.AddPuppeteerWebAutomationFrameworkInstance(GetPuppeteerDriverAsync);

            serviceCollection.AddSeleniumWebAutomationFrameworkInstance(GetFirefoxDriverAsync);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var automationEngine = serviceProvider.GetRequiredService<IWebAutomationEngine>();
            using (automationEngine)
            {
                await automationEngine.InitializeAsync();

                await automationEngine.Open("https://google.com");

                await automationEngine
                    .Enter("this is a very long test that works").In("input[type=text]:visible")
                    .Wait(until =>
                        until.Exists("input[name=btnK][type=submit]:visible"));

                var elements = await automationEngine
                    .Click.On("input[name=btnK][type=submit]:visible:first")
                    .Wait(until =>
                        until.Exists("#rso .g:visible"))
                    .Expect
                    .Count(10).Of("#rso .g:visible");

                await automationEngine
                    .TakeScreenshot
                    .Of(elements)
                    .SaveAs((framework, i, element) =>
                        "screenshot_" + framework.UserAgentName + "_" + i + "_" + element.TagName + ".jpg");

                Console.WriteLine("Test done!");
            }
        }

        private static async Task<Browser> GetPuppeteerDriverAsync()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            return await PuppeteerSharp.Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
                DefaultViewport = new ViewPortOptions()
                {
                    Width = 1100,
                    Height = 700
                }
            });
        }

        public static Task<IWebDriver> GetChromeDriverAsync()
        {
            var service = ChromeDriverService.CreateDefaultService(
                Path.Combine(
                    Environment.CurrentDirectory,
                    "Drivers"));

            service.EnableVerboseLogging = false;
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            service.HostName = "127.0.0.1";

            var options = new ChromeOptions()
            {
                Proxy = null,
                UnhandledPromptBehavior = UnhandledPromptBehavior.Accept,
                AcceptInsecureCertificates = true
            };

            var chromeDriver = new ChromeDriver(
                service,
                options);
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;

            return Task.FromResult<IWebDriver>(chromeDriver);
        }

        public static Task<IWebDriver> GetEdgeDriverAsync()
        {
            var options = new EdgeOptions()
            {
                PageLoadStrategy = PageLoadStrategy.Eager,
                UnhandledPromptBehavior = UnhandledPromptBehavior.Accept,
                UseInPrivateBrowsing = true
            };

            var pathsToCheck = new[]
            {
                Path.Combine("C:", "Windows", "SysWOW64"),
                Path.Combine(
                    Environment.CurrentDirectory,
                    "Drivers")
            };

            var driverFileName = "MicrosoftWebDriver.exe";
            var pathToUse = pathsToCheck
                .First(x => File.Exists(
                    Path.Combine(x, driverFileName)));

            var service = EdgeDriverService.CreateDefaultService(
                pathToUse,
                driverFileName,
                52296);

            var driver = new EdgeDriver(service, options);
            return Task.FromResult<IWebDriver>(driver);
        }

        private static Task<IWebDriver> GetOperaDriverAsync()
        {
            var options = new OperaOptions()
            {
                AcceptInsecureCertificates = true,
                UnhandledPromptBehavior = UnhandledPromptBehavior.Accept
            };

            var service = OperaDriverService.CreateDefaultService(
                Path.Combine(
                    Environment.CurrentDirectory,
                    "Drivers"),
                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),
                    "Programs",
                    "Opera",
                    "launcher.exe"));

            service.HostName = "127.0.0.1";

            var driver = new OperaDriver(
                service,
                options);
            return Task.FromResult<IWebDriver>(driver);
        }

        private static Task<IWebDriver> GetFirefoxDriverAsync()
        {
            var options = new FirefoxOptions()
            {
                PageLoadStrategy = PageLoadStrategy.Eager,
                AcceptInsecureCertificates = true,
                UnhandledPromptBehavior = UnhandledPromptBehavior.Accept
            };

            var service = FirefoxDriverService.CreateDefaultService(
                Path.Combine(
                    Environment.CurrentDirectory,
                    "Drivers"));

            service.Host = "127.0.0.1";
            service.HostName = "127.0.0.1";

            var driver = new FirefoxDriver(
                service,
                options);
            return Task.FromResult<IWebDriver>(driver);
        }
    }
}
