namespace BikeRental.API.DTOs.V1.Requests
{
    public record CreateDeliveryRequestDto
    {
        public int PriceCents { get; init; }
    }
}
