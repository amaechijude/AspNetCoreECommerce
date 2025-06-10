using System.Net;
using AspNetCoreEcommerce.Domain.Entities;

namespace AspNetCoreEcommerce.Shared
{
    public static class EmailBodyTemplates
    {
        private const string _baseUrl = "http://localhost:3000/auth";
        public static string ConfirmEmailBody(string token, string email)
        {
            var urlEncodedEmail = Uri.EscapeDataString(email);
            var urlEncodedToken = Uri.EscapeDataString(token);
            string htmlTemplate = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <title>Email Confirmation</title>
</head>
<body style=""margin: 0; padding: 0; font-family: Arial, sans-serif; line-height: 1.6;"">
    <center>
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""max-width: 600px; margin: auto;"">
            <!-- Header -->
            <tr>
                <td style=""padding: 20px 0; text-align: center; background-color: #f8f9fa;"">
                    <h1 style=""color: #1a82e2; margin: 0;"">YourAppName</h1>
                </td>
            </tr>

            <!-- Content -->
            <tr>
                <td style=""padding: 30px 20px;"">
                    <h2 style=""color: #333; margin-top: 0;"">Confirm Your Email Address</h2>
                    <p style=""color: #666;"">Hello {{UserName}},</p>
                    <p style=""color: #666;"">Thank you for registering! Please confirm your email address by clicking the button below:</p>
                    
                    <div style=""text-align: center; margin: 40px 0;"">
                        <a href=""{{ConfirmationLink}}"" 
                           style=""background-color: #1a82e2; 
                                  color: #ffffff; 
                                  padding: 15px 30px; 
                                  text-decoration: none; 
                                  border-radius: 5px;
                                  display: inline-block;
                                  font-weight: bold;"">
                            Confirm Email
                        </a>
                    </div>

                    <p style=""color: #666;"">If you didn't create this account, you can safely ignore this email.</p>
                    <p style=""color: #666; font-size: 0.9em;"">This link will expire in 24 hours.</p>
                </td>
            </tr>

            <!-- Footer -->
            <tr>
                <td style=""padding: 20px; background-color: #f8f9fa; text-align: center;"">
                    <p style=""color: #666; font-size: 0.8em; margin: 0;"">
                        Need help? Contact our support team at 
                        <a href=""mailto:{{SupportEmail}}"" style=""color: #1a82e2; text-decoration: none;"">
                            {{SupportEmail}}
                        </a>
                    </p>
                    <p style=""color: #666; font-size: 0.8em; margin: 10px 0 0;"">
                        © {{Year}} Your Company Name. All rights reserved.<br>
                        {{CompanyAddress}}
                    </p>
                    <p style=""font-size: 0.8em;"">
                        <a href=""{{UnsubscribeLink}}"" style=""color: #666; text-decoration: none;"">
                            Unsubscribe
                        </a>
                    </p>
                </td>
            </tr>
        </table>
    </center>
</body>
</html>
";

            var confirmLink = $"{_baseUrl}/confirm-email?email={urlEncodedEmail}&token={urlEncodedToken}";
            string formattedHtml = htmlTemplate
                .Replace("{{ConfirmationLink}}", confirmLink)
                .Replace("{{UserName}}", email)
                .Replace("{{SupportEmail}}", "support@aspnetcoreecommerce.com")
                .Replace("{{Year}}", DateTime.Now.Year.ToString())
                .Replace("{{CompanyAddress}}", "1234 Street Name, City, State, Zip")
                .Replace("{{UnsubscribeLink}}", "unsubscribeUrl");

            return formattedHtml;
        }

        public static string ForgotPasswordBody(string token, string email)
        {
            var urlEncodedEmail = Uri.EscapeDataString(email);
            var urlEncodedToken = Uri.EscapeDataString(token);
            var html = @"<html>
                        <body>
                            <h1>Reset your password</h1>
                            <p>Click the link below to reset your password:</p>
                            <a href='{{ResetLink}}'>Reset Password</a>
                        </body>
                    </html>";
            var resetLink = $"{_baseUrl}/reset-password?email={urlEncodedEmail}&token={urlEncodedToken}";
            return html.Replace("{{ResetLink}}", resetLink);
        }

        public static string VendorOtpBody(Vendor vendor, string code)
        {
            string otpEmailTemplate = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <title>Your OTP Code</title>
</head>
<body style=""margin: 0; padding: 0; font-family: Arial, sans-serif; line-height: 1.6;"">
    <center>
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""max-width: 600px; margin: auto;"">
            <!-- Header -->
            <tr>
                <td style=""padding: 20px 0; text-align: center; background-color: #f8f9fa;"">
                    <h1 style=""color: #1a82e2; margin: 0;"">YourAppName</h1>
                </td>
            </tr>

            <!-- Content -->
            <tr>
                <td style=""padding: 30px 20px;"">
                    <h2 style=""color: #333; margin-top: 0;"">Your Verification Code</h2>
                    <p style=""color: #666;"">Hello {{UserName}},</p>
                    <p style=""color: #666;"">Here is your one-time verification code:</p>
                    
                    <div style=""text-align: center; margin: 40px 0;"">
                        <div style=""display: inline-block; 
                                  background-color: #f8f9fa; 
                                  padding: 20px 40px; 
                                  border-radius: 5px;
                                  font-size: 32px;
                                  font-weight: bold;
                                  letter-spacing: 5px;
                                  color: #1a82e2;"">
                            {{OTPCode}}
                        </div>
                    </div>

                    <p style=""color: #666;"">This code will expire in {{OTPExpiration}} minutes.</p>
                    <p style=""color: #666; font-size: 0.9em;"">Don't share this code with anyone.</p>
                </td>
            </tr>

            <!-- Footer -->
            <tr>
                <td style=""padding: 20px; background-color: #f8f9fa; text-align: center;"">
                    <p style=""color: #666; font-size: 0.8em; margin: 0;"">
                        Need help? Contact our support team at 
                        <a href=""mailto:{{SupportEmail}}"" style=""color: #1a82e2; text-decoration: none;"">
                            {{SupportEmail}}
                        </a>
                    </p>
                    <p style=""color: #666; font-size: 0.8em; margin: 10px 0 0;"">
                        © {{Year}} Your Company Name. All rights reserved.<br>
                        {{CompanyAddress}}
                    </p>
                </td>
            </tr>
        </table>
    </center>
</body>
</html>
";

            // Usage example:
            string formattedOtpEmail = otpEmailTemplate
                .Replace("{{UserName}}", vendor.VendorName)
                .Replace("{{OTPCode}}", code)
                .Replace("{{OTPExpiration}}", "20")
                .Replace("{{SupportEmail}}", "support@aspnetcore.com")
                .Replace("{{Year}}", DateTime.Now.Year.ToString())
                .Replace("{{CompanyAddress}}", "Nigeria");

            return formattedOtpEmail;
        }
    }
}