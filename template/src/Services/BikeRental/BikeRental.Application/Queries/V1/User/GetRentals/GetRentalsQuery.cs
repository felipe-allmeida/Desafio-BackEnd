using BikeRental.Application.DTOs.V1;
using BuildingBlocks.Common;
using MediatR;

namespace BikeRental.Application.Queries.V1.User.GetRentals
{
    public record GetRentalsQuery : IRequest<PaginatedItem<RentalDto>>
    {
        public int Skip { get; init; }
        public int Take { get; init; } = 10;
    }
}
