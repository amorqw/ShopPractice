using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IOrder
{
    Task<Order> GetByIdAsync(Guid orderId);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(Guid orderId);
}