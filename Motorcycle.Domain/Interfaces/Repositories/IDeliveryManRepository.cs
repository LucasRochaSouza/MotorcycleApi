using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IDeliveryManRepository
    {
        public Task<int> UpdateLicenseImage(string taxPayerId, string filePath);

        public Task Create(DeliveryMan entity);

        Task<DeliveryMan> GetByTaxPayerId(string taxPayerId);
    }
}
