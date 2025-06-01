using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface ICategory
{
    
    
    Task<Category> GetByIdAsync(Guid categoryId);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> AddAsync(Category category);
    Task<Category> UpdateAsync(Category category);
    Task DeleteAsync(Guid categoryId);
}