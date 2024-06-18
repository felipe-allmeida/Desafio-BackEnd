using BikeRental.Application.DomainEventHandlers.BikeCreated;
using BikeRental.Application.IntegrationEvents;
using BikeRental.Application.IntegrationEvents.Events;
using BikeRental.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BikeRental.Application.DomainEventHandlers.DeliveryRiderCreated
{
    public class DeliveryRiderCreatedDomainEventHandler : INotificationHandler<DeliveryRiderCreatedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IIntegrationEventService _integrationEventService;

        public DeliveryRiderCreatedDomainEventHandler(ILoggerFactory logger, IIntegrationEventService integrationEventService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        public async Task Handle(DeliveryRiderCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<BikeCreatedDomainEventHandler>()
                .LogTrace("DeliveryRider with Id: {DeliveryRider} has ben successfully registered",
                    notification.DeliveryRider.Id);

            var integrationEvent = new DeliveryRiderCreatedIntegrationEvent(notification.DeliveryRider.Id, notification.DeliveryRider.Name, notification.DeliveryRider.Cnpj.Value, notification.DeliveryRider.Birthday);

            await _integrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
