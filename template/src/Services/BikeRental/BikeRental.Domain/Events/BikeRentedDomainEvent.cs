using BikeRental.Domain.Models.RentalAggregate;
using MediatR;

namespace BikeRental.Domain.Events
{
    public class BikeRentedDomainEvent : INotification
    {
        public BikeRentedDomainEvent(Rental rental)
        {
            Rental = rental;
        }

        public Rental Rental { get; }
    }
}
