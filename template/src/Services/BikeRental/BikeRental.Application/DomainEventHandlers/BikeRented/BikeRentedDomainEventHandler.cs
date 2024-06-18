using BikeRental.Application.IntegrationEvents;
using BikeRental.Application.IntegrationEvents.Events;
using BikeRental.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BikeRental.Application.DomainEventHandlers.BikeRented
{
    public class BikeRentedDomainEventHandler : INotificationHandler<BikeRentedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IIntegrationEventService _integrationEventService;

        public BikeRentedDomainEventHandler(ILoggerFactory logger, IIntegrationEventService integrationEventService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        public async Task Handle(BikeRentedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<BikeRentedDomainEventHandler>()
                .LogTrace("DeliveryRider with Id {DeliveryRiderId} successfully rented Bike with Id: {BikeId}",
                                   notification.Rental.DeliveryRiderId, notification.Rental.BikeId);

            var integrationEvent = new BikeRentedIntegrationEvent(
                notification.Rental.Id,
                notification.Rental.BikeId,
                notification.Rental.DeliveryRiderId,
                notification.Rental.Status,
                notification.Rental.CreatedAt,
                notification.Rental.StartAt,
                notification.Rental.EndAt,
                notification.Rental.ExpectedReturnAt,
                notification.Rental.DailyPriceCents,
                notification.Rental.PriceCents,
                notification.Rental.PenaltyPriceCents);

            await _integrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
