using Domain.Models.Request;

namespace Domain.Interfaces.Services
{
    public interface IDeliveryManService
    {
        Task Register(CreateDeliveryManRequest request);

        Task UpdateLicense(string taxPayerId, string filename, Stream licenseImage);

        Task RentMotorcyle(long motorcycleId, string taxPayerId, DateTime endPrevision);

        Task<decimal> CalculateRentValue(string taxPayerId, DateTime endDate);

        Task FinishRent(string taxPayerId);
    }
}
