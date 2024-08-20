using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class DeliveryManMapping : IEntityTypeConfiguration<DeliveryMan>
    {
        public void Configure(EntityTypeBuilder<DeliveryMan> builder)
        {
            builder.ToTable(typeof(DeliveryMan).Name.ToLowerInvariant());
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.HasIndex(x => x.LicenseNumber).IsUnique(true);
            builder.HasIndex(x => x.TaxPayerId).IsUnique(true);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.LicenseType).IsRequired(true);
            builder.Property(x => x.LicenseNumber).IsRequired(true);
            builder.Property(x => x.BirthDate).IsRequired(true);
            builder.Property(x => x.TaxPayerId).IsRequired(true);
            builder.Property(x => x.LicenseImagePath);
        }
    }
}