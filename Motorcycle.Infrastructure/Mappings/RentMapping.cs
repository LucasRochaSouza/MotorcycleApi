using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class RentMapping : IEntityTypeConfiguration<Rent>
    {
        public void Configure(EntityTypeBuilder<Rent> builder)
        {
            builder.ToTable(typeof(Rent).Name.ToLowerInvariant());
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.HasKey(x => x.Id);

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Start)
                .IsRequired();

            builder.Property(r => r.End)
                .IsRequired(false);
            builder.Property(r => r.EndPrevision)
                .IsRequired();
            builder.Property(r => r.IsFinished)
                .IsRequired()
                .HasDefaultValue(false);
            builder.Property(r => r.DeliveryManId)
                .IsRequired();
            builder.Property(r => r.MotorcycleId)
                .IsRequired();
            builder.Property(r => r.RentPlanId)
                .IsRequired();

            builder.HasOne(r => r.Motorcycle)
                .WithMany(x => x.Rents)
                .HasForeignKey(r => r.MotorcycleId);

            builder.HasOne(r => r.RentPlan)
                .WithMany(x => x.Rents)
                .HasForeignKey(r => r.RentPlanId);

            builder.HasOne(r => r.DeliveryMan)
                .WithMany(x => x.Rents)
                .HasForeignKey(r => r.DeliveryManId);
        }
    }
}
