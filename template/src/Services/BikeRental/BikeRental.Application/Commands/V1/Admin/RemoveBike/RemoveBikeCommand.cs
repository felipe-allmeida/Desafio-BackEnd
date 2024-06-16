using MediatR;

namespace BikeRental.Application.Commands.V1.Admin.RemoveBike
{
    public record RemoveBikeCommand : IRequest
    {
        public long Id { get; init; }
    }
}
