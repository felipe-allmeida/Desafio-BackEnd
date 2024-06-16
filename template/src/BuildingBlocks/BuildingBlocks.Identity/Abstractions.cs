using BuildingBlocks.Identity.Data;
using BuildingBlocks.Identity.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BuildingBlocks.Identity
{
    public static class Abstractions
    {
        public static IdentityBuilder AddIdentityConfiguration(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            return services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddUserStore<IdentityAppDbContext>()
                .AddDefaultTokenProviders();
        }

        public static IdentityBuilder AddDefaultIdentity(this IServiceCollection services, Action<IdentityOptions> options)
        {
            ArgumentNullException.ThrowIfNull(services);

            return services.AddDefaultIdentity<IdentityUser>(options);
        }

        public static IdentityBuilder AddCustomIdentity<TIdentityUser>(this IServiceCollection services, Action<IdentityOptions> options)
            where TIdentityUser : IdentityUser
        {
            ArgumentNullException.ThrowIfNull(services);

            return services.AddDefaultIdentity<TIdentityUser>(options);
        }

        public static IdentityBuilder AddCustomIdentity<TIdentityUser, TKey>(this IServiceCollection services, Action<IdentityOptions> options)
            where TIdentityUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            ArgumentNullException.ThrowIfNull(services);

            return services.AddDefaultIdentity<TIdentityUser>(options);
        }

        public static IdentityBuilder AddDefaultRoles(this IdentityBuilder builder)
        {
            return builder.AddRoles<IdentityRole>();
        }

        public static IdentityBuilder AddCustomRoles<TRole>(this IdentityBuilder builder)
            where TRole : IdentityRole
        {
            return builder.AddRoles<TRole>();
        }

        public static IdentityBuilder AddCustomRoles<TRole, TKey>(this IdentityBuilder builder)
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            return builder.AddRoles<TRole>();
        }

        public static IdentityBuilder AddDefaultEntityFrameworkStores(this IdentityBuilder builder)
        {
            return builder.AddEntityFrameworkStores<IdentityAppDbContext>();
        }

        public static IdentityBuilder AddCustomEntityFrameworkStores<TContext>(this IdentityBuilder builder) where TContext : DbContext
        {
            return builder.AddEntityFrameworkStores<TContext>();
        }

        public static IServiceCollection AddIdentityEntityFrameworkContextConfiguration(
            this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(options);
            return services.AddDbContext<IdentityAppDbContext>(options);
        }

        public static IApplicationBuilder UseAuthConfiguration(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);

            return app.UseAuthentication()
                      .UseAuthorization();
        }

        public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIdentityService, IdentityService>();

            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            var appSettingsSection = configuration.GetSection("JwtSettings");
            var appSettings = appSettingsSection.Get<AppJwtOptions>();
            
            ArgumentNullException.ThrowIfNull(appSettings);
            
            services.Configure<AppJwtOptions>(appSettingsSection);
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

            return services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;                
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.Audience,
                    ValidIssuer = appSettings.Issuer,
                    
                    // Valida a assinatura de um token recebido
                    ValidateIssuerSigningKey = true,

                    // Verifica se um token recebido ainda é válido
                    ValidateLifetime = true,

                    // Tempo de tolerância para a expiração de um token (utilizado
                    // caso haja problemas de sincronismo de horário entre diferentes
                    // computadores envolvidos no processo de comunicação)
                    ClockSkew = TimeSpan.Zero

                };
            });
        }
    }
}
