using Domain.Entities;

namespace Domain.Interfaces;

public interface IAuth
{
    Task<int> CreateUser(User user);
    Task<string> Login(string email, string password);
    Task<int> Register(string userName, string email, string password, string PhoneNumber);
}