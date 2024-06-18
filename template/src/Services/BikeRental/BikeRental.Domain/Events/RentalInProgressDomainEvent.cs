using BikeRental.Domain.Models.RentalAggregate;
using MediatR;

namespace BikeRental.Domain.Events
{
    public class RentalInProgressDomainEvent : INotification
    {
        public RentalInProgressDomainEvent(Rental rental)
        {
            Rental = rental;
        }

        public Rental Rental { get; }
    }
}
