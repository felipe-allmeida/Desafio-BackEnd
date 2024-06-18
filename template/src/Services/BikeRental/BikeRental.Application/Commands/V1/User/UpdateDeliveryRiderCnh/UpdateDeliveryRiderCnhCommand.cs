using BikeRental.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BikeRental.Application.Commands.V1.User.UpdateDeliveryRiderCnh
{
    public class UpdateDeliveryRiderCnhCommand : IRequest
    {
        public string UserId { get; init; }
        public ECNHType CnhType { get; init; }
        public string CnhNumber { get; init; }
        public IFormFile CnhImage { get; init; }
    }
}
