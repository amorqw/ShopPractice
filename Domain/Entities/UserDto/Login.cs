using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.UserDto;

public class Login
{
    [Required]
    public string email { get; set; } = null!;
    public string password { get; set; } = null!;
}