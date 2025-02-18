using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;

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

    public async Task<int> Register(string userName, string email, string password, string phoneNumber)
    {
        var hashedPassword = _passwordHasher.Generate(password);
        var newUser = new User
        {
            FirstName = userName,
            Email = email,
            Password = hashedPassword,
            PhoneNumber = phoneNumber
        };

        return await _user.CreateUser(newUser); 
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _user.GetUserByEmail(email); 
        if (user == null || !_passwordHasher.Verify(password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
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
