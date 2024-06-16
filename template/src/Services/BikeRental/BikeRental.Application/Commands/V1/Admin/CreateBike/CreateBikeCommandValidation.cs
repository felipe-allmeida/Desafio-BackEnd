using FluentValidation;

namespace BikeRental.Application.Commands.V1.Admin.CreateBike
{
    public class CreateBikeCommandValidation : AbstractValidator<CreateBikeCommand>
    {
        public CreateBikeCommandValidation()
        {
            RuleFor(x => x.Year).InclusiveBetween(1950, 2100);
            RuleFor(x => x.Model).NotEmpty();
            RuleFor(x =>x.Plate).NotEmpty();
        }
    }
}
