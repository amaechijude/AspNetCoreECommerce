using System.Security.Claims;
using System.Text;
using AspNetCoreEcommerce.Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreEcommerce.Authentication
{
    public class TokenProvider
    {
        public string Create(Customer customer)
        {
            DotNetEnv.Env.Load();
            var secretKey = $"{Environment.GetEnvironmentVariable("JWT_SECRET_KEY")}";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(JwtRegisteredClaimNames.Sub, customer.CustomerID.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, customer.CustomerEmail),
                        new Claim(ClaimTypes.Role, GlobalConstants.customerRole)
                    ]
                ),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = credentials,
                Issuer = $"{Environment.GetEnvironmentVariable("JWT_ISSUER")}",
                Audience = GlobalConstants.customerRole
            };

            JsonWebTokenHandler handler = new();
            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }

        public string CreateVendorToken(Vendor vendor)
        {
            DotNetEnv.Env.Load();
            var secretKey = $"{Environment.GetEnvironmentVariable("JWT_SECRET_KEY")}";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(JwtRegisteredClaimNames.Sub, vendor.VendorId.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, vendor.VendorEmail),
                       new Claim(ClaimTypes.Role, GlobalConstants.vendorRole)
                    ]
                ),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = credentials,
                Issuer = $"{Environment.GetEnvironmentVariable("JWT_ISSUER")}",
                Audience = GlobalConstants.vendorRole
            };

            JsonWebTokenHandler handler = new();
            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}
