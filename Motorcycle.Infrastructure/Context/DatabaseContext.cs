using Domain.Entities;
using Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<DeliveryMan> DeliveryMen { get; set; }

        public DbSet<RentPlan> RentPlans { get; set; }

        public DbSet<Motorcycle> Motorcycles { get; set; }

        public DbSet<Rent> Rents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new DeliveryManMapping());
            modelBuilder.ApplyConfiguration(new RentPlanMapping());
            modelBuilder.ApplyConfiguration(new MotorcycleMapping());
            modelBuilder.ApplyConfiguration(new RentMapping());
        }
    }
}