using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Autofake.Tests
{
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void LoggerSendsEmailWhenLogLevelIsError()
        {
            var serviceProvider = TestIocHelper.GetServiceProviderForUnit<ILogger>();

            var logger = serviceProvider.GetService<ILogger>();
            logger.Log(LogLevel.Error, "An error occured.");

            var fakeEmailSender = serviceProvider.GetService<IEmailSender>();
            fakeEmailSender.Received().Send("bugs@example.com", "Bug: An error occured.");
        }

        [TestMethod]
        public void LoggerDoesNotSendEmailWhenLogLevelIsWarning()
        {
            var serviceProvider = TestIocHelper.GetServiceProviderForUnit<ILogger>();

            var logger = serviceProvider.GetService<ILogger>();
            logger.Log(LogLevel.Warning, "This is a warning.");

            var fakeEmailSender = serviceProvider.GetService<IEmailSender>();
            fakeEmailSender.DidNotReceive().Send(Arg.Any<string>(), Arg.Any<string>());
        }

        [TestMethod]
        public void LoggerDoesNotSendEmailWhenLogLevelIsInformation()
        {
            var serviceProvider = TestIocHelper.GetServiceProviderForUnit<ILogger>();

            var logger = serviceProvider.GetService<ILogger>();
            logger.Log(LogLevel.Information, "This is some information.");

            var fakeEmailSender = serviceProvider.GetService<IEmailSender>();
            fakeEmailSender.DidNotReceive().Send(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
