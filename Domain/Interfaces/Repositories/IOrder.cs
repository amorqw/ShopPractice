using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IOrder
{
    Task<Order> GetByIdAsync(Guid orderId);
    Task<IEnumerable<Order>> GetAllAsync();
    Task AddAsync(Order order);
   
}