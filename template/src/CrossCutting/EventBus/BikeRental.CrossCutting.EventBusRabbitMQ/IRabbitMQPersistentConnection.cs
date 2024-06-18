using RabbitMQ.Client;

namespace BikeRental.CrossCutting.EventBusRabbitMQ
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();

    }

}
