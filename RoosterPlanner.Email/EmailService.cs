#nullable enable
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RoosterPlanner.Email
{
    public interface IEmailService
    {
        /// <summary>
        /// Send a  email to one receipient
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="sender"></param>
        /// <param name="attachment"></param>
        Task SendEmail(string recipient, string subject, string body, bool isBodyHtml, string? sender,
            Attachment? attachment);

        /// <summary>
        /// Send a email to multiple recipients
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="sender"></param>
        /// <param name="attachment"></param>
        Task SendEmail(IEnumerable<string> recipients, string subject, string body, bool isBodyHtml, string? sender,
            Attachment? attachment);
    }

    public class SMTPEmailService : IEmailService
    {
        /// <summary>
        /// Sender of the email
        /// </summary>
        private readonly string sender;

        /// <summary>
        /// SMTP client used to send the email
        /// </summary>
        private readonly SmtpClient smtpClient;

        //constructor
        public SMTPEmailService(SmtpClient smtpClient, string sender)
        {
            this.sender = sender;
            this.smtpClient = smtpClient;
        }

        /// <summary>
        /// Send a email to one receipient
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="sender"></param>
        /// <param name="attachment"></param>
        public async Task SendEmail(string recipient, string subject, string body, bool isBodyHtml, string? sender,
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
            if (attachment != null)
                mailMessage.Attachments.Add(attachment);

            mailMessage.To.Add(recipient);
            smtpClient.Send(mailMessage);
        }

        /// <summary>
        /// Send a email to multiple recipients
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="sender"></param>
        /// <param name="attachment"></param>
        public async Task SendEmail(IEnumerable<string> recipients, string subject, string body, bool isBodyHtml,
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
            if (attachment != null)
                mailMessage.Attachments.Add(attachment);

            foreach (string recipient in recipients)
                mailMessage.To.Add(recipient);

            smtpClient.Send(mailMessage);
        }
    }
}