using Domain.Interfaces;
using Infrastructure.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class LoginController : Controller
{
    private readonly IAuth _authService;

    public LoginController(IAuth authService)
    {
        _authService = authService;
    }
    [HttpGet]
    [Route("/login")]
    public IActionResult Index()
    {
        return View("Login");
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var token = await _authService.Login(email, password);
            return Redirect($"Home/Index?token={token}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
}