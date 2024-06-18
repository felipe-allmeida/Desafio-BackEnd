using BikeRental.Application.DTOs.V1;
using BikeRental.Application.Extensions;
using BikeRental.Domain.Models.RentalAggregate;
using BuildingBlocks.Common;
using MediatR;

namespace BikeRental.Application.Queries.V1.User.GetRentals
{
    public class GetRentalsQueryHandler : IRequestHandler<GetRentalsQuery, PaginatedItem<RentalDto>>
    {
        private readonly IRentalQueryRepository _queryRepository;

        public GetRentalsQueryHandler(IRentalQueryRepository queryRepository)
        {
            _queryRepository = queryRepository ?? throw new ArgumentNullException(nameof(queryRepository));
        }

        public async Task<PaginatedItem<RentalDto>> Handle(GetRentalsQuery request, CancellationToken cancellationToken)
        {
            return await _queryRepository.GetAll()
                .Select(x => new RentalDto
                {
                    Id = x.Id,
                    BikeId = x.BikeId,
                    DeliveryRiderId = x.DeliveryRiderId,
                    Status = x.Status,
                    StartAt = x.StartAt,
                    EndAt = x.EndAt,
                    ExpectedReturnAt = x.ExpectedReturnAt,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DailyPriceCents = x.DailyPriceCents,
                    PenaltyPriceCents = x.PenaltyPriceCents,
                    PriceCents = x.PriceCents
                })
                .OrderByDescending(x => x.CreatedAt)
                .PaginateAsync(request.Skip, request.Take);
        }
    }
}
