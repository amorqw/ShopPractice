using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class HomeController: Controller
{
    public IActionResult Index()
    {
        return View("/Views/Home/Home.cshtml");
    }
}