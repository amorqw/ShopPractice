using Domain.Entities;

namespace Domain.Interfaces;

public interface ICable
{
    Task<Cable> GetCableById(Guid id);
    Task<IEnumerable<Cable>> GetCables();
    Task AddCable(Cable cable);
    Task UpdateCable(int price);
    Task DeleteCable(Guid id);
    Task<IEnumerable<Cable>> GetByCategoryIdAsync(Guid categoryId);
}