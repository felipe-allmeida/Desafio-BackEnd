using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Identity.Models
{
    public record UserLogin
    {
        [Required(ErrorMessage = "The {0} is required")]
        [EmailAddress]
        public string Email { get; init; }

        [Required(ErrorMessage = "The {0} is required")]
        [StringLength(100, ErrorMessage = "The {0} must have between {2} and {1} characters", MinimumLength = 6)]
        public required string Password { get; init; }
    }
}
