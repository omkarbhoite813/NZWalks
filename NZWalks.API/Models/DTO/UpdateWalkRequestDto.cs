using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateWalkRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot be more than 100 characters")]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000, ErrorMessage = "Description cannot be more than 1000 characters")]
        public string Description { get; set; }
        [Required]
        [Range(0,50, ErrorMessage = "Length in km must be between 0 and 50")]
        public double LengthInKm { get; set; }
        public string? WalkImageUri { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }
    }
}
