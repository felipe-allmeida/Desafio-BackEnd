using BikeRental.Application.DTOs.V1;
using BikeRental.Domain.Models.RentalAggregate;
using MediatR;

namespace BikeRental.Application.Queries.V1.User.GetRentBikeInfo
{
    public class GetRentBikeInfoQueryHandler : IRequestHandler<GetRentBikeInfoQuery, RentalInfoDto>
    {
        public async Task<RentalInfoDto> Handle(GetRentBikeInfoQuery request, CancellationToken cancellationToken)
        {
            var rental = new Rental(request.StartAt, request.EndAt, request.ExpectedReturnAt);
            return await Task.FromResult(new RentalInfoDto
            {
                StartAt = rental.StartAt,
                EndAt = rental.EndAt,
                ExpectedReturnAt = rental.ExpectedReturnAt,
                DailyPriceCents = rental.DailyPriceCents,
                PriceCents = rental.PriceCents,
                PenaltyPriceCents = rental.PenaltyPriceCents
            });
        }
    }
}
