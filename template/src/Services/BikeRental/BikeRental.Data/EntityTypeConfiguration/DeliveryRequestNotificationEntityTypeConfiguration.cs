using BikeRental.Domain.Models.DeliveryRequestNotificationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRental.Data.EntityTypeConfiguration
{
    public class DeliveryRequestNotificationEntityTypeConfiguration : IEntityTypeConfiguration<DeliveryRequestNotification>
    {
        public void Configure(EntityTypeBuilder<DeliveryRequestNotification> builder)
        {
            builder.ToTable("delivery_request_notifications", BikeRentalContext.DEFAULT_SCHEMA);

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasIndex(x => new { x.DeliveryRiderId, x.DeliveryRequestId }).IsUnique();

            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.DeliveryRequestId).IsRequired();
            builder.Property(x => x.DeliveryRiderId).IsRequired();

            builder.Property(o => o.CreatedAt).HasDefaultValueSql("NOW()").ValueGeneratedOnAdd();
        }
    }
}
