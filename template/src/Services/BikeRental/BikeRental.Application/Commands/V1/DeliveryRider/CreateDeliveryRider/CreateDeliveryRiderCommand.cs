using BikeRental.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BikeRental.Application.Commands.V1.DeliveryRider.CreateDeliveryRider
{
    public record CreateDeliveryRiderCommand : IRequest<Domain.Models.DeliveryRiderAggregate.DeliveryRider>
    {
        public string Name { get; init; }
        public string Cnpj { get; init; }
        public DateTimeOffset Birthday { get; init; }
        public ECNHType ECNHType { get; init; }
        public string CnhNumber { get; init; }
        public IFormFile CnhImage { get; init; }
    }
}
