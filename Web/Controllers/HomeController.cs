using System.Security.Claims;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ICategory _category;
    private readonly ICable _cable;

    public HomeController(ICategory category, ICable cable)
    {
        _category = category;
        _cable = cable;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var token = Request.Cookies["tasty-cookies"];
        if (string.IsNullOrEmpty(token))
        {
            ViewBag.IsAdmin = false;
            return View("Home");
        }

        var roleClaim = User.Claims.FirstOrDefault(c =>
            c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
        var isAdmin = roleClaim != null &&
                      string.Equals(roleClaim.Value, "true", StringComparison.OrdinalIgnoreCase);

        ViewBag.IsAdmin = isAdmin;
        ViewBag.Category = await _category.GetAllAsync();
        ViewBag.Cable = await _cable.GetCables();


        return View("/Views/Home/Home.cshtml");
    }

    [HttpPost("/logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("tasty-cookies");
        return RedirectToAction("Index");
    }
}