using BikeRental.Domain.Models.RentalAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.User.RentBike
{
    public record RentBikeCommand : IRequest<Rental>
    {
        public string UserId { get; init; }
        public DateTimeOffset StartAt { get; init; }
        public DateTimeOffset EndAt { get; init; }
        public DateTimeOffset ExpectedReturnAt { get; init; }
    }
}
