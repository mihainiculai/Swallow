using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Swallow.Services.Email
{
    public interface IEmailSender
    {
        Task ResetPasswordAsync(string email, string fullName, string token);
        Task SendAccountDeletionEmailAsync(string email, string fullName, string token);
    }

    public class EmailSender(IOptions<EmailSettings> emailSettings, IConfiguration configuration) : IEmailSender
    {
        private readonly EmailSettings _emailSettings = emailSettings.Value;
        private readonly string _websiteUrl = configuration["WebsiteURL"] ?? "";
        private readonly string _supportEmail = configuration["SupportEmail"] ?? "";

        private async Task SendEmailAsync(string to, string subject, string template, Dictionary<string, string> replacements)
        {
            var templatePath = $"Services/Email/Layouts/{template}.html";

            var emailBody = await File.ReadAllTextAsync(templatePath);

            foreach (var replacement in replacements)
            {
                emailBody = emailBody.Replace($"{{{{{replacement.Key}}}}}", replacement.Value);
            }

            var emailTemplate = await File.ReadAllTextAsync("Services/Email/Layouts/EmailLayout.html");

            var htmlContent = emailTemplate.Replace("{{EmailTitle}}", subject)
                                           .Replace("{{EmailBody}}", emailBody)
                                           .Replace("{{year}}", DateTime.Now.Year.ToString(System.Globalization.CultureInfo.InvariantCulture));

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
            string resetPasswordLink = $"{_websiteUrl}/auth/reset-password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

            Dictionary<string, string> replacements = new()
            {
                { "FullName", fullName },
                { "ResetPasswordLink", resetPasswordLink },
                { "SupportEmail", _supportEmail },
                { "ResetPasswordUrl", resetPasswordLink }
            };

            await SendEmailAsync(email, "Reset Password", "ResetPasswordLayout", replacements);
        }

        public async Task SendAccountDeletionEmailAsync(string email, string fullName, string token)
        {
            string accountDeletionLink = $"{_websiteUrl}/delete-account?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

            Dictionary<string, string> replacements = new()
            {
                { "FullName", fullName },
                { "AccountDeletionLink", accountDeletionLink },
                { "SupportEmail", _supportEmail },
                { "AccountDeletionUrl", accountDeletionLink }
            };

            await SendEmailAsync(email, "Account Deletion", "DeleteAccountLayout", replacements);
        }
    }
}
