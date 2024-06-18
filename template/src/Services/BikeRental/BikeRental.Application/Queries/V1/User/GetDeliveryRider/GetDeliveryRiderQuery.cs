using BikeRental.Application.DTOs.V1;
using MediatR;

namespace BikeRental.Application.Queries.V1.User.GetDeliveryRider
{
    public record GetDeliveryRiderQuery : IRequest<DeliveryRiderDto?>
    {
        public string UserId { get; init; }
    }
}
