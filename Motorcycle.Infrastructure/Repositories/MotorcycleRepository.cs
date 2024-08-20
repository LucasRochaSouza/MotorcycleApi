using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Abstractions;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MotorcycleRepository : BaseRepository<Motorcycle>, IMotorcycleRepository
    {
        public MotorcycleRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }

        public Task<List<Motorcycle>> GetMotorcycles()
        {
            return _dbSet.ToListAsync();
        }

        public Task<Motorcycle> GetMotorcycle(string licensePlate)
        {
            return _dbSet.Where(x => x.LicensePlate == licensePlate).FirstOrDefaultAsync();
        }

        public Task<int> UpdateMotorcycle(long id, string licensePlate)
        {
            return _dbSet.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(x => x.LicensePlate, licensePlate));
        }

        public Task<int> DeleteMotorcycle(long id)
        {
            return _dbSet.Where(x => x.Rents.Count() == 0 && x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<bool> IsMotocycleAvailable(long id)
        {
            var motorcycleExists = await _dbSet.AnyAsync(x => x.Id == id);
            if (!motorcycleExists)
            {
                return false;
            }

            return await _dbSet
                .Where(x => x.Id == id)
                .AllAsync(x => x.Rents.All(rent => rent.IsFinished == true));
        }
    }
}