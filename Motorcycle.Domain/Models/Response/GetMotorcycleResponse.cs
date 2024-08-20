namespace Domain.Models.Response
{
    public class GetMotorcycleResponse
    {
        public long Id { get; set; }

        public uint Year { get; set; }

        public string Model { get; set; }

        public string LicensePlate { get; set; }
    }
}