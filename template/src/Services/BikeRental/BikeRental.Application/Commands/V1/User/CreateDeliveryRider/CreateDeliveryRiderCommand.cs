using BikeRental.Domain.Models.DeliveryRiderAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.User.CreateDeliveryRider
{
    public record CreateDeliveryRiderCommand : IRequest<DeliveryRider>
    {
        public string UserId { get; init; }
        public string Name { get; init; }
        public string Cnpj { get; init; }
        public DateTimeOffset Birthday { get; init; }        
    }
}
