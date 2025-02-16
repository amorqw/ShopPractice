using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Infrastructure.Data;


public class CategoryRepository : ICategory
{
    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Category> GetByIdAsync(Guid categoryId)
    {
        return await _context.Categories
            .Include(c => c.Cables) // Включаем связанные сущности Cable
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories
            .Include(c => c.Cables) 
            .ToListAsync();
    }

    public async Task AddAsync(Category category)
    {
        category.CategoryId = Guid.NewGuid(); 
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Cable>> GetCablesByCategoryIdAsync(Guid categoryId)
    {
        var category = await _context.Categories
            .Include(c => c.Cables) 
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

        return category?.Cables ?? new List<Cable>();
    }
}