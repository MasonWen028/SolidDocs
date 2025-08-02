using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SolidDocs.Models;

namespace SolidDocs.Services
{
    /// <summary>
    /// Implementation of JWT service for OnlyOffice integration
    /// </summary>
    internal class OnlyOfficeJwtService : IOnlyOfficeJwtService
    {
        private readonly SolidDocsOptions _options;

        public OnlyOfficeJwtService(SolidDocsOptions options)
        {
            _options = options;
        }

        public string GenerateToken(string docId, string fileName, string fileUrl, string userId, string userName, bool canEdit = true)
        {
            if (string.IsNullOrEmpty(_options.JwtSecret) || _options.JwtSecret.Length < 32)
                throw new InvalidOperationException("JWT secret must be at least 32 characters long");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.JwtSecret);

            var payload = new
            {
                document = new
                {
                    fileType = "docx",
                    key = docId,
                    title = fileName,
                    url = fileUrl
                },
                permissions = new
                {
                    edit = canEdit,
                    review = false,
                    comment = true
                },
                user = new
                {
                    id = userId,
                    name = userName
                }
            };

            var claims = new List<Claim>
            {
                new Claim("document", System.Text.Json.JsonSerializer.Serialize(payload.document)),
                new Claim("permissions", System.Text.Json.JsonSerializer.Serialize(payload.permissions)),
                new Claim("user", System.Text.Json.JsonSerializer.Serialize(payload.user))
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_options.JwtExpirationHours),
                Issuer = _options.JwtIssuer,
                Audience = _options.JwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_options.JwtSecret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _options.JwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = _options.JwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 