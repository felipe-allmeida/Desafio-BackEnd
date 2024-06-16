namespace BuildingBlocks.Identity
{
    public interface IIdentityService
    {
        string GetUserId();
        string GetUserName();
        string GetUserEmail();
        IEnumerable<string> GetUserRoles();
        string GetClaim(string name);
        Guid GetGuidClaim(string name);
        int? GetIntClaim(string name);
        long? GetLongClaim(string name);
        bool IsAutenticated();
    }

}
