using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.DeliveryRequestAggregate;
using BikeRental.Domain.Models.DeliveryRiderAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.User.CompleteDeliveryRequest
{
    public class CompleteDeliveryRequestCommandHandler : IRequestHandler<CompleteDeliveryRequestCommand>
    {
        private readonly IDeliveryRiderRepository _deliveryRiderRepository;
        private readonly IDeliveryRequestRepository _deliveryRequestRepository;

        public CompleteDeliveryRequestCommandHandler(IDeliveryRiderRepository deliveryRiderRepository, IDeliveryRequestRepository deliveryRequestRepository)
        {
            _deliveryRiderRepository = deliveryRiderRepository ?? throw new ArgumentNullException(nameof(deliveryRiderRepository));
            _deliveryRequestRepository = deliveryRequestRepository ?? throw new ArgumentNullException(nameof(deliveryRequestRepository));
        }

        public async Task Handle(CompleteDeliveryRequestCommand request, CancellationToken cancellationToken)
        {
            var deliveryRider = await _deliveryRiderRepository.GetByUserIdAsync(request.UserId);

            if (deliveryRider is null)
                throw new UnauthorizedException();

            if (deliveryRider.CurrentDeliveryRequestId is null)
                throw new DomainException("Delivery rider is not delivering any request");

            var deliveryRequest = await _deliveryRequestRepository.GetByIdAsync(deliveryRider.CurrentDeliveryRequestId.Value);

            if (deliveryRequest is null)
                throw new NotFoundException("Delivery request not found");

            deliveryRequest.MarkAsDelivered();
            deliveryRider.FinishDeliveryRequest();

            _deliveryRequestRepository.Update(deliveryRequest);
            _deliveryRiderRepository.Update(deliveryRider);
        }
    }
}
