using Domain.Entities;

namespace Domain.Interfaces;

public interface ICategory
{
    
    
    Task<Category> GetByIdAsync(Guid categoryId);
    Task<IEnumerable<Category>> GetAllAsync();
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Guid categoryId);
    Task<IEnumerable<Cable>> GetCablesByCategoryIdAsync(Guid categoryId);
}