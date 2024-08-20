using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Abstractions;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DeliveryManRepository : BaseRepository<DeliveryMan>, IDeliveryManRepository
    {
        public DeliveryManRepository(DatabaseContext context) : base(context) { }

        public Task<int> UpdateLicenseImage(string taxPayerId, string filePath)
        {
            return _dbSet.Where(x => x.TaxPayerId == taxPayerId).ExecuteUpdateAsync(x => x.SetProperty(x => x.LicenseImagePath, filePath));
        }

        public Task<bool> IsAbleToRent(string taxPayer)
        {
            return _dbSet
                .Where(x => x.TaxPayerId == taxPayer)
                .Select(
                    x => x.LicenseType == LicenseTypeEnum.B ||
                    x.LicenseType == LicenseTypeEnum.AB)
                .FirstOrDefaultAsync();
        }

        public Task<DeliveryMan> GetByTaxPayerId(string taxPayerId)
        {
            return _dbSet
                .Where(x => x.TaxPayerId == taxPayerId)
                .FirstOrDefaultAsync();
        }
    }
}