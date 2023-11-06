using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace Swallow.Services
{
    public class EmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(_emailSettings.From),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(to));

            using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
            {
                smtpClient.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                smtpClient.EnableSsl = _emailSettings.EnableSSL;

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }

    public class EmailSettings
    {
        public required string From { get; set; }
        public required string SmtpServer { get; set; }
        public required int Port { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required bool EnableSSL { get; set; }
    }
}
