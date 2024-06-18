using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.DeliveryRequestAggregate;
using BikeRental.Domain.Models.DeliveryRequestNotificationAggregate;
using BikeRental.Domain.Models.DeliveryRiderAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.User.AcceptDeliveryRequest
{
    public class AcceptDeliveryRequestCommandHandler : IRequestHandler<AcceptDeliveryRequestCommand>
    {
        private readonly IDeliveryRiderRepository _deliveryRiderRepository;
        private readonly IDeliveryRequestRepository _deliveryRequestRepository;
        private readonly IDeliveryRequestNotificationRepository _deliveryRequestNotificationRepository;

        public AcceptDeliveryRequestCommandHandler(IDeliveryRiderRepository deliveryRiderRepository, IDeliveryRequestRepository deliveryRequestRepository, IDeliveryRequestNotificationRepository deliveryRequestNotificationRepository)
        {
            _deliveryRiderRepository = deliveryRiderRepository ?? throw new ArgumentNullException(nameof(deliveryRiderRepository));
            _deliveryRequestRepository = deliveryRequestRepository ?? throw new ArgumentNullException(nameof(deliveryRequestRepository));
            _deliveryRequestNotificationRepository = deliveryRequestNotificationRepository ?? throw new ArgumentNullException(nameof(deliveryRequestNotificationRepository));
        }

        public async Task Handle(AcceptDeliveryRequestCommand request, CancellationToken cancellationToken)
        {
            var deliveryRider = await _deliveryRiderRepository.GetByUserIdAsync(request.UserId);

            if (deliveryRider is null)
                throw new UnauthorizedException();

            var notification = await _deliveryRequestNotificationRepository.GetDeliveryRequestNotification(request.DeliveryRequestId, deliveryRider.Id);
            
            if (notification is null)
                throw new DomainException("Delivery rider did not receive a notification");

            if (notification.Status != Domain.Enums.EDeliveryRequestNotificationStatus.Consumed)
                throw new DomainException("Delivery request notification is not consumed");

            var deliveryRequest = await _deliveryRequestRepository.GetByIdAsync(request.DeliveryRequestId);

            if (deliveryRequest is null)
                throw new NotFoundException("Delivery request not found");

            deliveryRequest.MarkAsAccepted(deliveryRider.Id);
            deliveryRider.AcceptDeliveryRequestId(deliveryRequest.Id);

            _deliveryRequestRepository.Update(deliveryRequest);
            _deliveryRiderRepository.Update(deliveryRider);
        }
    }
}
