using BikeRental.Domain.Enums;

namespace BikeRental.API.DTOs.V1.Requests
{
    public record CreateDeliveryRiderDto
    {
        public string Name { get; init; }
        public string Cnpj { get; init; }
        public DateTimeOffset Birthday { get; init; }
    }

    public record UpdateDeliveryRiderCnhDto
    {
        public ECNHType Type { get; init; }
        public string Number { get; init; }
        public IFormFile File { get; init; }
    }
}
