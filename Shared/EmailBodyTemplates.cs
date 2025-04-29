namespace AspNetCoreEcommerce.Shared
{
    public static class EmailBodyTemplates
    {
        public static string ConfirmEmailBody(string token, string email, HttpRequest httpRequest)
        {
            var urlEncodedEmail = Uri.EscapeDataString(email);
            var urlEncodedToken = Uri.EscapeDataString(token);
            var html = @"<html>
                        <body>
                            <h1>Confirm your email</h1>
                            <p>Click the link below to confirm your email address:</p>
                            <a href='{{ConfirmLink}}'>Confirm Email</a>
                        </body>
                    </html>";
            var confirmLink = $"{httpRequest.Scheme}/{httpRequest.Host}/confirm-email?token={urlEncodedToken}&email={urlEncodedEmail}";
            return html.Replace("{{ConfirmLink}}", confirmLink);
        }

        public static string ForgotPasswordBody(string token, string email, HttpRequest httpRequest)
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
            var resetLink = $"{httpRequest.Scheme}/{httpRequest.Host}/reset-password?token={urlEncodedToken}&email={urlEncodedEmail}";
            return html.Replace("{{ResetLink}}", resetLink);
        }
    }
}