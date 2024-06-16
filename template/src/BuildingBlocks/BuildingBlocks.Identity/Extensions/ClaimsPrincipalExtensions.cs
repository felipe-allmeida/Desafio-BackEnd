using System.Security.Claims;

namespace BuildingBlocks.Identity.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            ArgumentNullException.ThrowIfNull(principal);

            return principal?.Identity?.Name ?? string.Empty;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value ?? string.Empty;
        }

        public static string GetUserRole(this ClaimsPrincipal principal)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var claim = principal.FindFirst(ClaimTypes.Role);
            return claim?.Value ?? string.Empty;
        }
    }

}
