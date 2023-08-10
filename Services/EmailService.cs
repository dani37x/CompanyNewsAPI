using CompanyNewsAPI.Interfaces;
using System.Net.Mail;
using System.Net;

namespace CompanyNewsAPI.Services
{
    public class EmailService
    {
        private readonly string _email;
        private readonly string _password;
        public EmailService(string email, string password)
        {
            _email = email;
            _password = password;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_email, _password)
            };

            return client.SendMailAsync(
                new MailMessage(from: _email, to: email, subject, message));
        }
    }
}
