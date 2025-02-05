using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.Aes128CbcHmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(JwtRegisteredClaimNames.Sub, customer.CustomerID.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, customer.CustomerEmail)
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

        public string Create(Vendor vendor)
        {
            DotNetEnv.Env.Load();
            var secretKey = $"{Environment.GetEnvironmentVariable("JWT_SECRET_KEY")}";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.Aes128CbcHmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(JwtRegisteredClaimNames.Sub, vendor.VendorId.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, vendor.VendorEmail)
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