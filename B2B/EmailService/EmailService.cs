using System.Net;
using System.Net.Mail;
using B2B.Models;
using Microsoft.Extensions.Options;

namespace B2B.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient())
            {
                client.Host = _emailSettings.SmtpServer;
                client.Port = _emailSettings.Port;
                client.TargetName =  _emailSettings.SmtpServer;
                client.EnableSsl = _emailSettings.EnableSsl;

                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(
                    _emailSettings.SenderEmail,
                    _emailSettings.SenderPassword
                );
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                var message = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                message.To.Add(toEmail);

                await client.SendMailAsync(message);
            }
        }
    }
}
