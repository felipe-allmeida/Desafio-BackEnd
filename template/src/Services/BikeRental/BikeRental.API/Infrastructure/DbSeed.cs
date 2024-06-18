using BikeRental.API.Extensions;
using BikeRental.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BikeRental.API.Infrastructure.Security;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using BikeRental.Domain.Models.BikeAggregate;

namespace BikeRental.API.Infrastructure
{
    public class DbSeed : IDbSeeder<BikeRentalContext>
    {
        public async Task SeedAsync(BikeRentalContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);

            var rootPath = "";

            if (!context.Roles.Any())
            {
                await CreateOrUpdateRole(context, roleStore, Roles.Admin,
                [
                    new Claim(Claims.Global, ClaimValues.Read),
                    new Claim(Claims.Global, ClaimValues.Write),
                ]);

                await CreateOrUpdateRole(context, roleStore, Roles.DeliveryRider,
                [
                    new Claim(Claims.DeliveryRider, ClaimValues.Read),
                    new Claim(Claims.DeliveryRider, ClaimValues.Write),
                ]);
            }

            if (!context.Users.Any(x => x.NormalizedEmail == "admin@bikerental.com".ToUpperInvariant()))
            {
                var newIdentityUser = new IdentityUser
                {
                    Id = "7881c538-0e31-4141-b3ae-6659f930bb85",
                    UserName = "admin",
                    NormalizedUserName = "admin".ToUpperInvariant(),
                    Email = "admin@bikerental.com",
                    NormalizedEmail = "admin@bikerental.com".ToUpperInvariant(),
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

                newIdentityUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(newIdentityUser, "admin@1234");

                var userStore = new UserStore<IdentityUser>(context);
                var result = await userStore.CreateAsync(newIdentityUser);

                if (result.Succeeded)
                {
                    var identityUser = await userStore.FindByEmailAsync(newIdentityUser.Email.ToUpperInvariant());
                    await userStore.AddToRoleAsync(identityUser!, Roles.Admin);

                    await context.SaveChangesAsync();
                }
            }

            if (!context.Users.Any(x => x.NormalizedEmail == "user@bikerental.com".ToUpperInvariant()))
            {
                var newIdentityUser = new IdentityUser
                {
                    Id = "F7B52D1A-4FCE-4D23-A6A2-AC7E4E62F03E",
                    UserName = "user",
                    NormalizedUserName = "user".ToUpperInvariant(),
                    Email = "user@bikerental.com",
                    NormalizedEmail = "user@bikerental.com".ToUpperInvariant(),
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

                newIdentityUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(newIdentityUser, "user@1234");

                var userStore = new UserStore<IdentityUser>(context);
                var result = await userStore.CreateAsync(newIdentityUser);

                if (result.Succeeded)
                {
                    var identityUser = await userStore.FindByEmailAsync(newIdentityUser.Email.ToUpperInvariant());
                    await userStore.AddToRoleAsync(identityUser!, Roles.DeliveryRider);

                    await context.SaveChangesAsync();
                }
            }

            if (!context.Set<Bike>().Any())
            {
                for (int i = 0; i < 10; i++)
                {
                    var bike = new Bike($"ABCD0{i.ToString("D2")}", 2021, $"Model {i}");

                    context.Set<Bike>().Add(bike);

                    await context.SaveChangesAsync();
                }
            }
        }

        public static async Task CreateOrUpdateRole(BikeRentalContext context, RoleStore<IdentityRole> roleStore, string roleName, Claim[] claims)
        {
            var role = await context.Roles.FirstOrDefaultAsync(x => x.NormalizedName == roleName);

            if (role == null)
            {
                role = new IdentityRole { Name = roleName, NormalizedName = roleName };
                var result = await roleStore.CreateAsync(role);

                if (!result.Succeeded) return; // Handle failure appropriately                

                foreach (var claim in claims)
                {
                    await roleStore.AddClaimAsync(role, claim);
                }
            }
            else
            {
                foreach (var claim in claims)
                {
                    if (!context.RoleClaims.Any(x => x.RoleId == role.Id && x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
                    {
                        await roleStore.AddClaimAsync(role, new Claim(claim.Type, claim.Value));
                    }
                }
            }
        }
    }
}
