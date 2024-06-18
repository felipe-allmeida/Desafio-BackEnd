using BikeRental.Application.IntegrationEvents;
using BikeRental.Application.IntegrationEvents.Events;
using BikeRental.Domain.Events;
using BikeRental.Domain.Models.DeliveryRequestNotificationAggregate;
using BikeRental.Domain.Models.DeliveryRiderAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.Automated.NotifyDeliveryRequestToAvaiableDeliveryRiders
{
    public class NotifyDeliveryRequestToAvaiableDeliveryRidersCommandHandler : IRequestHandler<NotifyDeliveryRequestToAvaiableDeliveryRidersCommand>
    {
        private readonly IDeliveryRequestNotificationRepository _deliveryRequestNotificationRepository;
        private readonly IDeliveryRiderQueryRepository _deliveryRiderQueryRepository;

        public NotifyDeliveryRequestToAvaiableDeliveryRidersCommandHandler(IDeliveryRequestNotificationRepository deliveryRequestNotificationRepository, IDeliveryRiderQueryRepository deliveryRiderQueryRepository, IIntegrationEventService integrationEventService)
        {
            _deliveryRequestNotificationRepository = deliveryRequestNotificationRepository ?? throw new ArgumentNullException(nameof(deliveryRequestNotificationRepository));
            _deliveryRiderQueryRepository = deliveryRiderQueryRepository ?? throw new ArgumentNullException(nameof(deliveryRiderQueryRepository));
            
        }

        public async Task Handle(NotifyDeliveryRequestToAvaiableDeliveryRidersCommand request, CancellationToken cancellationToken)
        {
            var deliveryRiders = await _deliveryRiderQueryRepository.GetAvaiableDeliveryRiders();

            foreach (var deliveryRider in deliveryRiders)
            {
                var notification = new DeliveryRequestNotification(request.DeliveryRequestId, deliveryRider.Id);
                _deliveryRequestNotificationRepository.Add(notification);
            }
        }
    }
}
