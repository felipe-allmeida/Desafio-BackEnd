using MediatR;

namespace BuildingBlocks.Common
{
    public interface IAggregateRoot
    {
        IReadOnlyCollection<INotification> DomainEvents { get; }
        void AddDomainEvent(INotification eventItem);
        void RemoveDomainEvent(INotification eventItem);
        void ClearDomainEvents();
    }
}
