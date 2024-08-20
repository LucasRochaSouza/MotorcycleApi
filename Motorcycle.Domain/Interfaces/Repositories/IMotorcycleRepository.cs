using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IMotorcycleRepository
    {
        Task<List<Motorcycle>> GetMotorcycles();

        Task<Motorcycle> GetMotorcycle(string licensePlate);

        Task<int> UpdateMotorcycle(long id, string licensePlate);

        Task<int> DeleteMotorcycle(long id);

        Task Create(Motorcycle entity);

        Task<bool> IsMotocycleAvailable(long id);
    }
}
