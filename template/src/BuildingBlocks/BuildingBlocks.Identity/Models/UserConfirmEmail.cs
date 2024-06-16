using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Identity.Models
{
    public record UserConfirmEmail
    {
        [Required(ErrorMessage = "The {0} is required")]
        [EmailAddress(ErrorMessage = "The {0} is in a incorrect format")]
        public required string Email { get; init; }
        [Required(ErrorMessage = "The {0} is required")]
        public required string Token { get; init; }
    }

    public record UserResetPassword
    {
        [Required(ErrorMessage = "The {0} is required")]
        [EmailAddress(ErrorMessage = "The {0} is in a incorrect format")]
        public required string Email { get; init; }
        [Required(ErrorMessage = "The {0} is required")]
        [StringLength(100, ErrorMessage = "The {0} must have between {2} and {1} characters", MinimumLength = 6)]
        public required string NewPassword { get; init; }
        [Required(ErrorMessage = "The {0} is required")]
        public required string Token { get; init; }
    }
}
