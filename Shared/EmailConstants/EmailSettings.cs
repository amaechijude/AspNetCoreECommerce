using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreEcommerce.Shared.EmailConstants
{
    public class EmailSettings
    {
        public static string? SmtpServer { get; private set; }
        public static int SmtpPort { get; private set; }
        public static string? SmtpUsername { get; private set; }
        public static string? SmtpPassword { get; private set; }
        public static string? FromEmail => SmtpUsername; // Use the SMTP username as the From email
        public const string FromDisplayName = "AspNetCoreEcommerce"; // Default display name

        // Optional: Add a method to configure from IConfiguration
        public static void Configure(IConfiguration configuration)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            SmtpServer = configuration["SmtpSettings:Server"];
            SmtpPort = int.TryParse(configuration["SmtpSettings:Port"], out var port) ? port : 587; // Default to 587 if parsing fails
            SmtpUsername = configuration["SmtpSettings:Username"];
            SmtpPassword = configuration["SmtpSettings:Password"];
#pragma warning restore CS8601 // Possible null reference assignment.

            Validate(); // Validate the settings after configuration
        }

        // Optional: Add a method to validate the settings
        private static void Validate()
        {
            if (string.IsNullOrEmpty(SmtpServer))
                throw new InvalidOperationException($"{SmtpServer} SMTP server is not configured.");

            if (SmtpPort <= 0)
                throw new InvalidOperationException("SMTP port is not configured.");

            if (string.IsNullOrEmpty(SmtpUsername))
                throw new InvalidOperationException("SMTP username is not configured.");

            if (string.IsNullOrEmpty(SmtpPassword))
                throw new InvalidOperationException("SMTP password is not configured.");
        }
    }
}
