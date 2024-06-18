using BikeRental.Application.DTOs.V1;
using BikeRental.Domain.Models.BikeAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Application.Queries.V1.Admin.GetBike
{
    public class GetBikeQueryHandler : IRequestHandler<GetBikeQuery, BikeDto?>
    {
        private readonly IBikeQueryRepository _queryRepository;

        public GetBikeQueryHandler(IBikeQueryRepository queryRepository)
        {
            _queryRepository = queryRepository ?? throw new ArgumentNullException(nameof(queryRepository));
        }

        public async Task<BikeDto?> Handle(GetBikeQuery request, CancellationToken cancellationToken)
        {
            return await _queryRepository.GetAll()
                .IgnoreQueryFilters()
                .Where(x => x.Id == x.Id)
                .Select(x => new BikeDto
                {
                    Id = x.Id,
                    Plate = x.Plate,
                    Model = x.Model,
                    Year = x.Year,
                    IsDeleted = x.IsDeleted,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                })
                .SingleOrDefaultAsync();
        }
    }
}
