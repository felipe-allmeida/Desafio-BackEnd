using BikeRental.Domain.Models.DeliveryRequestAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.Admin.CreateDeliveryRequest
{
    public record CreateDeliveryRequestCommand : IRequest<DeliveryRequest>
    {
        public int PriceCents { get; init; }
    }
}
