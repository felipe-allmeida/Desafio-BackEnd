using BuildingBlocks.Identity;

namespace BikeRental.API.Services
{
    public class LoggedUserService : ILoggedUserService
    {
        public LoggedUserService(IIdentityService identityService)
        {
            if (identityService is null) throw new ArgumentNullException(nameof(identityService));

            IsAuthenticated = identityService.IsAutenticated();

            IdentityUserId = identityService.GetUserId();
            IdentityUserName = identityService.GetUserName();
            IdentityUserEmail = identityService.GetUserEmail();
            UserRoles = identityService.GetUserRoles();
        }

        public string IdentityUserId { get; private set; }

        public string IdentityUserName { get; private set; }

        public string IdentityUserEmail { get; private set; }

        public IEnumerable<string> UserRoles { get; private set; }
        public bool IsAuthenticated { get; private set; }
    }
}
