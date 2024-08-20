namespace Domain.Models.Request
{
    public class CreateMotorcycleRequest
    {
        public uint Year { get; set; }

        public string Model { get; set; }

        public string LicensePlate { get; set; }
    }
}