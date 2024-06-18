using BikeRental.Domain.Models.DeliveryRequestAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRental.Data.EntityTypeConfiguration
{
    public class DeliveryRequestEntityTypeConfiguration : IEntityTypeConfiguration<DeliveryRequest>
    {
        public void Configure(EntityTypeBuilder<DeliveryRequest> builder)
        {
            builder.ToTable("delivery_request", BikeRentalContext.DEFAULT_SCHEMA);

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasIndex(x => x.Status).IsDescending(false);
            builder.Property(x => x.Status).IsRequired();

            builder.Property(x => x.PriceCents).IsRequired();

            builder.Property(o => o.CreatedAt).HasDefaultValueSql("NOW()").ValueGeneratedOnAdd();

            builder.Property(o => o.UpdatedAt).HasDefaultValueSql("NOW()").ValueGeneratedOnAddOrUpdate();
        }
    }
}
