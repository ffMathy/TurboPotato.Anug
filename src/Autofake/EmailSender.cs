using System;
using System.Collections.Generic;
using System.Text;

namespace Autofake
{
    class EmailSender : IEmailSender
    {
        public void Send(string toAddress, string plainTextContent)
        {
            if(!toAddress.Contains("@"))
                throw new ArgumentException("The email is invalid.", nameof(toAddress));
        }
    }
}
