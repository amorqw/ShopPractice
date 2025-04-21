using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface ICartItem
{
    Task<CartItem> GetByIdAsync(Guid id);
    Task<IEnumerable<CartItem>> GetAllAsync();
    Task<IEnumerable<CartItem>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<CartItem>> GetByOrderIdAsync(Guid orderId);
    Task<IEnumerable<CartItem>> GetCartItemsAsync(Guid userId);
    Task AddAsync(CartItem cartItem);
    Task UpdateAsync(CartItem cartItem);
    Task DeleteAsync(Guid id);
    Task UpdateQuantityAsync(Guid id, int quantity);
    Task MoveToOrderAsync(Guid cartItemId, Guid orderId);
} 