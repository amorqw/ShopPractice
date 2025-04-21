using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class OrderRepository: IOrder
{
    private readonly DataContext _context;

    public OrderRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Order> GetByIdAsync(Guid orderId)
    {
        return await _context.Orders
            .Include(o => o.User) 
            .Include(o => o.CartItems) 
                .ThenInclude(ci => ci.Cable) 
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.CartItems)
                .ThenInclude(ci => ci.Cable)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.CartItems)
                .ThenInclude(ci => ci.Cable)
            .ToListAsync();
    }

    public async Task AddAsync(Order order)
    {
        order.OrderId = Guid.NewGuid();
        order.OrderDate = DateTime.UtcNow; 
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid orderId)
    {
        var order = await _context.Orders
            .Include(o => o.CartItems)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order != null)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.OrderId == orderId)
                .ToListAsync();
            
            _context.CartItems.RemoveRange(cartItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}