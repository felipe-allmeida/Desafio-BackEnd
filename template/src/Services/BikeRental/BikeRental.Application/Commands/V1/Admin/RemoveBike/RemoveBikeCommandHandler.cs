using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.BikeAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.Admin.RemoveBike
{
    public class RemoveBikeCommandHandler(IBikeRepository repository) : IRequestHandler<RemoveBikeCommand>
    {
        private readonly IBikeRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task Handle(RemoveBikeCommand request, CancellationToken cancellationToken)
        {
            var bike = await _repository.GetByIdAsync(request.Id);

            if (bike is null) throw new NotFoundException();

            bike.MarkAsDeleted();

            _repository.Update(bike);
        }
    }
}
