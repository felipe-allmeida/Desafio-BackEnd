using BikeRental.CrossCutting.Storage.Abstractions;
using BikeRental.Domain.Exceptions;
using BikeRental.Domain.Models.DeliveryRiderAggregate;
using BikeRental.Domain.ValueObjects;
using MediatR;

namespace BikeRental.Application.Commands.V1.User.UpdateDeliveryRiderCnh
{
    public class UpdateDeliveryRiderCnhCommandHandler : IRequestHandler<UpdateDeliveryRiderCnhCommand>
    {
        private readonly IDeliveryRiderRepository _repository;
        private readonly IStorageService _storageService;

        public UpdateDeliveryRiderCnhCommandHandler(IDeliveryRiderRepository repository, IStorageService storageService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        public async Task Handle(UpdateDeliveryRiderCnhCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsByCnhAsync(request.CnhNumber))
                throw new ConflictException($"Delivery Rider with CNH '{request.CnhNumber}' already exists");

            var deliveryRider = await _repository.GetByUserIdAsync(request.UserId);

            if (deliveryRider is null)
                throw new NotFoundException();

            using var stream = request.CnhImage.OpenReadStream();
            var fileName = $"{deliveryRider.Id}/documents/cnh{Path.GetExtension(request.CnhImage.FileName)}";

            var response = await _storageService.UploadBlob("deliveryriders", fileName, stream, request.CnhImage.ContentType);
            var cnh = new CNH(request.CnhType, request.CnhNumber, response.Url);

            deliveryRider.UpdateCNH(cnh);

            _repository.Update(deliveryRider);
        }
    }
}
