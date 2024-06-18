using BikeRental.Domain.Models.BikeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRental.Data.EntityTypeConfiguration
{
    public class BikeEntityTypeConfiguration : IEntityTypeConfiguration<Bike>
    {
        public void Configure(EntityTypeBuilder<Bike> builder)
        {
            builder.ToTable("bikes", BikeRentalContext.DEFAULT_SCHEMA);

            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasIndex(x => x.Plate).IsUnique();
            builder.HasIndex(x => x.CreatedAt).IsDescending(true);
            builder.HasIndex(x => x.UpdatedAt).IsDescending(true);
            builder.HasIndex(x => x.DeletedAt).IsDescending(true);

            builder.Property(x => x.Plate).IsRequired().HasMaxLength(7);
            builder.Property(x => x.Model).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Year).IsRequired();

            builder.Property(o => o.IsDeleted).HasDefaultValue(false);

            builder.Property(o => o.CreatedAt).HasDefaultValueSql("NOW()").ValueGeneratedOnAdd();

            builder.Property(o => o.UpdatedAt).HasDefaultValueSql("NOW()").ValueGeneratedOnAddOrUpdate();

            builder.Property(o => o.DeletedAt);
        }
    }
}
