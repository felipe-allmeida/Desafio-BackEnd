namespace BikeRental.API.DTOs.V1.Requests
{
    public record FileDto
    {
        public IFormFile? File { get; init; }
    }
}
