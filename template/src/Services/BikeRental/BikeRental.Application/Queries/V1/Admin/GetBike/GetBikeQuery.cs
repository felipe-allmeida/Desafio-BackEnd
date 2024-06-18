using BikeRental.Application.DTOs.V1;
using MediatR;

namespace BikeRental.Application.Queries.V1.Admin.GetBike
{
    public record GetBikeQuery : IRequest<BikeDto?>
    {
        public long Id { get; init; }
    }
}
