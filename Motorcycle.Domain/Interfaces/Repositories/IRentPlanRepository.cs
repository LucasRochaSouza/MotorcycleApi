using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IRentPlanRepository
    {
        Task<List<RentPlan>> GetAll();

        Task<RentPlan> GetBydId(long id);

        Task Create(RentPlan entity);

        Task<RentPlan> GetSelectedPlan(DateTime start, DateTime end);
    }
}
