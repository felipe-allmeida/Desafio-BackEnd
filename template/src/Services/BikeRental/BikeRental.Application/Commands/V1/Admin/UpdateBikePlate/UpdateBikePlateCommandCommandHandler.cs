using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.BikeAggregate;
using BikeRental.Domain.Models.RentalAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.Admin.UpdateBikePlate
{
    public class UpdateBikePlateCommandCommandHandler : IRequestHandler<UpdateBikePlateCommand>
    {
        private readonly IBikeRepository _repository;

        public UpdateBikePlateCommandCommandHandler(IBikeRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(UpdateBikePlateCommand request, CancellationToken cancellationToken)
        {
            var bike = await _repository.GetByIdAsync(request.Id);

            if (bike is null) throw new NotFoundException();

            if (await _repository.ExistsByPlateAsync(request.Plate))
                throw new ConflictException($"Bike with plate '{request.Plate}' already exists");

            bike.UpdatePlate(request.Plate);

            _repository.Update(bike);
        }
    }
}
