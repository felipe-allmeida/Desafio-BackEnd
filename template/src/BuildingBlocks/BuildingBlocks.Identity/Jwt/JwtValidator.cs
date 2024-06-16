using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BuildingBlocks.Identity.Jwt
{
    public class JwtValidator(AppJwtOptions appJwtSettings)
    {
        private readonly AppJwtOptions _appJwtSettings = appJwtSettings ?? throw new ArgumentNullException(nameof(appJwtSettings));

        public async Task<TokenValidationResult> ValidateToken(string token)
        {
            var handler = new JsonWebTokenHandler();

            var key = Encoding.ASCII.GetBytes(_appJwtSettings.SecretKey);
            var result = await handler.ValidateTokenAsync(token, new TokenValidationParameters()
            {
                ValidIssuer = _appJwtSettings.Issuer,
                ValidAudience = _appJwtSettings.Audience,
                RequireSignedTokens = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            });

            return result;
        }
    }
}
