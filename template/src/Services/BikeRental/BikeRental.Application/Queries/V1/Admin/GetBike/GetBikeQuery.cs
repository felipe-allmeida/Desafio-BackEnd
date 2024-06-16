using BikeRental.Application.DTOs.V1;
using BikeRental.Domain.Models.BikeAggregate;
using MediatR;

namespace BikeRental.Application.Queries.V1.Admin.GetBike
{
    public record GetBikeQuery(IBikeQueryRepository queryRepository) : IRequest<BikeDto?>
    {
        public long Id { get; init; }
    }
}
