using System.Security.Claims;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ICategory _category;
    private readonly ICable _cable;
    private readonly IMemoryCache _cache;

    public HomeController(ICategory category, ICable cable, IMemoryCache cache)
    {
        _category = category;
        _cable = cable;
        _cache = cache;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string cacheKey = "cables_list";
        if (!_cache.TryGetValue(cacheKey, out List<Cable> cables))
        {
            cables = (await _cable.GetCables()).Take(4).ToList();
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            _cache.Set(cacheKey, cables, cacheEntryOptions);
        }
        var token = Request.Cookies["tasty-cookies"];
        ViewBag.Category = await _category.GetAllAsync();
        ViewBag.Cable = (cables);

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