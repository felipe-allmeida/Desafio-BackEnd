using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Identity.Models
{
    public record UserUpdateEmail
    {
        [Required(ErrorMessage = "The {0} is required")]
        [EmailAddress(ErrorMessage = "The {0} is in a incorrect format")]
        public required string Email { get; init; }
    }
}
