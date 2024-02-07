using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Swallow.Services.Email
{
    public class EmailSender(IOptions<EmailSettings> emailSettings, IConfiguration configuration)
    {
        private readonly EmailSettings _emailSettings = emailSettings.Value;
        private readonly string _websiteUrl = configuration["WebsiteURL"] ?? "";
        private readonly string _supportEmail = configuration["SupportEmail"] ?? "";

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

            using var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port);
            smtpClient.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
            smtpClient.EnableSsl = _emailSettings.EnableSSL;

            await smtpClient.SendMailAsync(mailMessage);
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

            using var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port);
            smtpClient.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
            smtpClient.EnableSsl = _emailSettings.EnableSSL;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }

    
}
