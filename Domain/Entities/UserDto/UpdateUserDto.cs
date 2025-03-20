using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.UserDto;

public class UpdateUserDto
{
    [Key]
    public Guid UserId { get; set; }
    public string FirstName { get; set; }=string.Empty;
    public string LastName { get; set; }=string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = null!;
}