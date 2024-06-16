using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BuildingBlocks.Identity.Authorization
{
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequerimentClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) };
        }
    }
}
