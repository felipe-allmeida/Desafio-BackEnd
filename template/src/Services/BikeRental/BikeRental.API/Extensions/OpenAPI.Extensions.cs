using BikeRental.API.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BikeRental.API.Extensions
{
    public static partial class HostingExtension
    {
        public static void AddOpenApi(this IHostApplicationBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            var openApi = configuration.GetSection("OpenApi").Get<OpenApiOptions>();

            ArgumentNullException.ThrowIfNull(openApi);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                //TODO: we need to add the document when there is no AREA
                foreach (var doc in openApi.Documents)
                {
                    options.SwaggerDoc(doc.Area, new OpenApiInfo
                    {
                        Title = doc.Title,
                        Version = doc.Version,
                        Description = doc.Description
                    });
                }

                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var area = apiDesc.ActionDescriptor.RouteValues["area"];

                    return docName == area || area is null;
                });



                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {

                    Description = "Input the JWT like: Bearer {your token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                        Array.Empty<string>()
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }

        public static void UseOpenApi(this WebApplication app)
        {
            var configuration = app.Configuration;
            var openApi = configuration.GetSection("OpenApi").Get<OpenApiOptions>();

            ArgumentNullException.ThrowIfNull(openApi);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(setup =>
                {
                    foreach (var doc in openApi.Documents)
                    {
                        var swaggerUrl = $"/swagger/{doc.Area}/swagger.json";

                        setup.SwaggerEndpoint(swaggerUrl, doc.Title);
                    }
                });
            }
        }

        private sealed class AuthorizeCheckOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var metadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;

                if (!metadata.OfType<IAuthorizeData>().Any())
                {
                    return;
                }

                operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                };

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new() {
                        {
                            scheme,
                            Array.Empty<string>()
                        }
                    }
                };
            }
        }
    }

}
