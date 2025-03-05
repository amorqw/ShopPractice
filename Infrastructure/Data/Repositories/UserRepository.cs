using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class UserRepository: IUser
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserById(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u=> u.UserId == id);
    }

    public async Task<int> CreateUser(User user)
    {
        if (await UserExists(user.UserId))
            return 0;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return 1;
    }

    public async Task<bool> DeleteUserById(Guid id)
    {
        if(await UserExists(id)) 
            _context.Users.Remove(await GetUserById(id));
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task UpdateUser(Guid id, string password)
    {
        await _context.Users.Where(u => u.UserId == id)
            .ExecuteUpdateAsync(s => s.SetProperty(c=>c.Password, password));
    }

    public async Task<bool> UserExists(Guid id)
    {
        return await _context.Users.AnyAsync(u => u.UserId == id);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}