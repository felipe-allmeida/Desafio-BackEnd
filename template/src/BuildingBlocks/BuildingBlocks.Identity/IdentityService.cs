using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BuildingBlocks.Identity
{
    internal class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _accessor;

        public IdentityService(IHttpContextAccessor accessor)
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        }

        public virtual string GetUserId()
        {
            if (_accessor.HttpContext is null) return String.Empty;
            var claim = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return claim is null ? String.Empty : claim.Value;
        }

        public virtual string GetUserName()
        {
            return IsAutenticated() ? _accessor.HttpContext?.User.Identity?.Name ?? String.Empty : String.Empty;
        }

        public virtual string GetUserEmail()
        {
            if (_accessor.HttpContext is null) return String.Empty;
            var claim = _accessor.HttpContext.User.FindFirst(ClaimTypes.Email);
            return claim is null ? String.Empty : claim.Value;
        }

        public virtual IEnumerable<string> GetUserRoles()
        {
            if (_accessor.HttpContext is null) return Enumerable.Empty<string>();
            var claim = _accessor.HttpContext.User.FindAll(ClaimTypes.Role);
            return claim is null ? Enumerable.Empty<string>() : claim.Select(x => x.Value);
        }

        public virtual string GetClaim(string name)
        {
            if (_accessor.HttpContext is null) return String.Empty;
            var claim = _accessor.HttpContext.User.FindFirst(name);
            return claim is null ? String.Empty : claim.Value;
        }

        public virtual Guid GetGuidClaim(string name)
        {
            if (_accessor.HttpContext is null) return Guid.Empty;
            var claim = _accessor.HttpContext.User.FindFirst(name);
            return claim is null ? Guid.Empty : Guid.Parse(claim.Value);
        }

        public virtual int? GetIntClaim(string name)
        {
            if (_accessor.HttpContext is null) return null;
            var claim = _accessor.HttpContext.User.FindFirst(name);
            return claim is null ? null : int.Parse(claim.Value);
        }

        public virtual long? GetLongClaim(string name)
        {
            if (_accessor.HttpContext is null) return null;
            var claim = _accessor.HttpContext.User.FindFirst(name);
            return claim is null ? null : long.Parse(claim.Value);
        }

        public virtual bool IsAutenticated()
        {
            return _accessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        }
    }

}
