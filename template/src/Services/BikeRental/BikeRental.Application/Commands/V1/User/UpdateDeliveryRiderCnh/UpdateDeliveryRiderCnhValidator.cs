using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BikeRental.Application.Commands.V1.User.UpdateDeliveryRiderCnh
{
    public class UpdateDeliveryRiderCnhValidator : AbstractValidator<UpdateDeliveryRiderCnhCommand>
    {
        public UpdateDeliveryRiderCnhValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.CnhType).IsInEnum();
            RuleFor(x => x.CnhNumber).NotEmpty();

            RuleFor(x => x.CnhImage)
                .Must(IsValidFileType).WithMessage("Only .png and .bmp files are allowed.");
        }

        private bool IsValidFileType(IFormFile file)
        {
            var allowedExtensions = new[] { ".png", ".bmp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            return allowedExtensions.Contains(extension);
        }
    }   
}
