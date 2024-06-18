using BikeRental.API.DTOs.V1.Responses;
using BikeRental.API.Infrastructure;
using BikeRental.API.Infrastructure.Filters;
using BikeRental.API.Infrastructure.Middlewares;
using BikeRental.API.Infrastructure.Security;
using BikeRental.API.Infrastructure.Serialization;
using BikeRental.API.Services;
using BikeRental.Application.Behaviours;
using BikeRental.Application.Commands.V1.Admin.CreateBike;
using BikeRental.Application.Commands.V1.Admin.CreateDeliveryRequest;
using BikeRental.Application.Commands.V1.Admin.UpdateBikePlate;
using BikeRental.Application.Commands.V1.User.CreateDeliveryRider;
using BikeRental.Application.Commands.V1.User.RentBike;
using BikeRental.Application.Commands.V1.User.UpdateDeliveryRiderCnh;
using BikeRental.Application.Commands.V1.User.UpdateRentStatus;
using BikeRental.Application.IntegrationEvents;
using BikeRental.Application.IntegrationEvents.EventHandling;
using BikeRental.Application.IntegrationEvents.Events;
using BikeRental.Application.Queries.V1.User.GetRentBikeInfo;
using BikeRental.CrossCutting.EventBus;
using BikeRental.CrossCutting.EventBus.Abstractions;
using BikeRental.CrossCutting.EventBusRabbitMQ;
using BikeRental.CrossCutting.IntegrationEventLog;
using BikeRental.CrossCutting.IntegrationEventLog.Services;
using BikeRental.CrossCutting.MinIO.Extensions;
using BikeRental.Data;
using BikeRental.Data.QueryRepositories;
using BikeRental.Data.Repositories;
using BikeRental.Domain.Models.BikeAggregate;
using BikeRental.Domain.Models.DeliveryRequestAggregate;
using BikeRental.Domain.Models.DeliveryRequestNotificationAggregate;
using BikeRental.Domain.Models.DeliveryRiderAggregate;
using BikeRental.Domain.Models.RentalAggregate;
using BuildingBlocks.Identity;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using System.Data.Common;
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
                .AddNpgSql(configuration.GetConnectionString("Database")!, name: "db-check", tags: ["bikerentaldb"])
                .AddRabbitMQ($"amqp://{configuration["EventBus:RabbitMQ:EventBusConnection"]}", name: "rabbitmq-check", tags: ["rabbitmqbus"]);
        }

        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            IConfiguration configuration = builder.Configuration;

            builder.Services.AddMigration<BikeRentalContext, DbSeed>();
            builder.Services.AddMigration<IntegrationEventLogContext>();

            builder.Services.AddDbContext<BikeRentalContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Database")!,
                    npgsqlOptionsAction: options =>
                    {
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        options.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                    });

                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                }
            }, ServiceLifetime.Scoped);

            builder.Services.AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Database")!, options =>
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

            services.AddSingleton<IValidator<CreateBikeCommand>, CreateBikeCommandValidator>();
            services.AddSingleton<IValidator<UpdateBikePlateCommand>, UpdateBikePlateCommandValidator>();
            services.AddSingleton<IValidator<CreateDeliveryRiderCommand>, CreateDeliveryRiderCommandValidator>();
            services.AddSingleton<IValidator<UpdateDeliveryRiderCnhCommand>, UpdateDeliveryRiderCnhValidator>();
            services.AddSingleton<IValidator<RentBikeCommand>, RentBikeCommandValidator>();
            services.AddSingleton<IValidator<UpdateRentStatusCommand>, UpdateRentStatusCommandValidator>();
            services.AddSingleton<IValidator<CreateDeliveryRequestCommand>,  CreateDeliveryRequestCommandValidator>();

            services.AddSingleton<IValidator<GetRentBikeInfoQuery>, GetRentBikeInfoQueryValidator>();

            services.AddScoped<ILoggedUserService, LoggedUserService>();

            services.AddScoped<IBikeRepository, BikeRepository>();
            services.AddScoped<IBikeQueryRepository, BikeQueryRepository>();

            services.AddScoped<IDeliveryRiderRepository, DeliveryRiderRepository>();
            services.AddScoped<IDeliveryRiderQueryRepository, DeliveryRiderQueryRepository>();

            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IRentalQueryRepository, RentalQueryRepository>();

            services.AddScoped<IDeliveryRequestRepository, DeliveryRequestRepository>();
            services.AddScoped<IDeliveryRequestNotificationRepository,  DeliveryRequestNotificationRepository>();

            services.AddJwtAuthentication(configuration);

            services.AddAuthorization(PoliciesConfiguration.ConfigureAuthorization);
        }

        public static void AddApplicationIntegrationServices(this IHostApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var services = builder.Services;

            services.AddMinIO(configuration);

            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c, typeof(CreateBikeCommand).Assembly));
            services.AddTransient<IIntegrationEventService, IntegrationEventService>();
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = configuration["EventBus:RabbitMQ:EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(configuration["EventBus:RabbitMQ:EventBusUserName"]))
                    factory.UserName = configuration["EventBus:RabbitMQ:EventBusUserName"];

                if (!string.IsNullOrEmpty(configuration["EventBus:RabbitMQ:EventBusPassword"]))
                    factory.Password = configuration["EventBus:RabbitMQ:EventBusPassword"];

                var retryCount = 5;
                var eventBusRetryCountStr = configuration["EventBus:RabbitMQ:EventBusRetryCount"];
                if (!string.IsNullOrEmpty(eventBusRetryCountStr))
                {
                    retryCount = int.Parse(eventBusRetryCountStr);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });


            //services.AddMandrill(configuration);
            //services.AddSendGrid(configuration);
        }        

        public static void AddEventBus(this IHostApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var services = builder.Services;

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var subscriptionClientName = configuration["EventBus:RabbitMQ:SubscriptionClientName"]!;
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                var evtBusRetryCountStr = configuration["EventBus:RabbitMQ:EventBusRetryCount"];

                if (!string.IsNullOrEmpty(evtBusRetryCountStr))
                {
                    retryCount = int.Parse(evtBusRetryCountStr);
                }

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, sp, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddTransient<DeliveryRequestIntegrationIntegrationEventHandler>();
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

            var eventBus = app.Services.GetRequiredService<IEventBus>();

            eventBus.Subscribe<DeliveryRequestCreatedIntegrationEvent, DeliveryRequestIntegrationIntegrationEventHandler>();
            eventBus.Subscribe<DeliveryRequestNotificationCreatedIntegrationEvent, DeliveryRequestIntegrationIntegrationEventHandler>();

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
