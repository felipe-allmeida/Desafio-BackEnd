using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.DeliveryRiderAggregate;
using BikeRental.Domain.ValueObjects;
using MediatR;

namespace BikeRental.Application.Commands.V1.User.CreateDeliveryRider
{
    public class CreateDeliveryRiderCommandHandler : IRequestHandler<CreateDeliveryRiderCommand, DeliveryRider>
    {
        private readonly IDeliveryRiderRepository _repository;

        public CreateDeliveryRiderCommandHandler(IDeliveryRiderRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Domain.Models.DeliveryRiderAggregate.DeliveryRider> Handle(CreateDeliveryRiderCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsByCnpjAsync(request.Cnpj))
                throw new ConflictException($"Delivery Rider with CNPJ '{request.Cnpj}' already exists");

            var cnpj = CNPJ.Parse(request.Cnpj);
            var deliveryRider = new Domain.Models.DeliveryRiderAggregate.DeliveryRider(request.UserId, request.Name, request.Birthday, cnpj);

            _repository.Add(deliveryRider);

            return deliveryRider;
        }
    }
}
