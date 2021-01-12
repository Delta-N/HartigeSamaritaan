#nullable enable
using System.Collections.Generic;
using System.Net.Mail;

namespace RoosterPlanner.Email
{
    public interface IEmailService
    {
        void SendEmail(string recipient, string subject, string body, bool isBodyHtml, string? sender,Attachment? attachment);
        void SendEmail(IEnumerable<string> recipients, string subject, string body, bool isBodyHtml, string? sender, Attachment? attachment);
    }

    public class SMTPEmailService : IEmailService
    {
        private readonly string sender;
        private readonly SmtpClient smtpClient;

        public SMTPEmailService(SmtpClient smtpClient, string sender)
        {
            this.sender = sender;
            this.smtpClient = smtpClient;
        }

        public void SendEmail(string recipient, string subject, string body, bool isBodyHtml, string? sender,
            Attachment? attachment)
        {
            sender ??= this.sender;
            var mailMessage = new MailMessage
            {
                From = new MailAddress(sender),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };
            if(attachment!=null)
                mailMessage.Attachments.Add(attachment);

            mailMessage.To.Add(recipient);
            smtpClient.Send(mailMessage);
        }

        public void SendEmail(IEnumerable<string> recipients, string subject, string body, bool isBodyHtml,
            string? sender, Attachment? attachment)
        {
            sender ??= this.sender;
            var mailMessage = new MailMessage
            {
                From = new MailAddress(sender),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };
            if(attachment!=null)
                mailMessage.Attachments.Add(attachment);

            foreach (string recipient in recipients)
                mailMessage.To.Add(recipient);

            smtpClient.Send(mailMessage);
        }
    }
}