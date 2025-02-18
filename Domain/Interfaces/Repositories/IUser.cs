using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUser
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserById(Guid id);
    Task<int> CreateUser(User user);
    Task<bool> DeleteUserById(Guid id);
    Task UpdateUser(Guid id, string password);
    Task<bool> UserExists(Guid id);
    Task<User> GetUserByEmail(string email);

}