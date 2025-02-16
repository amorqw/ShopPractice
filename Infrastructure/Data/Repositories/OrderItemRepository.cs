using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class OrderItemRepository:IOrderItem
{
    private readonly DataContext _context;

    public OrderItemRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<OrderItem> GetByIdAsync(Guid orderItemId)
    {
        return await _context.OrderItems
            .Include(oi => oi.Cable) 
            .Include(oi => oi.Order) 
            .FirstOrDefaultAsync(oi => oi.Id == orderItemId);
    }

    public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId)
    {
        return await _context.OrderItems
            .Where(oi => oi.OrderId == orderId)
            .Include(oi => oi.Cable)
            .ToListAsync();
    }

    public async Task AddAsync(OrderItem orderItem)
    {
        orderItem.Id = Guid.NewGuid();
        await _context.OrderItems.AddAsync(orderItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(OrderItem orderItem)
    {
        _context.OrderItems.Update(orderItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid orderItemId)
    {
        var orderItem = await _context.OrderItems.FindAsync(orderItemId);
        if (orderItem != null)
        {
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
        }
    }
}