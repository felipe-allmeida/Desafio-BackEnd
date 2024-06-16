using BikeRental.Domain.Models.BikeAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.Admin.CreateBike
{
    public record CreateBikeCommand : IRequest<Bike>
    {
        public int Year { get; init; }
        public string Model { get; init; } = string.Empty;
        public string Plate { get; init; } = string.Empty;
    }
}
