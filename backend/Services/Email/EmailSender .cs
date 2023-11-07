using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Swallow.Services.Email
{
    public class EmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly string _websiteUrl;
        private readonly string _supportEmail;
        public EmailSender(IOptions<EmailSettings> emailSettings, IConfiguration configuration)
        {
            _emailSettings = emailSettings.Value;
            _websiteUrl = configuration["WebsiteURL"] ?? "";
            _supportEmail = configuration["SupportEmail"] ?? "";
        }

        public async Task SendEmailAsync(string to, string subject, string bodyContent)
        {
            var emailTemplate = File.ReadAllText("Services/Email/EmailLayout.html");

            var htmlContent = emailTemplate.Replace("{{EmailTitle}}", subject)
                                           .Replace("{{EmailBody}}", bodyContent);

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

        public async Task ResetPasswordAsync(string email, string fullName, string token)
        {
            var resetPasswordTemplate = File.ReadAllText("Services/Email/Templates/ResetPasswordTemplate.html");

            string resetPasswordLink = $"{_websiteUrl}/auth/reset-password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

            var emailBody = resetPasswordTemplate.Replace("{{FullName}}", fullName)
                                                 .Replace("{{ResetPasswordLink}}", resetPasswordLink)
                                                 .Replace("{{SupportEmail}}", _supportEmail)
                                                 .Replace("{{ResetPasswordUrl}}", resetPasswordLink);

            var emailTemplate = File.ReadAllText("Services/Email/EmailLayout.html");

            var htmlContent = emailTemplate.Replace("{{EmailTitle}}", "Reset Password")
                                           .Replace("{{EmailBody}}", emailBody);

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(_emailSettings.From),
                Subject = "Reset Password",
                Body = htmlContent,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(email));

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
