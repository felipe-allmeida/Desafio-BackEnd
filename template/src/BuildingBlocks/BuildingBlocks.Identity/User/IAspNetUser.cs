using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BuildingBlocks.Identity.User
{
    public interface IAspNetUser
    {
        string Name { get; }
        string GetUserId();
        string GetUserEmail();
        bool IsAuthenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetUserClaims();
        HttpContext? GetHttpContext();
    }

}
