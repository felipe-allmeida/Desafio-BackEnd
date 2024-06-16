using BikeRental.API.DTOs.V1.Responses;
using BikeRental.API.Infrastructure;
using BikeRental.API.Infrastructure.Filters;
using BikeRental.API.Infrastructure.Middlewares;
using BikeRental.API.Infrastructure.Security;
using BikeRental.API.Infrastructure.Serialization;
using BikeRental.API.Services;
using BikeRental.Application.Behaviours;
using BikeRental.Application.Commands.V1.Admin.CreateBike;
using BikeRental.Application.Commands.V1.Admin.UpdateBikePlate;
using BikeRental.CrossCutting.MinIO.Extensions;
using BikeRental.Data;
using BikeRental.Data.QueryRepositories;
using BikeRental.Data.Repositories;
using BikeRental.Domain.Models.BikeAggregate;
using BuildingBlocks.Identity;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace BikeRental.API.Extensions
{
    public static partial class HostingExtensions
    {
        public static void AddServiceDefaults(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            builder.Services.AddRouting(options =>
            {
                options.LowercaseQueryStrings = true;
                options.LowercaseUrls = true;
            });

            builder.Services
                .AddControllers(options =>
                {
                    options.ValueProviderFactories.Add(new SnakeCaseQueryValueProviderFactory());

                    options.Filters.Add(typeof(HttpExceptionFilter));
                    options.Filters.Add(new ProducesAttribute("application/json"));
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
                    options.JsonSerializerOptions.DictionaryKeyPolicy = new SnakeCaseNamingPolicy();
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(new SnakeCaseNamingPolicy()));
                });

            builder.Services.AddResponseCaching();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.AddEndpointsApiExplorer();

            var origins = configuration.GetSection("Origins").Get<string[]>()!;
            builder.Services.AddCors(options => PoliciesConfiguration.ConfigureCors(options, origins));

            // Add Health Checks
            builder.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddNpgSql(configuration.GetConnectionString("BikeRentalContext")!, name: "Db-check", tags: Array.Empty<string>());
        }

        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            IConfiguration configuration = builder.Configuration;

            builder.Services.AddMigration<BikeRentalContext, DbSeed>();

            // Add DbContext
            builder.Services.AddDbContext<BikeRentalContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("BikeRentalContext")!,
                    npgsqlOptionsAction: options =>
                    {
                        options.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                    });

                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            // Add IdentityDbContext
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 0;

                options.Lockout.MaxFailedAccessAttempts = 5;
            })
           .AddEntityFrameworkStores<BikeRentalContext>()
           .AddDefaultTokenProviders();

            builder.Services.Configure<DataProtectionTokenProviderOptions>(x => x.TokenLifespan = TimeSpan.FromHours(1));

            var services = builder.Services;

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining(typeof(CreateBikeCommand));

                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            });

            services.AddSingleton<IValidator<CreateBikeCommand>, CreateBikeCommandValidation>();
            services.AddSingleton<IValidator<UpdateBikePlateCommand>, UpdateBikePlateCommandValidator>();

            services.AddScoped<ILoggedUserService, LoggedUserService>();

            services.AddScoped<IBikeRepository, BikeRepository>();
            services.AddScoped<IBikeQueryRepository, BikeQueryRepository>();


            services.AddJwtAuthentication(configuration);

            services.AddAuthorization(PoliciesConfiguration.ConfigureAuthorization);
        }

        public static void AddApplicationIntegrationServices(this IHostApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var services = builder.Services;

            services.AddMinIO(configuration);

            //services.AddMandrill(configuration);
            //services.AddSendGrid(configuration);
        }

        /// <summary>
        /// Configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static void ConfigurePipeline(this WebApplication app)
        {
            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseOpenApi();

            app.UseHttpsRedirection();

            app.UseResponseCaching();

            app.UseCors(Policies.AllowSpecificOrigins);

            var env = app.Services.GetRequiredService<IWebHostEnvironment>();
            app.UseExceptionHandler(ExceptionHandlerMiddleware.ExceptionHandler(env));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRequestLocalization(options =>
            {
                options.ApplyCurrentCultureToResponseHeaders = true;

                var supportedCultures = new[] { "pt-BR", "en-US" };

                options
                    .SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures);

            });

            app.UseHealthChecks("/health", GetHealthCheckOptions());

            app.MapHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.MapHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.MapControllers();
        }

        private static HealthCheckOptions GetHealthCheckOptions()
        {
            return new HealthCheckOptions
            {
                AllowCachingResponses = false,
                ResponseWriter = async (context, healthReport) =>
                {
                    var results = healthReport.Entries.Select(pair =>
                    {
                        return KeyValuePair.Create(pair.Key, new HealthCheckResultDto
                        {
                            Status = pair.Value.Status.ToString(),
                            Description = pair.Value.Description,
                            Duration = pair.Value.Duration.TotalSeconds.ToString() + "s",
                            ExceptionMessage = pair.Value.Exception != null ? pair.Value.Exception.Message : string.Empty,
                            Data = pair.Value.Data
                        });
                    }).ToDictionary(k => k.Key, v => v.Value);

                    var response = new HealthCheckResponseDto
                    {
                        Status = healthReport.Status.ToString(),
                        TotalDuration = healthReport.TotalDuration.TotalSeconds.ToString() + "s",
                        Results = results
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            };
        }
    }
}
