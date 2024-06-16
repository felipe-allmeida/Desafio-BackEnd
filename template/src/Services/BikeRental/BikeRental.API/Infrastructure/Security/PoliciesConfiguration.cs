using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace BikeRental.API.Infrastructure.Security
{
    public static class PoliciesConfiguration
    {
        public static void ConfigureCors(CorsOptions options, string[] origins)
        {
            options.AddPolicy(Policies.AllowSpecificOrigins,
                builder => builder
                    .WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        }

        public static void ConfigureAuthorization(AuthorizationOptions options)
        {
            options.AddPolicy(Policies.NotAnonymous, builder => builder.RequireAuthenticatedUser());

            options.AddPolicy(Policies.AdminRead, builder => builder.RequireAuthenticatedUser().RequireClaim(Claims.Global, ClaimValues.Read));
            options.AddPolicy(Policies.AdminWrite, builder => builder.RequireAuthenticatedUser().RequireClaim(Claims.Global, ClaimValues.Write));

            options.AddPolicy(Policies.DeliveryRiderRead, builder => builder
                .RequireAuthenticatedUser().RequireAssertion(x => x.User.HasClaim(Claims.Global, ClaimValues.Read) || x.User.HasClaim(Claims.DeliveryRider, ClaimValues.Read)));
            options.AddPolicy(Policies.DeliveryRiderWrite, builder => builder
                .RequireAuthenticatedUser().RequireAssertion(x => x.User.HasClaim(Claims.Global, ClaimValues.Write) || x.User.HasClaim(Claims.DeliveryRider, ClaimValues.Write)));
        }
    }
}
