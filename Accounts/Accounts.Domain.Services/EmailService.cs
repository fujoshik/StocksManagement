using Accounts.Domain.Abstraction.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Accounts.Domain.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string emailAddressTo, string text)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("judge.kemmer22@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(emailAddressTo));
            email.Subject = "Verification Code";
            email.Body = new TextPart(TextFormat.Plain) { Text = text };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("judge.kemmer22@ethereal.email", "gvEyV47Q2PDTym179A");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}
