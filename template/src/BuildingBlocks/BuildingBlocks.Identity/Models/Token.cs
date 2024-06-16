using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BuildingBlocks.Identity.Models
{
    public class Token
    {
        [Required]
        [JsonPropertyName("refresh-token")]
        public required string RefreshToken { get; set; }
    }
}
