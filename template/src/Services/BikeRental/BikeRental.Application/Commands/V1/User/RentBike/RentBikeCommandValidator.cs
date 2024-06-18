using FluentValidation;

namespace BikeRental.Application.Commands.V1.User.RentBike
{
    public class RentBikeCommandValidator : AbstractValidator<RentBikeCommand>
    {
        public RentBikeCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.StartAt).NotEmpty().Must(BeTheDayAfterToday)
                .WithMessage("StartAt must be the day after today.");

            RuleFor(x => x.EndAt).NotEmpty();
            RuleFor(x => x.ExpectedReturnAt).NotEmpty();

            RuleFor(x => x)
                .Must(x => BeValidRentalPeriod(x.StartAt, x.EndAt))
                .WithMessage("Rental period must be 7, 15 or 30 days.");

            RuleFor(x => x)
                .Must(x => BeValidReturnDate(x.StartAt, x.EndAt, x.ExpectedReturnAt))
                .WithMessage("Expected return date must be between StartAt and EndAt.");
        }

        public bool BeTheDayAfterToday(DateTimeOffset date)
        {
            var today = DateTimeOffset.UtcNow.AddHours(date.Offset.Hours);
            var tomorrow = today.AddDays(1);

            
            return date.Date == tomorrow.Date;
        }

        public bool BeValidReturnDate(DateTimeOffset startAt, DateTimeOffset endAt, DateTimeOffset expectedReturnAt)
        {
            return startAt.Date <= expectedReturnAt.Date;
        }

        public bool BeValidRentalPeriod(DateTimeOffset startAt, DateTimeOffset endAt)
        {
            var rentalDays = (endAt.Date - startAt.Date).Days;
            return rentalDays == 7 || rentalDays == 15 || rentalDays == 30;
        }
    }
}
