using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Admin;

public class OrderManageController : Controller
{
    private readonly IOrder _orderService;
    private readonly IUser _userService;
    private readonly ICartItem _cartItemService;

    public OrderManageController(
        IOrder orderService,
        IUser userService,
        ICartItem cartItemService,
        ILogger<OrderManageController> logger)
    {
        _orderService = orderService;
        _userService = userService;
        _cartItemService = cartItemService;
    }

    [HttpGet]
    [Route("admin/order")]
    public async Task<IActionResult> ManageOrder()
    {
        var orders = await _orderService.GetAllAsync();
        return View("~/Views/Admin/Order/ManageOrder.cshtml", orders);
    }

    [HttpGet]
    [Route("admin/order/{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        
        ViewBag.Users = await _userService.GetAllUsers();
        ViewBag.CartItems = await _cartItemService.GetAllAsync();
        return View("~/Views/Admin/Order/ViewOrder.cshtml", order);
    }
} 