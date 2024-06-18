using MediatR;

namespace BikeRental.Application.Commands.V1.User.CompleteDeliveryRequest
{
    public record CompleteDeliveryRequestCommand : IRequest
    {
        public string UserId { get; init; }
    }
}
