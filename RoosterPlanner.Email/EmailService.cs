#nullable enable
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using RoosterPlanner.Common.Config;

namespace RoosterPlanner.Email
{
    public interface IEmailService
    {
        void SendEmail(string recipient, string subject, string body, bool isBodyHtml,string? sender);
        void SendEmail(IEnumerable<string> recipients, string subject, string body, bool isBodyHtml, string? sender);
    }

    public class SMTPEmailService:IEmailService
    {
        private readonly EmailConfig emailConfig;
        private readonly SmtpClient smtpClient;
        public SMTPEmailService(IOptions<EmailConfig> emailConfig)
        {
            this.emailConfig = emailConfig.Value;
            smtpClient = new SmtpClient(this.emailConfig.SMTPadres)
            {
                Port = this.emailConfig.Port,
                Credentials = new NetworkCredential(this.emailConfig.Emailadres, this.emailConfig.Password),
                EnableSsl = this.emailConfig.EnableSsl
            };
        }

        public void SendEmail(string recipient, string subject, string body, bool isBodyHtml, string? sender)
        {
            sender ??= emailConfig.Emailadres;
            var mailMessage = new MailMessage
            {
                From = new MailAddress(sender),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };
            mailMessage.To.Add(recipient);
            smtpClient.Send(mailMessage);
          
        }

        public void SendEmail(IEnumerable<string> recipients, string subject, string body, bool isBodyHtml, string? sender)
        {
            sender ??= emailConfig.Emailadres;
            var mailMessage = new MailMessage
            {
                From = new MailAddress(sender),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };
            
            foreach (string recipient in recipients)
                mailMessage.To.Add(recipient);

            smtpClient.Send(mailMessage);
        }
    }
}