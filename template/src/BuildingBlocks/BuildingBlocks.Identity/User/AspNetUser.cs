using BuildingBlocks.Identity.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BuildingBlocks.Identity.User
{

    public class AspNetUser(IHttpContextAccessor accessor) : IAspNetUser
    {
        protected readonly IHttpContextAccessor _accessor = accessor;

        public virtual string Name => _accessor.HttpContext?.User?.Identity?.Name ?? string.Empty;

        public virtual string GetUserId()
        {
            return IsAuthenticated() ? _accessor.HttpContext?.User?.GetUserId() ?? string.Empty : string.Empty;
        }

        public virtual string GetUserEmail()
        {
            return IsAuthenticated() ? _accessor.HttpContext?.User?.GetUserEmail() ?? string.Empty : string.Empty;
        }

        public virtual bool IsAuthenticated()
        {
            return _accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }

        public virtual bool IsInRole(string role)
        {
            return _accessor.HttpContext?.User?.IsInRole(role) ?? false;
        }

        public virtual IEnumerable<Claim> GetUserClaims()
        {
            return _accessor.HttpContext?.User?.Claims ?? [];
        }

        public HttpContext? GetHttpContext()
        {
            return _accessor.HttpContext;
        }
    }
}
