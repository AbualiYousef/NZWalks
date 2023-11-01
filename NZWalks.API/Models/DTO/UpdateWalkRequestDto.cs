using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO;

public class UpdateWalkRequestDto
{
    [Microsoft.Build.Framework.Required]
    [MaxLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
    public string Name { get; set; } = string.Empty;
    [Microsoft.Build.Framework.Required]
    [MaxLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string Description { get; set; } = string.Empty;
    [Microsoft.Build.Framework.Required]
    [Range(0, 50, ErrorMessage = "Length must be between 0 and 50 km.")]
    public double LengthInKm { get; set; } 

    public string? WalkImageUrl{ get; set; }
    [Microsoft.Build.Framework.Required]
    public Guid DifficultyId { get; set; }
    [Microsoft.Build.Framework.Required]
    public Guid RegionId { get; set; }
}