using Domain.Entities;
using Domain.Entities.UserDto;

namespace Domain.Interfaces.Repositories;

public interface IUser
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserById(Guid id);
    Task<int> CreateUser(User user);
    Task<User> UpdateUser(UpdateUserDto userDto, Guid id);
    Task<User> GetUserByEmail(string email);

}