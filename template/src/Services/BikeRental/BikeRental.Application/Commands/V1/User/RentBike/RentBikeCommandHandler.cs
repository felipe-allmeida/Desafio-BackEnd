using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.BikeAggregate;
using BikeRental.Domain.Models.DeliveryRiderAggregate;
using BikeRental.Domain.Models.RentalAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.User.RentBike
{
    public class RentBikeCommandHandler : IRequestHandler<RentBikeCommand, Rental>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBikeQueryRepository _bikeQueryRepository;
        private readonly IDeliveryRiderRepository _deliveryRiderRepository;

        public RentBikeCommandHandler(IRentalRepository rentalRepository, IBikeQueryRepository bikeQueryRepository, IDeliveryRiderRepository deliveryRiderRepository)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
            _bikeQueryRepository = bikeQueryRepository ?? throw new ArgumentNullException(nameof(bikeQueryRepository));
            _deliveryRiderRepository = deliveryRiderRepository ?? throw new ArgumentNullException(nameof(deliveryRiderRepository));
        }

        public async Task<Rental> Handle(RentBikeCommand request, CancellationToken cancellationToken)
        {
            var deliveryRider = await _deliveryRiderRepository.GetByUserIdAsync(request.UserId);

            if (deliveryRider is null)
                throw new UnauthorizedException();

            if (deliveryRider.Cnh.Type == Domain.Enums.ECNHType.B)
                throw new DomainException("Delivery Rider must have CNH type 'A' or 'AB'");

            if (deliveryRider.CurrentBikeId.HasValue)
                throw new DomainException("Delivery Rider has already rented a bike");

            var bike = await _bikeQueryRepository.GetFirstAvailableBikeAsync(request.StartAt, request.EndAt);

            if (bike is null)
                throw new DomainException("No bikes available");

            var rental = new Rental(bike.Id, deliveryRider.Id, request.StartAt, request.EndAt, request.ExpectedReturnAt);

            _rentalRepository.Add(rental);

            return rental;
        }
    }
}
