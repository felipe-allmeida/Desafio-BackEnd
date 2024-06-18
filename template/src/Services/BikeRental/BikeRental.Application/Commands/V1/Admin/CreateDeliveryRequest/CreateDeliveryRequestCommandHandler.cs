using BikeRental.Domain.Models.DeliveryRequestAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.Admin.CreateDeliveryRequest
{
    public class CreateDeliveryRequestCommandHandler : IRequestHandler<CreateDeliveryRequestCommand, DeliveryRequest>
    {
        private readonly IDeliveryRequestRepository _repository;

        public CreateDeliveryRequestCommandHandler(IDeliveryRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeliveryRequest> Handle(CreateDeliveryRequestCommand request, CancellationToken cancellationToken)
        {
            var deliveryRequest = new DeliveryRequest(request.PriceCents);

            _repository.Add(deliveryRequest);

            return deliveryRequest;
        }
    }
}
