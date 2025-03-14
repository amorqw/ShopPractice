using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class CableRepository: ICable
{
    private readonly DataContext _context;

    public CableRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Cable> GetCableById(Guid id)
    {
        return await _context.Cables.FirstOrDefaultAsync(c => c.CableId == id);
    }

    public async Task<IEnumerable<Cable>> GetCables()
    {
        return await _context.Cables.ToListAsync();
    }

    public async Task AddCable(Cable cable)
    {
        if (!_context.Cables.Any(c => c.CableId == cable.CableId))
        {
            cable.CableId = Guid.NewGuid();
            await _context.Cables.AddAsync(cable);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCable(Guid id)
    {
        var cable = await _context.Cables.FirstOrDefaultAsync(c => c.CableId == id);
        if (cable != null)
        {
            _context.Cables.Remove(cable);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateCable(int price)
    {
        await _context.Cables.Where(c => c.Price == price)
            .ExecuteUpdateAsync(s => s.SetProperty(c=>c.Price, price));
    }
    public async Task<IEnumerable<Cable>> GetByCategoryIdAsync(Guid categoryId)
    {
        return await _context.Cables
            .Where(c => c.CategoryId == categoryId)
            .Include(c => c.Category) 
            .ToListAsync();
    }
    
    
}