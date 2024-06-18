using MediatR;

namespace BikeRental.Application.Commands.V1.Automated.UpdateDeliveryRequestNotificationStatus
{
    public record UpdateDeliveryRequestNotificationStatusCommand : IRequest
    {
        public Guid DeliveryRequestId { get; init; }
        public long DeliveryRiderId { get; init; }
    }
}
