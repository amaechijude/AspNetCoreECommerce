using System.Security.Claims;
using System.Text;
using AspNetCoreEcommerce.Domain.Entities;
using AspNetCoreEcommerce.Shared;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreEcommerce.Application.UseCases.Authentication
{
    public class TokenProvider
    {
         public string CreateAppUsertoken(User appUser)
        {
            DotNetEnv.Env.Load();
            var secretKey = $"{Environment.GetEnvironmentVariable("JWT_SECRET_KEY")}";
            var jwtIssuer = $"{Environment.GetEnvironmentVariable("JWT_ISSUER")}";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
#pragma warning disable CS8604 // Possible null reference argument.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(JwtRegisteredClaimNames.Sub, appUser.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, appUser.Email)
                    ]
                ),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = credentials,
                Issuer = jwtIssuer,
                Audience = jwtIssuer
            };
#pragma warning restore CS8604 // Possible null reference argument.

            JsonWebTokenHandler handler = new();

            return handler.CreateToken(tokenDescriptor);
        }
    }
}
