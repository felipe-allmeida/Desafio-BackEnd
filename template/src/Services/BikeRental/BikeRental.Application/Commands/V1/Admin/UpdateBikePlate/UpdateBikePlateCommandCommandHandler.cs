using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.BikeAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.Admin.UpdateBikePlate
{
    public class UpdateBikePlateCommandCommandHandler(IBikeRepository repository) : IRequestHandler<UpdateBikePlateCommand>
    {
        private readonly IBikeRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task Handle(UpdateBikePlateCommand request, CancellationToken cancellationToken)
        {
            var bike = await _repository.GetByIdAsync(request.Id);

            if (bike is null) throw new NotFoundException();

            if (!await _repository.ExistsByPlateAsync(request.Plate))
                throw new ConflictException($"Bike with {request.Plate} already exists");

            bike.UpdatePlate(request.Plate);

            _repository.Update(bike);
        }
    }
}
