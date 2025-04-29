using System.Threading.Channels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace AspNetCoreEcommerce.Infrastructure.EmailInfrastructure
{
    public class EmailService : IEmailSender
    {
        private readonly string _emaiAddress;
        private readonly string _emailPassword;
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string Name = "AspNetCoreEcommerce";

        public EmailService()
        {
            DotNetEnv.Env.TraversePath().Load();
            _emaiAddress = $"{Environment.GetEnvironmentVariable("EMAIL_HOST_ADDRESS")}";
            _emailPassword = $"{Environment.GetEnvironmentVariable("EMAIL_HOST_PASSWORD")}";
            _smtpServer = $"{Environment.GetEnvironmentVariable("EMAIL_SMTP_SERVER")}";
            _port = int.Parse($"{Environment.GetEnvironmentVariable("EMAIL_PORT")}");
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailDto = new EmailDto
            {
                EmailTo = email,
                Subject = subject,
                Body = htmlMessage,
            };
            await SendMail(emailDto);
            //
        }

        private async Task SendMail(EmailDto emailDto)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(Name, _emaiAddress));
            email.To.Add(new MailboxAddress(Name, emailDto.EmailTo));
            email.Subject = emailDto.Subject;
            email.Body = new TextPart("html") { Text = emailDto.Body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emaiAddress, _emailPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        private async Task Papercut(EmailDto emailDto)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(Name, _emaiAddress));
            email.To.Add(new MailboxAddress(Name, emailDto.EmailTo));
            email.Subject = emailDto.Subject;
            email.Body = new TextPart("html") { Text = emailDto.Body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("localhost", 25);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        public class EmailBackgroundService(
            EmailService emailService,
            Channel<EmailDto> emailChannel,
            ILogger<EmailBackgroundService> logger) : BackgroundService
        {
            private readonly EmailService _emailService = emailService;
            private readonly Channel<EmailDto> _emailChannel = emailChannel;
            private readonly ILogger<EmailBackgroundService> _logger = logger;

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                await foreach (var e in _emailChannel.Reader.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        await _emailService.SendEmailAsync(e.EmailTo, e.Subject, e.Body);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Email failed to send", [e.EmailTo]);
                    }
                }
            }
        }

    }
}