using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IRentRepository
    {
        Task<List<Rent>> GetAll();

        Task<int> FinishCurrentRent(string taxPayerId);

        Task<Rent> GetCurrentRunningRent(string taxPayerId);

        Task Create(Rent entity);
    }
}
