namespace Domain.Models.Dto
{
    public class CreateMotorcycleDto
    {
        public uint Year { get; set; }

        public string Model { get; set; }

        public string LicensePlate { get; set; }

        public DateTime ProcessedAt { get; set; }
    }
}
