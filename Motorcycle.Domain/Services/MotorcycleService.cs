using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models.Dto;
using Domain.Models.Request;
using Domain.Models.Response;

namespace Domain.Services
{
    public class MotorcycleService : IMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        private readonly IRentRepository _rentRepository;

        private readonly IPublisher _publisher;

        public MotorcycleService(
            IMotorcycleRepository motorcycleRepository,
            IRentRepository rentRepository,
            IPublisher publisher)
        {
            _motorcycleRepository = motorcycleRepository;
            _rentRepository = rentRepository;
            _publisher = publisher;
        }

        public async Task CreateMotorcycle(CreateMotorcycleRequest request)
        {
            await _motorcycleRepository.Create(new Motorcycle
            {
                LicensePlate = request.LicensePlate,
                Model = request.Model,
                Year = request.Year,
            });

            _publisher.publish(new CreateMotorcycleDto
            {
                Year = request.Year,
                Model = request.Model,
                LicensePlate = request.LicensePlate,
                ProcessedAt = DateTime.UtcNow,
            },
                "consumer-exchange",
                "consumer." + request.Year);
        }

        public async Task<List<GetMotorcycleResponse>> GetAllMotorcycles()
        {
            var result = await _motorcycleRepository.GetMotorcycles();

            return result
                .Select(x => new GetMotorcycleResponse
                {
                    Id = x.Id,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    Model = x.Model
                })
                .ToList();
        }

        public async Task<GetMotorcycleResponse> GetMotorcycle(string licensePlate)
        {
            var result = await _motorcycleRepository.GetMotorcycle(licensePlate);

            return new GetMotorcycleResponse
            {
                Id = result.Id,
                Year = result.Year,
                LicensePlate = result.LicensePlate,
                Model = result.Model
            };
        }

        public async Task ModifyMotorcycle(long id, string licensePlate)
        {
            var rents = await _rentRepository.GetAll();

            if (rents.Count() > 0)
            {
                throw new KeyNotFoundException($"Error updating motorcycle. No motorcycle found for given id: {id}");
            }

            var deletes = await _motorcycleRepository.UpdateMotorcycle(id, licensePlate);

            if (deletes == 0)
            {
                throw new Exception("Failed to update motorcycle");
            }
        }

        public async Task RemoveMotorcycle(long id)
        {
            var rents = await _rentRepository.GetAll();

            if (rents.Count() > 0)
            {
                throw new KeyNotFoundException($"Error removing motorcycle. No motorcycle found for given id: {id}");
            }

            var deletes = await _motorcycleRepository.DeleteMotorcycle(id);

            if (deletes == 0)
            {
                throw new Exception("Failed to delete motorcycle");
            }
        }
    }
}
