using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Company.PL.Helper.MailKitFeature
{
    public class MailService : IMailService
    {
        private readonly MailKitSetting _mailSetting;
        private readonly ILogger<MailService> _logger;

        public MailService(IOptions<MailKitSetting> mailSetting, ILogger<MailService> logger)
        {
            _mailSetting = mailSetting.Value; 
            _logger = logger;
        }

        public async Task<bool> SendMailAsync(Email email)
        {
            try
            {
                // ── Build Message ──────────────────────────────────
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Email));
                mail.To.Add(MailboxAddress.Parse(email.To));
                mail.Subject = email.Subject;

                var body = new BodyBuilder();

                if (email.IsHtml)
                    body.HtmlBody = email.Body;   
                else
                    body.TextBody = email.Body;  

                mail.Body = body.ToMessageBody();

                // ── Connect & Send ─────────────────────────────────
                using var smtpClient = new SmtpClient();

                await smtpClient.ConnectAsync(
                    _mailSetting.Host,
                    _mailSetting.Port,
                    SecureSocketOptions.StartTls  
                );

                await smtpClient.AuthenticateAsync(_mailSetting.Email, _mailSetting.Password);

                await smtpClient.SendAsync(mail);

                await smtpClient.DisconnectAsync(quit: true); 

                _logger.LogInformation("Email sent successfully to {Recipient}", email.To);

                return true;
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Failed to send email to {Recipient}", email.To);
                return false;
            }
        }
    }
}