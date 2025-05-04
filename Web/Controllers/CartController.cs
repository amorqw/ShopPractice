using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Web.Controllers;

public class CartController : Controller
{
    private readonly ICartItem _cartItemRepo;
    private readonly ICable _cableRepo;
    private readonly IOrder _orderRepo;

    public CartController(ICartItem cartItemRepo, ICable cableRepo, IOrder orderRepo)
    {
        _cartItemRepo = cartItemRepo;
        _cableRepo = cableRepo;
        _orderRepo = orderRepo;
    }

    [HttpGet("/Cart/OrderSuccess")]
    public IActionResult OrderSuccess()
    {
        return View("OrderSuccess");
    }
    [HttpGet("/Cart")]
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var cartItems = await _cartItemRepo.GetCartItemsAsync(userId);
        return View("Cart", cartItems);
    }

    [HttpPost("/Cart/Add")]
    public async Task<IActionResult> Add(Guid cableId, int quantity = 1)
    {
        var userId = GetUserId();
        var cable = await _cableRepo.GetCableById(cableId);

        var existing = (await _cartItemRepo.GetCartItemsAsync(userId))
            .FirstOrDefault(ci => ci.CableId == cableId && ci.Status == ItemStatus.InCart);

        if (existing != null)
        {
            await _cartItemRepo.UpdateQuantityAsync(existing.Id, existing.Quantity + quantity);
        }
        else
        {
            var cartItem = new CartItem
            {
                CableId = cableId,
                UserId = userId,
                Quantity = quantity,
                TotalPrice = cable.Price * quantity,
                Status = ItemStatus.InCart
            };
            await _cartItemRepo.AddAsync(cartItem);
        }

        return RedirectToAction("Index");
    }

    [HttpPost("/Cart/Remove")]
    public async Task<IActionResult> Remove(Guid cartItemId)
    {
        await _cartItemRepo.DeleteAsync(cartItemId);
        return RedirectToAction("Index");
    }

    [HttpPost("/Cart/Update")]
    public async Task<IActionResult> Update(Guid cartItemId, int quantity)
    {
        await _cartItemRepo.UpdateQuantityAsync(cartItemId, quantity);
        return RedirectToAction("Index");
    }

    private Guid GetUserId()
    {
        var token = Request.Cookies["tasty-cookies"];
        if (string.IsNullOrEmpty(token))
            return Guid.Empty;

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == "userid");
        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            return userId;

        return Guid.Empty;
    }

    [HttpPost("/Cart/Checkout")]
    public async Task<IActionResult> Checkout(string shippingAddress)
    {
        var userId = GetUserId();
        var cartItems = (await _cartItemRepo.GetCartItemsAsync(userId)).ToList();

        if (!cartItems.Any())
        {
            return RedirectToAction("Index");
        }

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            ShippingAddress = shippingAddress,
            CartItems = new List<CartItem>()
        };

        foreach (var item in cartItems)
        {
            item.Status = ItemStatus.InOrder;
            order.CartItems.Add(item);
            await _cartItemRepo.UpdateAsync(item);
        }

        await _orderRepo.AddAsync(order);

        return RedirectToAction("OrderSuccess");
    }
}