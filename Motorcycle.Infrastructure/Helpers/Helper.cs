using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Helpers
{
    public class Helper
    {
        private readonly IRentPlanRepository _rentPlanRepository;

        private List<RentPlan> RentPlans = new()
        {
            new RentPlan
            {
                Days = 7,
                DailyPrice = 30,
                EarlyDeliveryTaxRate = 0.20m,
                LateDeliveryRate = 50
            },
            new RentPlan
            {
                Days = 15,
                DailyPrice = 28,
                EarlyDeliveryTaxRate = 0.40m,
                LateDeliveryRate = 50
            },
            new RentPlan
            {
                Days = 30,
                DailyPrice = 22,
                EarlyDeliveryTaxRate = 0,
                LateDeliveryRate = 50
            },
            new RentPlan
            {
                Days = 45,
                DailyPrice = 20,
                EarlyDeliveryTaxRate = 0,
                LateDeliveryRate = 50
            },
            new RentPlan
            {
                Days = 50,
                DailyPrice = 18,
                EarlyDeliveryTaxRate = 0,
                LateDeliveryRate = 50
            }
        };

        public Helper(IRentPlanRepository rentPlanRepository)
        {
            _rentPlanRepository = rentPlanRepository;
        }

        public async Task CreateInitialPlans()
        {
            foreach (var rentPlan in RentPlans)
            {
                await _rentPlanRepository.Create(rentPlan);
            }
        }
    }
}
