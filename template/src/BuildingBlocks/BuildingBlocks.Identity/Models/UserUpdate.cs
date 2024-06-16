using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Identity.Models
{
    public record UserUpdate
    {
        [Required(ErrorMessage = "The {0} is required")]
        public required string FirstName { get; set; }
        [Required(ErrorMessage = "The {0} is required")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "The {0} is required")]
        [EmailAddress(ErrorMessage = "The {0} is in a incorrect format")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "The {0} is required")]
        public required string Role { get; init; }
    }
}
