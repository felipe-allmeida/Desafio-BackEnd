using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.BikeAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.Admin.CreateBike
{
    public class CreateBikeCommandHandler : IRequestHandler<CreateBikeCommand, Bike>
    {
        private readonly IBikeRepository _repository;

        public CreateBikeCommandHandler(IBikeRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Bike> Handle(CreateBikeCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsByPlateAsync(request.Plate))
                throw new ConflictException($"Bike with plate '{request.Plate}' already exists");

            var bike = new Bike(request.Plate, request.Year, request.Model);

            _repository.Add(bike);

            return bike;
        }
    }
}
