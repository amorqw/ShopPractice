using Domain.Entities;
using Domain.Entities.UserDto;

namespace Domain.Interfaces;

public interface IAuth
{
    Task<string> Login(string email, string password);
    Task<int> Register(Register register);
}