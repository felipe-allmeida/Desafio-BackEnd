using BikeRental.Domain.Enums;

namespace BikeRental.Application.DTOs.V1
{
    public record CnhDto
    {
        public ECNHType Type { get; init; }
        public string Number { get; init; }
        public string Image { get; init; }
    }
}
