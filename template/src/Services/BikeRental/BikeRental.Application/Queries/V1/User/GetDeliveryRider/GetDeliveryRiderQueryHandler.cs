using BikeRental.Application.DTOs.V1;
using BikeRental.Domain.Models.DeliveryRiderAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Application.Queries.V1.User.GetDeliveryRider
{
    public class GetDeliveryRiderQueryHandler : IRequestHandler<GetDeliveryRiderQuery, DeliveryRiderDto?>
    {
        private readonly IDeliveryRiderQueryRepository _queryRepository;

        public GetDeliveryRiderQueryHandler(IDeliveryRiderQueryRepository queryRepository)
        {
            _queryRepository = queryRepository ?? throw new ArgumentNullException(nameof(queryRepository));
        }

        public async Task<DeliveryRiderDto?> Handle(GetDeliveryRiderQuery request, CancellationToken cancellationToken)
        {
            return await _queryRepository.GetAll()
                .Where(x => x.UserId == request.UserId)
                .Select(x => new DeliveryRiderDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Cnpj = x.Cnpj.Value,
                    Cnh = x.Cnh != null
                        ? new CnhDto
                        {
                            Number = x.Cnh.Number,
                            Type = x.Cnh.Type,
                            Image = x.Cnh.Image
                        }
                        : null,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt

                }).SingleOrDefaultAsync();
        }
    }
}
