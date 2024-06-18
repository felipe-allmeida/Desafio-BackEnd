using BikeRental.Application.IntegrationEvents;
using BikeRental.Application.IntegrationEvents.Events;
using BikeRental.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BikeRental.Application.DomainEventHandlers.DeliveryRequestCreated
{
    public class DelievryRequestCreatedDomainEventHandler : INotificationHandler<DeliveryRequestCreatedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IIntegrationEventService _integrationEventService;

        public DelievryRequestCreatedDomainEventHandler(ILoggerFactory logger, IIntegrationEventService integrationEventService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        public async Task Handle(DeliveryRequestCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<DeliveryRequestCreatedDomainEvent>()
                .LogTrace("DeliveryRequest with Id: {DeliveryRequest} has ben successfully registered",
                    notification.DeliveryRequest.Id);

            var integrationEvent = new DeliveryRequestCreatedIntegrationEvent(
                notification.DeliveryRequest.Id, 
                (int)notification.DeliveryRequest.Status, 
                notification.DeliveryRequest.PriceCents, 
                notification.DeliveryRequest.CreatedAt);

            await _integrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
