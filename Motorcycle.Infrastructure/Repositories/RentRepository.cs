using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Abstractions;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RentRepository : BaseRepository<Rent>, IRentRepository
    {
        public RentRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public Task<int> FinishCurrentRent(string taxPayerId)
        {
            return _dbSet.Where(x => x.DeliveryMan.TaxPayerId == taxPayerId)
                .ExecuteUpdateAsync(x => x
                .SetProperty(x => x.IsFinished, true)
                .SetProperty(x => x.End, DateTime.Now.ToUniversalTime()));
        }

        public Task<Rent> GetCurrentRunningRent(string taxPayerId)
        {
            return _dbSet.Where(x => x.DeliveryMan.TaxPayerId == taxPayerId).FirstOrDefaultAsync();
        }
    }
}
