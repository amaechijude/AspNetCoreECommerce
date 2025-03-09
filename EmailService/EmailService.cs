using System.Threading.Channels;
using MailKit.Net.Smtp;
using MimeKit;

namespace AspNetCoreEcommerce.EmailService
{
    public class EmailService
    {
        private readonly string _emaiAddress;
        private readonly string _emailPassword;
        private readonly string _smtpServer;
        private readonly int _port;

        public EmailService()
        {
            DotNetEnv.Env.TraversePath().Load();
            _emaiAddress = $"{Environment.GetEnvironmentVariable("EMAIL_ADDRESS")}";
            _emailPassword = $"{Environment.GetEnvironmentVariable("EMAIL_PASSWORD")}";
            _smtpServer = $"{Environment.GetEnvironmentVariable("SMTP_SERVER")}";
            _port = 587;//Convert.ToInt32($"{Environment.GetEnvironmentVariable("EMAIL_PORT")}");
        }

        public async Task SendMail(EmailDto emailDto)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("AspNetCoreEcommerce", _emaiAddress));
            email.To.Add(new MailboxAddress(emailDto.Name, emailDto.EmailTo));
            email.Subject = emailDto.Subject;
            email.Body = new TextPart("html") { Text = emailDto.Body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emaiAddress, _emailPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            return;
        }

        public class EmailBackgroundService : BackgroundService
        {
            private readonly EmailService _emailService;
            private readonly Channel<EmailDto> _emailChannel;

            public EmailBackgroundService(EmailService emailService, Channel<EmailDto> emailChannel) 
            {
                _emailService = emailService;
                _emailChannel = emailChannel;
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                await foreach (var emailDto in _emailChannel.Reader.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        await _emailService.SendMail(emailDto);
                    }
                    catch (Exception)
                    {
                        //Log ex
                    }
                }
            }
        }

    }
}
