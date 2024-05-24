using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using PetCare.Email.Models;

namespace PetCare.Email
{
    public interface IEmailService
    {
        void SendEmail(MailData mailData, TextFormat textFormat = TextFormat.Plain);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailConfig _config;

        public EmailService(EmailConfig config)
        {
            _config = config;
        }

        public void SendEmail(MailData mailData, TextFormat textFormat = TextFormat.Plain)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_config.SenderName, _config.SenderEmail));
            email.To.Add(new MailboxAddress(mailData.Name, mailData.Email));
            email.Subject = mailData.Subject;
            email.Body = new TextPart(textFormat) { Text = mailData.Body };
            using (var client = new SmtpClient())
            {
                client.Connect(_config.Server, _config.Port, false);
                client.Authenticate(_config.Username, _config.Password);
                client.Send(email);
                client.Disconnect(true);
            }
        }
    }
}
