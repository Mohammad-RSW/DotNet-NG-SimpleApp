using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Utility
{
    public static class Token
    {
        private static SymmetricSecurityKey key;

        static Token()
        {
            key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("AbCdEfGhIjKlMnOpQrStUvWxYzAbCdEfGhIjKlMnOpQrStUvWxYz"));
        }

        public static TokenValidationParameters Params => new()
        {
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = key,
            RequireSignedTokens = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
        };

        public static string Generate(int userId)
        {
            JwtSecurityTokenHandler handler = new();

            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256Signature);

            ClaimsIdentity claims = new();

            claims.AddClaim(new Claim(ClaimTypes.Name, userId.ToString()));

            var descriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.MaxValue,
                SigningCredentials = credentials,
                Subject = claims
            };

            SecurityToken token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}