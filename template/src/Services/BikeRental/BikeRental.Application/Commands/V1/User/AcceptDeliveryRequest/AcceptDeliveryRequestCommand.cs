using MediatR;

namespace BikeRental.Application.Commands.V1.User.AcceptDeliveryRequest
{
    public record AcceptDeliveryRequestCommand : IRequest
    {
        public string UserId { get; init; }
        public Guid DeliveryRequestId { get; init; }
    }
}
