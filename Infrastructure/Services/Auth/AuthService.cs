using Domain.Entities;
using Domain.Entities.UserDto;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Auth;

public class AuthService: IAuth
{
    private readonly IUser _user;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IUser user, IPasswordHasher passwordHasher, IJwtProvider jwtProvider, IHttpContextAccessor httpContextAccessor)
    {
        _user = user;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> Register(Register userRegister)
    {
        var hashedPassword = _passwordHasher.Generate(userRegister.Password);
        var newUser = new User
        {
            UserId = Guid.NewGuid(),
            Password = hashedPassword,
            Email = userRegister.Email,
            FirstName = userRegister.FirstName,
            LastName = userRegister.LastName,
            PhoneNumber = userRegister.PhoneNumber
        };

        return await _user.CreateUser(newUser); 
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _user.GetUserByEmail(email);
        if (user == null || !_passwordHasher.Verify(password, user.Password))
        {
            throw new UnauthorizedAccessException("Неверные данные!");
            
        }
        var token = _jwtProvider.GenerateToken(user);
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            context.Response.Cookies.Append("tasty-cookies", token);
        }
        return token;
    }

    public async Task<int> CreateUser(User user)
    {
        return await _user.CreateUser(user); 
    }
}
