using BikeRental.Application.IntegrationEvents;
using BikeRental.Application.IntegrationEvents.Events;
using BikeRental.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BikeRental.Application.DomainEventHandlers.DeliveryRequestNotificationCreated
{
    public class DeliveryRequestNotificationCreatedDomainEventHandler : INotificationHandler<DeliveryRequestNotificationCreatedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IIntegrationEventService _integrationEventService;

        public DeliveryRequestNotificationCreatedDomainEventHandler(ILoggerFactory logger, IIntegrationEventService integrationEventService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        public async Task Handle(DeliveryRequestNotificationCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<DeliveryRequestNotificationCreatedDomainEventHandler>()
                .LogTrace("DeliveryRequestNotification with Id: {DeliveryRequestNotification} has ben successfully registered",
                                   notification.Notification.Id);

            var integrationEvent = new DeliveryRequestNotificationCreatedIntegrationEvent(notification.Notification.DeliveryRequestId, notification.Notification.DeliveryRiderId);
            await _integrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
