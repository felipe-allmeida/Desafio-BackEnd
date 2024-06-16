namespace BikeRental.API.DTOs.V1.Responses
{
    public record ErrorResponseDto
    {
        public int StatusCode { get; init; }
        public required string[] Messages { get; init; }
    }
}
