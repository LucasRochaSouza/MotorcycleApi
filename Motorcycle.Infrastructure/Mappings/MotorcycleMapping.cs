using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class MotorcycleMapping : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder.ToTable(typeof(Motorcycle).Name.ToLowerInvariant());
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.LicensePlate).IsUnique(true);

            builder.Property(x => x.Year).IsRequired(true);
            builder.Property(x => x.Model).IsRequired(true);
            builder.Property(x => x.LicensePlate).IsRequired(true);
        }
    }
}