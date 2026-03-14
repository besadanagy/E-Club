
namespace E_Club.Services
{
    public class MailSenderServices : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<MailSenderServices> _logger;

        public MailSenderServices(IOptions<MailSettings> mailSettings, ILogger<MailSenderServices> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            try
            {
                var message = new MimeMessage();

                // ◀️ تأكدي إن _mailSettings.Email مش فاضي
                if (string.IsNullOrWhiteSpace(_mailSettings.Email))
                    throw new InvalidOperationException("MailSettings.Email is not configured!");

                message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;
                message.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

                using var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                _logger.LogInformation("Connecting to {Host}:{Port} with email {FromEmail}",
                    _mailSettings.Host, _mailSettings.Port, _mailSettings.Email);

                await client.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

                _logger.LogInformation("Authenticating with {FromEmail}", _mailSettings.Email);
                await client.AuthenticateAsync(_mailSettings.Email, _mailSettings.Password);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully to {ToEmail}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {ToEmail}. From: {FromEmail}, Host: {Host}:{Port}",
                    email, _mailSettings.Email, _mailSettings.Host, _mailSettings.Port);
                throw;
            }
        }
    }
}