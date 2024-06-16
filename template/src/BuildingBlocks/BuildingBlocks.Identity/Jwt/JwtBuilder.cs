using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BuildingBlocks.Identity.Jwt
{
    public class JwtBuilder<TIdentityUser, TIdentityRole, TKey>(UserManager<TIdentityUser> userManager, RoleManager<TIdentityRole> roleManager, AppJwtOptions appJwtSettings, string email, Dictionary<string, string>? customClaims = null)
            where TIdentityUser : IdentityUser<TKey>
            where TIdentityRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TIdentityUser> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly RoleManager<TIdentityRole> _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        private readonly AppJwtOptions _appJwtSettings = appJwtSettings ?? throw new ArgumentNullException(nameof(appJwtSettings));
        private readonly string _email = string.IsNullOrEmpty(email) ? throw new ArgumentNullException(nameof(email)) : email;
        private readonly Dictionary<string, string> _customClaims = customClaims ?? [];

        public async Task<object> GenerateAccessAndRefreshToken()
        {
            var user = await _userManager.FindByEmailAsync(_email.ToUpperInvariant());
            var userRoles = await _userManager.GetRolesAsync(user);

            return new
            {
                user = new 
                {
                    id = user.Id,         
                    user_name = user.UserName,
                    email = user.Email,
                    roles = userRoles.ToList(),
                },
                access_token = await GenerateAccessToken(user, userRoles),
                refresh_token = await GenerateRefreshToken(user),
                expires = DateTime.UtcNow.AddHours(_appJwtSettings.Expiration)
            };
        }

        private async Task<string> GenerateAccessToken(TIdentityUser user, IList<string> userRoles)
        {
            ArgumentNullException.ThrowIfNull(user);

            var identityClaims = new ClaimsIdentity();

            identityClaims.AddClaims(await _userManager.GetClaimsAsync(user));
            identityClaims.AddClaims(userRoles.Select(s => new Claim("role", s)));

            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);
                identityClaims.AddClaims(await _roleManager.GetClaimsAsync(role));
            }

            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var kvp in _customClaims)
            {
                identityClaims.AddClaim(new Claim(kvp.Key, kvp.Value));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appJwtSettings.SecretKey);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appJwtSettings.Issuer,
                Audience = _appJwtSettings.Audience,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appJwtSettings.Expiration),
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow,
                TokenType = "at+jwt",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private async Task<string> GenerateRefreshToken(TIdentityUser user)
        {
            var jti = Guid.NewGuid().ToString();
            var identityClaims = new ClaimsIdentity();

            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, _email));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, jti));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appJwtSettings.SecretKey);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appJwtSettings.Issuer,
                Audience = _appJwtSettings.Audience,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddDays(30),
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow,
                TokenType = "rt+jwt",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            });

            var generatedToken = tokenHandler.WriteToken(token);

            await UpdateLastGeneratedClaim(user, jti);

            return generatedToken;
        }

        private async Task UpdateLastGeneratedClaim(TIdentityUser user, string jti)
        {
            var claims = await _userManager.GetClaimsAsync(user);

            var newLastRtClaim = new Claim("LastRefreshToken", jti);

            var claimLastRt = claims.FirstOrDefault(f => f.Type == "LastRefreshToken");

            if (claimLastRt != null)
                await _userManager.ReplaceClaimAsync(user, claimLastRt, newLastRtClaim);
            else
                await _userManager.AddClaimAsync(user, newLastRtClaim);
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
    }

    public class JwtBuilder<TIdentityUser, TIdentityRole>(UserManager<TIdentityUser> userManager, RoleManager<TIdentityRole> roleManager, AppJwtOptions appJwtSettings, string email, Dictionary<string, string>? customClaims = null) : JwtBuilder<TIdentityUser, TIdentityRole, string>(userManager, roleManager, appJwtSettings, email, customClaims)
        where TIdentityUser : IdentityUser<string>
        where TIdentityRole : IdentityRole<string>
    {
    }

    public sealed class JwtBuilder(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AppJwtOptions appJwtSettings, string email, Dictionary<string, string>? customClaims = null) 
        : JwtBuilder<IdentityUser, IdentityRole>(userManager, roleManager, appJwtSettings, email, customClaims)
    {
    }
}
