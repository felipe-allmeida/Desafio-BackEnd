using FluentValidation;

namespace BikeRental.Application.Commands.V1.Admin.UpdateBikePlate
{
    public class UpdateBikePlateCommandValidator : AbstractValidator<UpdateBikePlateCommand>
    {
        public UpdateBikePlateCommandValidator()
        {
            RuleFor(x => x.Plate).NotEmpty().Length(7);
        }
    }
}
