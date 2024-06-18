namespace BikeRental.Application.DTOs.V1
{
    public record BikeDto
    {
        public long Id { get; init; }
        public string Plate { get; init; }
        public string Model { get; init; }
        public int Year { get; init; }
        public bool IsDeleted { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public DateTimeOffset? DeletedAt { get; init; }
    }
}
