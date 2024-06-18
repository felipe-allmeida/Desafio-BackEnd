using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.BikeAggregate;
using BikeRental.Domain.Models.RentalAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.Admin.RemoveBike
{
    public class RemoveBikeCommandHandler : IRequestHandler<RemoveBikeCommand>
    {
        private readonly IBikeRepository _repository;
        private readonly IRentalRepository _rentalRepository;

        public RemoveBikeCommandHandler(IBikeRepository repository, IRentalRepository rentalRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
        }

        public async Task Handle(RemoveBikeCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.BikeHasRentals(request.Id)) 
                throw new ConflictException("Cannot remove bike with rentals");

            var bike = await _repository.GetByIdAsync(request.Id);

            if (bike is null) throw new NotFoundException();

            if (await _rentalRepository.ExistsRentalForBike(bike.Id))
                throw new ConflictException("Cannot remove bike with rentals");

            bike.MarkAsDeleted();

            _repository.Update(bike);
        }
    }
}
