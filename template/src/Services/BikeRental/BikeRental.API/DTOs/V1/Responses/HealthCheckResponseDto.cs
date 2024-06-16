namespace BikeRental.API.DTOs.V1.Responses
{
    public record HealthCheckResponseDto
    {
        public required string Status { get; init; }
        public required string TotalDuration { get; init; }
        public IReadOnlyDictionary<string, HealthCheckResultDto> Results { get; init; } = new Dictionary<string, HealthCheckResultDto>();
    }

    public record HealthCheckResultDto
    {
        public required string Status { get; init; }

        public required string Duration { get; init; }

        public string? Description { get; init; }

        public required string ExceptionMessage { get; init; }

        public IReadOnlyDictionary<string, object> Data { get; init; } = new Dictionary<string, object>();
    }
}
