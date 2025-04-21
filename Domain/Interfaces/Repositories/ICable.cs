using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface ICable
{
    Task<Cable> GetCableById(Guid id);
    Task<IEnumerable<Cable>> GetCables();
    Task AddCable(Cable cable);
    Task UpdateCable(Cable cable);
    Task DeleteCable(Guid id);
    Task<IEnumerable<Cable>> GetByCategoryIdAsync(Guid categoryId);
}