using Domain.Entities.UserDto;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class RegisterController : Controller
{
    readonly IAuth _auth;

    public RegisterController(IAuth auth)
    {
        _auth = auth;
    }

    [HttpGet]
    [Route("/register")]
    public IActionResult Index()
    {
        return View("/Views/Register/Register.cshtml", new Register());
    }

    [HttpPost]
    [Route("/register")]
    public async Task<IActionResult> Register(Register registerUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(registerUser);
        }

        try 
        {
            await _auth.Register(registerUser);
            return RedirectToAction("Index", "Login");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Ошибка при регистрации: " + ex.Message);
            return View("/Views/Register/Register.cshtml", registerUser);
        }
    }
}