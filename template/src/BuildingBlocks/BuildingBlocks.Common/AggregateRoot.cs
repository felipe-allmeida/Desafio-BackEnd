using MediatR;

namespace BuildingBlocks.Common
{
    public class AggregateRoot<T> : Entity<T>, IAggregateRoot
    {
        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        protected AggregateRoot() : base()
        {
            _domainEvents ??= new List<INotification>();
        }

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
