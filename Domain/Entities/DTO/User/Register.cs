using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.UserDto;

public class Register
{
    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Пароль обязателен")]
    [MinLength(6, ErrorMessage = "Пароль должен быть не менее 6 символов")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Имя обязательно")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Фамилия обязательна")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Номер телефона обязателен")]
    [Phone(ErrorMessage = "Некорректный формат номера телефона")]
    public string PhoneNumber { get; set; } = null!;
}