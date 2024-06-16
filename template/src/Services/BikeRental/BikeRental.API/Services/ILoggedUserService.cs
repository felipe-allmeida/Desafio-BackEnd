namespace BikeRental.API.Services
{
    public interface ILoggedUserService
    {
        public string IdentityUserId { get; }
        public string IdentityUserName { get; }
        public string IdentityUserEmail { get; }
        public IEnumerable<string> UserRoles { get; }
        public bool IsAuthenticated { get; }

    }
}
