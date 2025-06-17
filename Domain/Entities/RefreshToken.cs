using System.Security.Cryptography;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; private set; } = string.Empty;
        public DateTimeOffset ExpiresIn { get; private set; }
        public bool IsRevoked { get; set; }
        public bool IsExpired => ExpiresIn < DateTimeOffset.UtcNow;

        public RefreshToken(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Token = GenerateToken();
            ExpiresIn = DateTimeOffset.UtcNow.AddDays(7);
            IsRevoked = false;
        }
        private static string GenerateToken()
        {
            const int tokenLength = 64;
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            var randomBytes = new byte[tokenLength];
            randomNumberGenerator.GetBytes(randomBytes);
            var refreshToken = Convert.ToBase64String(randomBytes);

            return refreshToken;
        }
    }

}
