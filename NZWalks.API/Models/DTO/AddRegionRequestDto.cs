using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO;

public class AddRegionRequestDto
{
    [Required]
    [MinLength(3,ErrorMessage = "Code must be at least 3 characters long")]
    [MaxLength(3,ErrorMessage = "Code must be at most 3 characters long")]
    public string Code { get; set; } = string.Empty;
    [Required]
    [MaxLength(50,ErrorMessage = "Name must be at most 50 characters long")]
    public string Name { get; set; } = string.Empty;
    public string? RegionImageUrl { get; set; } 
}