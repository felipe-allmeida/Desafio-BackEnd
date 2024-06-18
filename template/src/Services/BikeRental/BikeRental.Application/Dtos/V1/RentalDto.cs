using BikeRental.Domain.Enums;

namespace BikeRental.Application.DTOs.V1
{
    public record RentalDto
    {
        public Guid Id { get; init; }
        public long BikeId { get; init; }
        public long DeliveryRiderId { get; init; }
        public ERentalStatus Status { get; init; }
        public DateTimeOffset StartAt { get; init; }
        public DateTimeOffset EndAt { get; init; }
        public DateTimeOffset ExpectedReturnAt { get; init; }
        public int DailyPriceCents { get; init; }
        public decimal PriceCents { get; init; }
        public int? PenaltyPriceCents { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
    }

    public record RentalInfoDto
    {
        public DateTimeOffset StartAt { get; init; }
        public DateTimeOffset EndAt { get; init; }
        public DateTimeOffset ExpectedReturnAt { get; init; }
        public int DailyPriceCents { get; init; }
        public decimal PriceCents { get; init; }
        public int? PenaltyPriceCents { get; init; }
    }
}
