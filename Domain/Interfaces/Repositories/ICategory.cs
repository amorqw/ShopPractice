using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface ICategory
{
    
    
    Task<Category> GetByIdAsync(Guid categoryId);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> AddAsync(Category category);
    Task<Category> UpdateAsync(Guid id);
    Task DeleteAsync(Guid categoryId);
    Task<IEnumerable<Cable>> GetCablesByCategoryIdAsync(Guid categoryId);
}