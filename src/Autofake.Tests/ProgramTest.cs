using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;

namespace Autofake.Tests
{
    [TestClass]
    public class ProgramTest
    {
        [TestMethod]
        public void ProgramWritesToLogOnStartup()
        {
            var serviceProvider = TestIocHelper.GetServiceProviderForUnit<IProgram>();

            var program = serviceProvider.GetService<IProgram>();
            program.Execute();

            var fakeLogger = serviceProvider.GetService<ILogger>();
            fakeLogger.Received().Log(LogLevel.Information, "The program is starting up.");
        }

        [TestMethod]
        public void ProgramSendsEmailOnStartup()
        {
            var serviceProvider = TestIocHelper.GetServiceProviderForUnit<IProgram>();

            var program = serviceProvider.GetService<IProgram>();
            program.Execute();

            var fakeEmailSender = serviceProvider.GetService<IEmailSender>();
            fakeEmailSender.Received().Send("hello@example.com", "Hello world");
        }

        [TestMethod]
        public void ProgramLogsErrorIfEmailSenderFails()
        {
            var serviceProvider = TestIocHelper.GetServiceProviderForUnit<IProgram>();

            var fakeEmailSender = serviceProvider.GetService<IEmailSender>();
            fakeEmailSender
                .When(x => x.Send(Arg.Any<string>(), Arg.Any<string>()))
                .Throw<Exception>();

            var program = serviceProvider.GetService<IProgram>();
            program.Execute();

            var fakeLogger = serviceProvider.GetService<ILogger>();
            fakeLogger.Received().Log(LogLevel.Error, Arg.Any<string>());
        }

        [TestMethod]
        public void ProgramLogsNoErrorsWhenExecuting()
        {
            var serviceProvider = TestIocHelper.GetServiceProviderForUnit<IProgram>();

            var program = serviceProvider.GetService<IProgram>();
            program.Execute();

            var fakeLogger = serviceProvider.GetService<ILogger>();
            fakeLogger.DidNotReceive().Log(LogLevel.Error, Arg.Any<string>());
        }
    }
}
