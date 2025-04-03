using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Data.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationCore(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IUser, UserRepository>();
        services.AddScoped<IAuth, AuthService>();
        services.AddScoped<ICable, CableRepository>();
        services.AddScoped<ICategory, CategoryRepository>();
        return services;
    }
}