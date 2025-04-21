using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

public class CategoryRepository : ICategory
{
    private readonly DataContext _context;
    private readonly ILogger<CategoryRepository> _logger;

    public CategoryRepository(DataContext context, ILogger<CategoryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Category> GetByIdAsync(Guid categoryId)
    {
        return await _context.Categories
            .Include(c => c.Cables)
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories
            .Include(c => c.Cables)
            .ToListAsync();
    }

    public async Task<Category> AddAsync(Category category)
    {
        category.CategoryId = Guid.NewGuid();
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        _logger.LogInformation("Attempting to update category. ID: {Id}, Title: {Title}", 
            category.CategoryId, category.Title);
            
        var existingCategory = await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);
            
        if (existingCategory != null)
        {
            _logger.LogInformation("Found existing category. Current Title: {CurrentTitle}", 
                existingCategory.Title);
                
            existingCategory.Title = category.Title;
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Category updated successfully. New Title: {NewTitle}", 
                category.Title);
                
            return existingCategory;
        }
        
        _logger.LogWarning("Category not found for update. ID: {Id}", category.CategoryId);
        return null;
    }

    public async Task DeleteAsync(Guid categoryId)
    {
        var category = await _context.Categories
            .Include(c => c.Cables)
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
            
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