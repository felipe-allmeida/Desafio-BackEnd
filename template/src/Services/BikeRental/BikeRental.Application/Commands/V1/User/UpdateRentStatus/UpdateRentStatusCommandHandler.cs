using BikeRental.Domain.Enums;
using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.DeliveryRiderAggregate;
using BikeRental.Domain.Models.RentalAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.User.UpdateRentStatus
{
    public class UpdateRentStatusCommandHandler : IRequestHandler<UpdateRentStatusCommand>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IDeliveryRiderRepository _deliveryRiderRepository;

        public UpdateRentStatusCommandHandler(IRentalRepository rentalRepository, IDeliveryRiderRepository deliveryRiderRepository)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
            _deliveryRiderRepository = deliveryRiderRepository ?? throw new ArgumentNullException(nameof(deliveryRiderRepository));
        }

        public async Task Handle(UpdateRentStatusCommand request, CancellationToken cancellationToken)
        {
            var deliveryRider = await _deliveryRiderRepository.GetByUserIdAsync(request.UserId);
            if (deliveryRider is null)
                throw new UnauthorizedException();

            var rental = await _rentalRepository.GetByIdAsync(deliveryRider.Id, request.RentalId);
            if (rental is null)
                throw new NotFoundException();

            switch (request.Status)
            {
                case ERentalStatus.InProgress:
                    rental.MarkAsInProgress();
                    deliveryRider.AttachBike(rental.BikeId);
                    break;
                case ERentalStatus.Completed:
                    rental.MarkAsCompleted();
                    deliveryRider.DetachBike();
                    break;
                default:
                    throw new DomainException("Invalid status");
            }
        }
    }
}
