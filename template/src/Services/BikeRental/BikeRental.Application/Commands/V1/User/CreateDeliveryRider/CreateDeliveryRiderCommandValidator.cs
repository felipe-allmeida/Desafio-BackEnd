using FluentValidation;

namespace BikeRental.Application.Commands.V1.User.CreateDeliveryRider
{
    public class CreateDeliveryRiderCommandValidator : AbstractValidator<CreateDeliveryRiderCommand>
    {
        public CreateDeliveryRiderCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Cnpj).NotEmpty();
            RuleFor(x => x.Birthday).NotEmpty().Must(x => (DateTimeOffset.UtcNow.Year - x.Year) >= 18).WithMessage("The rider must be at least 18 years old.");
        }
    }
}
