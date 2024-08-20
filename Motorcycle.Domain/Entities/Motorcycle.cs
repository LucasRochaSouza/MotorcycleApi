namespace Domain.Entities
{
    public class Motorcycle : BaseEntity
    {
        public uint Year { get; set; }

        public string Model { get; set; }

        public string LicensePlate { get; set; }

        public IEnumerable<Rent> Rents { get; set; }
    }
}
