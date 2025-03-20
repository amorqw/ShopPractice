using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Admin
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Admin/AdminPanel.cshtml");
        }


        public IActionResult ManageIngredients()
        {
            return View("~/Views/Admin/ManageIngredients.cshtml");
        }

        public IActionResult ManageUsers()
        {
            return View("~/Views/Admin/ManageUsers.cshtml");
        }

        public IActionResult ManageOrders()
        {
            return View("~/Views/Admin/ManageOrders.cshtml");
        }
    }
}