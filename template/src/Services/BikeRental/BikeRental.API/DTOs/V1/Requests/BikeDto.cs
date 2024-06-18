namespace BikeRental.API.DTOs.V1.Requests
{    
    public record CreateBikeDto
    {
        public int Year { get; init; }
        public string Model { get; init; }
        public string Plate { get; init; }
    }

    public record UpdateBikePlateDto
    {
        public string Plate { get; init; }
    }
}
