using BikeRental.Domain.Enums;

namespace BikeRental.API.DTOs.V1.Requests
{
    public record RentBikeDto
    {
        public DateTimeOffset StartAt { get; init; }
        public DateTimeOffset EndAt { get; init; }
        public DateTimeOffset ExpectedReturnAt { get; init; }
    }

    public record UpdateRentalDto
    {
        public ERentalStatus Status { get; init; }
    }
}
