using BikeRental.Domain.Enums;
using FluentValidation;

namespace BikeRental.Application.Commands.V1.User.UpdateRentStatus
{
    public class UpdateRentStatusCommandValidator : AbstractValidator<UpdateRentStatusCommand>
    {
        public UpdateRentStatusCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.RentalId).NotEmpty();
            RuleFor(x => x.Status).IsInEnum().Must(BeValidStatus).WithMessage("Status can only be InProgress or Completed");
        }

        private bool BeValidStatus(ERentalStatus status)
        {
            return status != ERentalStatus.Pending;
        }
    }
}
