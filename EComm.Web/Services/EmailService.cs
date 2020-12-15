using EComm.Web.Interfaces;
using System;

namespace EComm.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailFormatter _formatter;

        public EmailService(IEmailFormatter formatter)
            => _formatter = formatter;

        public bool SendEmail(string email, string body)
        {
            Console.WriteLine($"Sending email to {email} with body '{_formatter.Format(body)}'!");
            return true;
        }
    }
}
