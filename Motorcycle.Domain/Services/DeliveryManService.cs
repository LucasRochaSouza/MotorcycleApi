using Domain.CustomExceptions;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models.Request;

namespace Domain.Services
{
    public class DeliveryManService : IDeliveryManService
    {
        private readonly IRentRepository _rentRepository;

        private readonly IDeliveryManRepository _deliveryManRepository;

        private readonly IRentPlanRepository _rentPlanRepository;

        private readonly IMotorcycleRepository _motorcycleRepository;

        public DeliveryManService(
            IRentRepository rentRepository,
            IDeliveryManRepository deliveryManRepository,
            IRentPlanRepository rentPlanRepository,
            IMotorcycleRepository motorcycleRepository)
        {
            _rentRepository = rentRepository;
            _deliveryManRepository = deliveryManRepository;
            _rentPlanRepository = rentPlanRepository;
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<decimal> CalculateRentValue(string taxPayerId, DateTime endDate)
        {
            var currentRent = await _rentRepository.GetCurrentRunningRent(taxPayerId);

            if (currentRent.Start > endDate)
            {
                throw new UserException("Cannot get rent value if start date is lower then end date");
            }

            var rentPlan = await _rentPlanRepository.GetBydId(currentRent.RentPlanId);

            if (rentPlan == null)
            {
                throw new Exception("No selected rent plan found");
            }

            var runningDays = (endDate - currentRent.Start).Days;
            var expectedDays = (currentRent.EndPrevision - currentRent.Start).Days;

            decimal totalValue = 0;

            if (runningDays > expectedDays)
            {
                totalValue += (rentPlan.LateDeliveryRate + rentPlan.DailyPrice) * (runningDays - expectedDays);
            }

            if (runningDays < expectedDays)
            {
                totalValue += rentPlan.EarlyDeliveryTaxRate * (expectedDays - runningDays);
            }

            totalValue += rentPlan.DailyPrice * runningDays;

            return totalValue;
        }

        public Task Register(CreateDeliveryManRequest request)
        {
            return _deliveryManRepository.Create(
                new DeliveryMan
                {
                    Name = request.Name,
                    BirthDate = request.BirthDate,
                    LicenseNumber = request.LicenseNumber,
                    LicenseType = request.LicenseType,
                    TaxPayerId = request.TaxPayerId
                });
        }

        public async Task RentMotorcyle(long motorcycleId, string taxPayerId, DateTime endPrevision)
        {
            var deliveryMan = await _deliveryManRepository.GetByTaxPayerId(taxPayerId);

            if (!await _motorcycleRepository.IsMotocycleAvailable(motorcycleId))
            {
                throw new UserException("Motorcycle not available");
            }

            if (!(deliveryMan.LicenseType == LicenseTypeEnum.A || deliveryMan.LicenseType == LicenseTypeEnum.AB))
            {
                throw new UserException("Invalid license type");
            }

            var rentPlan = await _rentPlanRepository.GetSelectedPlan(DateTime.Now, endPrevision);

            if (rentPlan == null)
            {
                throw new Exception("No selected rent plan found");
            }

            await _rentRepository.Create(new Rent
            {
                MotorcycleId = motorcycleId,
                DeliveryManId = deliveryMan.Id,
                RentPlanId = rentPlan.Id,
                EndPrevision = endPrevision,
                Start = DateTime.Now.ToUniversalTime().AddDays(1),
            });
        }

        public async Task UpdateLicense(string taxPayerId, string filename, Stream licenseImage)
        {
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Licenses");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = $"{taxPayerId}_{filename}";
            var filePath = Path.Combine(directoryPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                licenseImage.Seek(0, SeekOrigin.Begin);
                await licenseImage.CopyToAsync(fileStream);
            }

            var updatedRows = await _deliveryManRepository.UpdateLicenseImage(taxPayerId, fileName);

            if (updatedRows < 1)
            {
                throw new Exception($"Failure updating license plate for tax payer id: {taxPayerId}");
            }
        }

        public async Task FinishRent(string taxPayerId)
        {
            var updatedRows = await _rentRepository.FinishCurrentRent(taxPayerId);

            if (updatedRows < 1)
            {
                throw new Exception($"Failure finishing rent for motorcycle tax payer id: {taxPayerId}");
            }
        }
    }
}
