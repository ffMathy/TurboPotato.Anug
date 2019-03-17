using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Autofake.Tests
{
    [TestClass]
    public class EmailSenderTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmailSenderThrowsIfEmailAddressIsInvalid()
        {
            var serviceProvider = TestIocHelper.GetServiceProviderForUnit<IEmailSender>();
           
            var emailSender = serviceProvider.GetService<IEmailSender>();
            emailSender.Send("invalidemail", "content");
        }

        [TestMethod]
        public void EmailSenderDoesNotThrowIfEmailIsValid()
        {
            var serviceProvider = TestIocHelper.GetServiceProviderForUnit<IEmailSender>();

            var emailSender = serviceProvider.GetService<IEmailSender>();
            emailSender.Send("hello@example.com", "content");
        }
    }
}
