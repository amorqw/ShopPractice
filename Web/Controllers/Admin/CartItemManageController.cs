using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Admin;

public class CartItemManageController : Controller
{
    private readonly ICartItem _cartItemService;
    private readonly IUser _userService;
    private readonly ICable _cableService;
    private readonly ILogger<CartItemManageController> _logger;

    public CartItemManageController(
        ICartItem cartItemService,
        IUser userService,
        ICable cableService,
        ILogger<CartItemManageController> logger)
    {
        _cartItemService = cartItemService;
        _userService = userService;
        _cableService = cableService;
        _logger = logger;
    }

    [HttpGet]
    [Route("admin/cartitem")]
    public async Task<IActionResult> ManageCartItem()
    {
        var cartItems = await _cartItemService.GetAllAsync();
        return View("~/Views/Admin/CartItem/ManageCartItem.cshtml", cartItems);
    }

    [HttpGet]
    [Route("admin/cartitem/{id}")]
    public async Task<IActionResult> GetCartItem(Guid id)
    {
        var cartItem = await _cartItemService.GetByIdAsync(id);
        if (cartItem == null)
        {
            return NotFound();
        }
        
        ViewBag.Users = await _userService.GetAllUsers();
        ViewBag.Cables = await _cableService.GetCables();
        return View("~/Views/Admin/CartItem/ViewCartItem.cshtml", cartItem);
    }

    [HttpGet]
    [Route("admin/cartitem/add")]
    public async Task<IActionResult> AddCartItem()
    {
        ViewBag.Cables = await _cableService.GetCables();
        return View("~/Views/Admin/CartItem/AddCartItem.cshtml");
    }

    [HttpPost]
    [Route("admin/cartitem/add")]
    public async Task<IActionResult> AddCartItem(CartItem cartItem)
    {
        _logger.LogInformation("Attempting to add new cart item. Data: {@CartItem}", cartItem);
        
        if (!ModelState.IsValid)
        {
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogWarning("Validation error: {ErrorMessage}", error.ErrorMessage);
                }
            }
            ViewBag.Cables = await _cableService.GetCables();
            return View("~/Views/Admin/CartItem/AddCartItem.cshtml", cartItem);
        }

        try
        {
            await _cartItemService.AddAsync(cartItem);
            _logger.LogInformation("Cart item added successfully");
            return RedirectToAction("ManageCartItem");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding cart item");
            ModelState.AddModelError("", "Произошла ошибка при добавлении позиции");
            ViewBag.Cables = await _cableService.GetCables();
            return View("~/Views/Admin/CartItem/AddCartItem.cshtml", cartItem);
        }
    }

    [HttpPost]
    [Route("admin/cartitem/update/{id}")]
    public async Task<IActionResult> UpdateCartItem(Guid id, CartItem cartItem)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Cables = await _cableService.GetCables();
            return View("~/Views/Admin/CartItem/EditCartItem.cshtml", cartItem);
        }

        try
        {
            cartItem.Id = id;
            await _cartItemService.UpdateAsync(cartItem);
            return RedirectToAction("ManageCartItem");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating cart item");
            ModelState.AddModelError("", "Произошла ошибка при обновлении позиции");
            ViewBag.Cables = await _cableService.GetCables();
            return View("~/Views/Admin/CartItem/EditCartItem.cshtml", cartItem);
        }
    }

    [HttpPost]
    [Route("admin/cartitem/delete/{id}")]
    public async Task<IActionResult> DeleteCartItem(Guid id)
    {
        try
        {
            await _cartItemService.DeleteAsync(id);
            _logger.LogInformation("Cart item deleted successfully. ID: {Id}", id);
            return RedirectToAction("ManageCartItem");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting cart item. ID: {Id}", id);
            return RedirectToAction("ManageCartItem");
        }
    }

    [HttpPost]
    [Route("admin/cartitem/{id}/quantity")]
    public async Task<IActionResult> UpdateQuantity(Guid id, int quantity)
    {
        await _cartItemService.UpdateQuantityAsync(id, quantity);
        return RedirectToAction("ManageCartItem");
    }

    [HttpPost]
    [Route("admin/cartitem/{id}/movetoorder/{orderId}")]
    public async Task<IActionResult> MoveToOrder(Guid id, Guid orderId)
    {
        await _cartItemService.MoveToOrderAsync(id, orderId);
        return RedirectToAction("ManageCartItem");
    }

    public async Task<IActionResult> ViewCartItem(Guid id)
    {
        var cartItem = await _cartItemService.GetByIdAsync(id);
        if (cartItem == null)
        {
            return NotFound();
        }

        ViewBag.Users = await _userService.GetAllUsers();
        ViewBag.Cables = await _cableService.GetCables();

        return View(cartItem);
    }
} 