using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class RentPlanMapping : IEntityTypeConfiguration<RentPlan>
    {
        public void Configure(EntityTypeBuilder<RentPlan> builder)
        {
            builder.ToTable(typeof(RentPlan).Name.ToLowerInvariant());
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Days).IsRequired(true);
            builder.Property(x => x.DailyPrice).IsRequired(true);
            builder.Property(x => x.EarlyDeliveryTaxRate).IsRequired(true);
            builder.Property(x => x.LateDeliveryRate).IsRequired(true);
        }
    }
}