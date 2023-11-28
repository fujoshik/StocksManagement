using Accounts.Domain.Abstraction.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Accounts.Domain.Services
{
    public class EmailService : IEmailService
    {
        private const string PDF_PATH = "../../welcome.pdf";
        private const string TEMP_EMAIL = "taurean.cruickshank@ethereal.email";
        private const string TEMP_PASSWORD = "RctQCxMHWdrGh6D47w";
        public void SendEmail(string emailAddressTo, string text)
        {
            if (File.Exists(PDF_PATH))
            {
                File.Delete(PDF_PATH);
            }

            BuildPdf(text);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(TEMP_EMAIL));
            email.To.Add(MailboxAddress.Parse(emailAddressTo));
            email.Subject = "Verification Code";
            var builder = new BodyBuilder();
            builder.Attachments.Add(PDF_PATH);

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(TEMP_EMAIL, TEMP_PASSWORD);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }

        private void BuildPdf(string text)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Header().AlignCenter().Text("Dear customer,").FontSize(30);
                    page.Content()
                    .AlignCenter()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);
                        x.Item().Text("Thank you for your joining StockManagement!");
                        x.Item().Text("In order to complete your registration, please input this code into our Verify endpoint:");
                        x.Item().Text(text).FontSize(15);
                    });
                });
            }).GeneratePdf(PDF_PATH);
        }
    }
}
