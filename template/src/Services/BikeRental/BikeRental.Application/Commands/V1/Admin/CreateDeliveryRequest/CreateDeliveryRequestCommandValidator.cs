using FluentValidation;

namespace BikeRental.Application.Commands.V1.Admin.CreateDeliveryRequest
{
    public class CreateDeliveryRequestCommandValidator : AbstractValidator<CreateDeliveryRequestCommand>
    {
        public CreateDeliveryRequestCommandValidator()
        {
            RuleFor(x => x.PriceCents).NotEmpty();
        }
    }
}
