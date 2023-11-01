using System.ComponentModel.DataAnnotations;
namespace NZWalks.API.Models.DTO;

public class LoginRequestDto
{
    [Microsoft.Build.Framework.Required]
    [DataType(DataType.EmailAddress)]
    public string UserName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}