using Domain.Enums;

namespace Domain.Entities
{
    public class DeliveryMan : BaseEntity
    {
        public string Name { get; set; }

        public string TaxPayerId { get; set; }

        public DateTime BirthDate { get; set; }

        public string LicenseNumber { get; set; }

        public LicenseTypeEnum LicenseType { get; set; }

        public string LicenseImagePath { get; set; }

        public ICollection<Rent> Rents { get; } = new List<Rent>();
    }
}
