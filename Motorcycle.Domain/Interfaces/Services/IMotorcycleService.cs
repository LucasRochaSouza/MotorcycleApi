using Domain.Models.Request;
using Domain.Models.Response;

namespace Domain.Interfaces.Services
{
    public interface IMotorcycleService
    {
        Task CreateMotorcycle(CreateMotorcycleRequest request);

        Task<GetMotorcycleResponse> GetMotorcycle(string licensePlate);

        Task<List<GetMotorcycleResponse>> GetAllMotorcycles();

        Task ModifyMotorcycle(long id, string licensePlate);

        Task RemoveMotorcycle(long id);
    }
}
