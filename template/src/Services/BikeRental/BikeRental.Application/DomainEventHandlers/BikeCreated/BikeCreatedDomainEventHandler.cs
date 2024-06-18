using BikeRental.Application.IntegrationEvents;
using BikeRental.Application.IntegrationEvents.Events;
using BikeRental.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BikeRental.Application.DomainEventHandlers.BikeCreated
{
    public class BikeCreatedDomainEventHandler : INotificationHandler<BikeCreatedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IIntegrationEventService _integrationEventService;

        public BikeCreatedDomainEventHandler(ILoggerFactory logger, IIntegrationEventService integrationEventService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        public async Task Handle(BikeCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<BikeCreatedDomainEventHandler>()
                .LogTrace("Bike with Id: {BikeId} has ben successfully registered",
                    notification.Bike.Id);

            var integrationEvent = new BikeCreatedIntegrationEvent(notification.Bike.Id, notification.Bike.Plate, notification.Bike.Year, notification.Bike.Model);

            await _integrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
