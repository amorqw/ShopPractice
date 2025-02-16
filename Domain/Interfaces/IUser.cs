using Domain.Entities;

namespace Domain.Interfaces;

public interface IUser
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserById(Guid id);
    Task<bool> CreateUser(User user);
    Task<bool> DeleteUserById(Guid id);
    Task<User> UpdateUser(User user);
    
    
}