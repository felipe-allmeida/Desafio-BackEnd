using BikeRental.Domain.Models.DeliveryRiderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRental.Data.EntityTypeConfiguration
{
    public class DeliveryRiderEntityTypeConfiguration : IEntityTypeConfiguration<DeliveryRider>
    {
        public void Configure(EntityTypeBuilder<DeliveryRider> builder)
        {
            builder.ToTable("delivery_riders", BikeRentalContext.DEFAULT_SCHEMA);

            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasIndex(x => x.UserId).IsUnique(true);
            builder.Property(x => x.UserId).HasMaxLength(450).IsRequired();

            builder.HasIndex(x => x.CreatedAt).IsDescending(true);
            builder.HasIndex(x => x.UpdatedAt).IsDescending(true);
            builder.HasIndex(x => x.DeletedAt).IsDescending(true);

            builder.Property(o => o.IsDeleted).HasDefaultValue(false);

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Birthday).IsRequired();

            builder.OwnsOne(x => x.Cnpj, ownedBuilder =>
            {
                ownedBuilder.Property(y => y.Value).HasMaxLength(18).IsRequired();
                ownedBuilder.HasIndex(y => y.Value).IsUnique(true);
            });

            builder.OwnsOne(x => x.Cnh, ownedBuilder =>
            {
                ownedBuilder.Property(y => y.Type);

                ownedBuilder.HasIndex(y => y.Number).IsUnique(true);
                ownedBuilder.Property(y => y.Number).HasMaxLength(11);

                ownedBuilder.Property(y => y.Image);
            });

            builder.Property(o => o.CreatedAt).HasDefaultValueSql("NOW()").ValueGeneratedOnAdd();

            builder.Property(o => o.UpdatedAt).HasDefaultValueSql("NOW()").ValueGeneratedOnAddOrUpdate();

            builder.Property(o => o.DeletedAt);
        }
    }
}
