using BikeRental.Domain.Enums;
using BikeRental.Domain.Models.RentalAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRental.Data.EntityTypeConfiguration
{
    public class RentalEntityTypeConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.ToTable("rental", BikeRentalContext.DEFAULT_SCHEMA);

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasIndex(x => x.BikeId);
            builder.HasIndex(x => x.DeliveryRiderId);            
            builder.HasIndex(x => new { x.StartAt, x.EndAt });

            builder.Property(x => x.BikeId).IsRequired();
            builder.Property(x => x.DeliveryRiderId).IsRequired();

            builder.Property(x => x.StartAt).IsRequired();
            builder.Property(x => x.EndAt).IsRequired();
            builder.Property(x => x.ExpectedReturnAt).IsRequired();

            builder.Property(x => x.DailyPriceCents).IsRequired();
            builder.Property(x => x.PriceCents).IsRequired();
            builder.Property(x => x.PenaltyPriceCents);

            builder.Property(o => o.CreatedAt).HasDefaultValueSql("NOW()").ValueGeneratedOnAdd();

            builder.Property(o => o.UpdatedAt).HasDefaultValueSql("NOW()").ValueGeneratedOnAddOrUpdate();
        }
    }
}
