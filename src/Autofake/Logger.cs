using System;
using System.Collections.Generic;
using System.Text;

namespace Autofake
{
    class Logger : ILogger
    {
        private readonly IEmailSender emailSender;

        public Logger(
            IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public void Log(
            LogLevel logLevel, 
            string message)
        {
            if(logLevel == LogLevel.Error)
            {
                emailSender.Send("bugs@example.com", "Bug: An error occured.");
            }

            Console.WriteLine(logLevel + ": " + message);
        }
    }
}
