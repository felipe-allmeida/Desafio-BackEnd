using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.DeliveryRequestNotificationAggregate;
using BikeRental.Domain.Models.DeliveryRiderAggregate;
using MediatR;

namespace BikeRental.Application.Commands.V1.Automated.UpdateDeliveryRequestNotificationStatus
{
    public class UpdateDeliveryRequestNotificationStatusCommandHandler : IRequestHandler<UpdateDeliveryRequestNotificationStatusCommand>
    {
        private readonly IDeliveryRequestNotificationRepository _repository;        

        public UpdateDeliveryRequestNotificationStatusCommandHandler(IDeliveryRequestNotificationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(UpdateDeliveryRequestNotificationStatusCommand request, CancellationToken cancellationToken)
        {
            var notification = await _repository.GetDeliveryRequestNotification(request.DeliveryRequestId, request.DeliveryRiderId);

            if (notification is null)
                throw new NotFoundException();

            notification.MarkAsConsumed();

            _repository.Update(notification);
        }
    }
}
