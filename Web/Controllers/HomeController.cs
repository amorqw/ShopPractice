using System.Security.Claims;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
        ViewBag.Category = await _category.GetAllAsync();
        ViewBag.Cable = (await _cable.GetCables()).Take(4).ToList();

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

        return View("/Views/Home/Home.cshtml");
    }

    [HttpPost("/logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("tasty-cookies");
        return RedirectToAction("Index");
    }

    [HttpGet("/Menu")]
    public async Task<IActionResult> Menu(string sort)
    {
        ViewBag.Category = await _category.GetAllAsync();
        var cables = await _cable.GetCables();

        cables = sort switch
        {
            "price_asc" => cables.OrderBy(c => c.Price).ToList(),
            "price_desc" => cables.OrderByDescending(c => c.Price).ToList(),
            "name_asc" => cables.OrderBy(c => c.CableName).ToList(),
            "name_desc" => cables.OrderByDescending(c => c.CableName).ToList(),
            _ => cables
        };

        ViewBag.Cable = cables;
        return View("Menu");
    }

    [HttpGet("/Menu/Category/{categoryId}")]
    public async Task<IActionResult> MenuByCategory(Guid categoryId, string sort)
    {
        ViewBag.Category = await _category.GetAllAsync();
        var cables = await _cable.GetByCategoryIdAsync(categoryId);

        cables = sort switch
        {
            "price_asc" => cables.OrderBy(c => c.Price).ToList(),
            "price_desc" => cables.OrderByDescending(c => c.Price).ToList(),
            "name_asc" => cables.OrderBy(c => c.CableName).ToList(),
            "name_desc" => cables.OrderByDescending(c => c.CableName).ToList(),
            _ => cables
        };

        ViewBag.Cable = cables;
        ViewBag.SelectedCategory = categoryId;
        return View("Menu");
    }
}