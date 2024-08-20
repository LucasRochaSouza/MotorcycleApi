using Domain.Enums;

namespace Domain.Models.Request
{
    public class CreateDeliveryManRequest
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string LicenseNumber { get; set; }

        public LicenseTypeEnum LicenseType { get; set; }

        public string TaxPayerId { get; set; }
    }
}
