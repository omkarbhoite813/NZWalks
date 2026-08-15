using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MaxLength(3, ErrorMessage = "Code cannot be more than 3 characters")]
        [MinLength(3, ErrorMessage = "Code cannot be less than 3 characters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot be more than 100 characters")]
        [MinLength(2, ErrorMessage = "Name cannot be less than 2 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
