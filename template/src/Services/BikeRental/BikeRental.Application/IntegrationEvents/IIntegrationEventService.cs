using BikeRental.CrossCutting.EventBus.Events;
using BikeRental.CrossCutting.EventBusRabbitMQ;

namespace BikeRental.Application.IntegrationEvents
{
    public interface IIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
