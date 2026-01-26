using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Company.PL.Helper.MailKitFeature
{
    public class MailService : IMailService
    {
        private readonly IOptions<MailKitSetting> _mailSetting;

        public MailService(IOptions<MailKitSetting> mailSetting)
        {
            _mailSetting = mailSetting;
        }

        public bool SendMail(Email email)
        {
            try
            {
                // Build Message

                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress(_mailSetting.Value.DisplayName, _mailSetting.Value.Email)); // Sender
                mail.To.Add(MailboxAddress.Parse(email.To)); // Receiver
                mail.Subject = email.Subject; // Subject

                var body = new BodyBuilder();
                body.TextBody = email.Body;
                mail.Body = body.ToMessageBody(); // Body


                // Open Connection With Mail Server
                using var SmtpServerClient = new SmtpClient(); // Server Client
                SmtpServerClient.Connect(_mailSetting.Value.Host, _mailSetting.Value.Port);
                SmtpServerClient.Authenticate(_mailSetting.Value.Email, _mailSetting.Value.Password);


                // Send Mail
                SmtpServerClient.Send(mail);

                return true ;
            }

            catch (Exception)
            {
                return false;
            }

        }
    }
}
