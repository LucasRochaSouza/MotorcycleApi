namespace Domain.Entities
{
    public class Rent : BaseEntity
    {
        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public DateTime EndPrevision { get; set; }

        public bool IsFinished { get; set; }

        public long DeliveryManId { get; set; }

        public long MotorcycleId { get; set; }

        public long RentPlanId { get; set; }

        public Motorcycle Motorcycle { get; set; }

        public RentPlan RentPlan { get; set; }

        public DeliveryMan DeliveryMan { get; set; }
    }
}
