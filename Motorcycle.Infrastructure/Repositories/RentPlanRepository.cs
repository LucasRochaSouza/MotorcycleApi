using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Abstractions;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RentPlanRepository : BaseRepository<RentPlan>, IRentPlanRepository
    {
        public RentPlanRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }

        public Task<RentPlan> GetSelectedPlan(DateTime start, DateTime end)
        {
            if (end < start)
            {
                return null;
            }

            var totalDays = (end - start).Days;

            return _dbSet
                .Where(rp => rp.Days <= totalDays)
                .OrderByDescending(rp => rp.Days)
                .FirstOrDefaultAsync();
        }
    }
}