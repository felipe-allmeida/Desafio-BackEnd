using BikeRental.CrossCutting.MinIO.Options;
using BikeRental.CrossCutting.Storage.Abstractions;
using BikeRental.CrossCutting.Storage.MinIO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace BikeRental.CrossCutting.MinIO.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMinIO(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("MinIO");
            var options = section.Get<MinIOOptions>()!;
            
            services.Configure<MinIOOptions>(section);

            services = services.AddMinio(configureClient => configureClient
                .WithEndpoint(options.Endpoint, options.Port).WithSSL(false)
                .WithCredentials(options.AccessKey, options.SecretKey));

            services.AddScoped<IStorageService, MinIOService>();
        }
    }
}
