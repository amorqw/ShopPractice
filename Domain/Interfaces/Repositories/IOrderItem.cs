using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IOrderItem
{
    Task<OrderItem> GetByIdAsync(Guid orderItemId);
    Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId);
    Task AddAsync(OrderItem orderItem);
    Task UpdateAsync(OrderItem orderItem);
    Task DeleteAsync(Guid orderItemId);
}