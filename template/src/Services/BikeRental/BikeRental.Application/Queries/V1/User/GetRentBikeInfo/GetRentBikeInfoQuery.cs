using BikeRental.Application.DTOs.V1;
using MediatR;

namespace BikeRental.Application.Queries.V1.User.GetRentBikeInfo
{
    public record GetRentBikeInfoQuery : IRequest<RentalInfoDto>
    {
        public DateTimeOffset StartAt { get; init; }
        public DateTimeOffset EndAt { get; init; }
        public DateTimeOffset ExpectedReturnAt { get; init; }
    }
}
