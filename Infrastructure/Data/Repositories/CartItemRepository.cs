using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class CartItemRepository : ICartItem
{
    private readonly DataContext _context;

    public CartItemRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<CartItem> GetByIdAsync(Guid id)
    {
        return await _context.CartItems
            .Include(ci => ci.Cable)
            .Include(ci => ci.Order)
            .Include(ci => ci.User)
            .FirstOrDefaultAsync(ci => ci.Id == id);
    }

    public async Task<IEnumerable<CartItem>> GetAllAsync()
    {
        return await _context.CartItems
            .Include(ci => ci.Cable)
            .Include(ci => ci.Order)
            .Include(ci => ci.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<CartItem>> GetByUserIdAsync(Guid userId)
    {
        return await _context.CartItems
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Cable)
            .Include(ci => ci.Order)
            .ToListAsync();
    }

    public async Task<IEnumerable<CartItem>> GetByOrderIdAsync(Guid orderId)
    {
        return await _context.CartItems
            .Where(ci => ci.OrderId == orderId)
            .Include(ci => ci.Cable)
            .Include(ci => ci.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(Guid userId)
    {
        return await _context.CartItems
            .Where(ci => ci.UserId == userId && ci.Status == ItemStatus.InCart)
            .Include(ci => ci.Cable)
            .ToListAsync();
    }

    public async Task AddAsync(CartItem cartItem)
    {
        cartItem.Id = Guid.NewGuid();
        await _context.CartItems.AddAsync(cartItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CartItem cartItem)
    {
        _context.CartItems.Update(cartItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var cartItem = await _context.CartItems.FindAsync(id);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateQuantityAsync(Guid id, int quantity)
    {
        var cartItem = await _context.CartItems.FindAsync(id);
        if (cartItem != null)
        {
            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MoveToOrderAsync(Guid cartItemId, Guid orderId)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem != null)
        {
            cartItem.OrderId = orderId;
            cartItem.Status = ItemStatus.InOrder;
            await _context.SaveChangesAsync();
        }
    }
} 