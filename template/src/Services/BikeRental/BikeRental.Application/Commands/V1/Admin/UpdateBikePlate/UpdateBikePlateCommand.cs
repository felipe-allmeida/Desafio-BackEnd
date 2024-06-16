using MediatR;

namespace BikeRental.Application.Commands.V1.Admin.UpdateBikePlate
{
    public record UpdateBikePlateCommand : IRequest
    {
        public long Id { get; init; }
        public string Plate { get; init; }
    }
}
