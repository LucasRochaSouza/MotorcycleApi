namespace Domain.Entities
{
    public class RentPlan : BaseEntity
    {
        public uint Days { get; set; }

        public decimal DailyPrice { get; set; }

        public decimal EarlyDeliveryTaxRate { get; set; }

        public decimal LateDeliveryRate { get; set; }

        public ICollection<Rent> Rents { get; } = new List<Rent>();
    }
}
