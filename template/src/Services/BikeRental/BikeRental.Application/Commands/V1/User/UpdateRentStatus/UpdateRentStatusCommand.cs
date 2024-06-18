using BikeRental.Domain.Enums;
using MediatR;

namespace BikeRental.Application.Commands.V1.User.UpdateRentStatus
{
    public record UpdateRentStatusCommand : IRequest
    {
        public string UserId { get; init; }
        public ERentalStatus Status { get; init; }
        public Guid RentalId { get; init; }
    }
}
