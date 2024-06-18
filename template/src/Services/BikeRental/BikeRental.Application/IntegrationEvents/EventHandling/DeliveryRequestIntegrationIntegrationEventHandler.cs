using BikeRental.Application.Commands.V1.Automated.NotifyDeliveryRequestToAvaiableDeliveryRiders;
using BikeRental.Application.Commands.V1.Automated.UpdateDeliveryRequestNotificationStatus;
using BikeRental.Application.IntegrationEvents.Events;
using BikeRental.CrossCutting.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BikeRental.Application.IntegrationEvents.EventHandling
{
    public class DeliveryRequestIntegrationIntegrationEventHandler :
           IIntegrationEventHandler<DeliveryRequestCreatedIntegrationEvent>,
           IIntegrationEventHandler<DeliveryRequestNotificationCreatedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DeliveryRequestIntegrationIntegrationEventHandler> _logger;

        public DeliveryRequestIntegrationIntegrationEventHandler(IMediator mediator, ILogger<DeliveryRequestIntegrationIntegrationEventHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(DeliveryRequestCreatedIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "BikeRental.API", @event);

            await _mediator.Send(new NotifyDeliveryRequestToAvaiableDeliveryRidersCommand
            {
                DeliveryRequestId = @event.DeliveryRequestId,
                PriceCents = @event.PriceCents,
                Status = @event.Status,
            });
        }

        public async Task Handle(DeliveryRequestNotificationCreatedIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "BikeRental.API", @event);

            await _mediator.Send(new UpdateDeliveryRequestNotificationStatusCommand
            {
                DeliveryRequestId = @event.DeliveryRequestId,
                DeliveryRiderId = @event.DeliveryRiderId,
            });
        }
    }
}
