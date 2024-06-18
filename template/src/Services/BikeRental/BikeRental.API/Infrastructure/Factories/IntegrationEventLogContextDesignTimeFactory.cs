using BikeRental.CrossCutting.IntegrationEventLog;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.API.Infrastructure.Factories
{
    //public class IntegrationEventLogContextDesignTimeFactory : IDesignTimeDbContextFactory<IntegrationEventLogContext>
    //{
    //    public IntegrationEventLogContext CreateDbContext(string[] args)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<IntegrationEventLogContext>();

    //        optionsBuilder.UseNpgsql(".", options => options.MigrationsAssembly(GetType().Assembly.GetName().Name));

    //        return new IntegrationEventLogContext(optionsBuilder.Options);
    //    }
    //}
}
