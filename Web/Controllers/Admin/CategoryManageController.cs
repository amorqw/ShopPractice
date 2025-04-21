using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Admin;

public class CategoryManageController : Controller
{
    private readonly ICategory _categoryService;
    private readonly ILogger<CategoryManageController> _logger;

    public CategoryManageController(ICategory categoryService, ILogger<CategoryManageController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    [Route("admin/category")]
    public async Task<IActionResult> ManageCategory()
    {
        var categories = await _categoryService.GetAllAsync();
        return View("~/Views/Admin/Category/ManageCategory.cshtml", categories);
    }

    [HttpGet]
    [Route("admin/category/{id}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View("~/Views/Admin/Category/EditCategory.cshtml", category);
    }

    [HttpGet]
    [Route("Admin/AddCategory")]
    public IActionResult AddCategory()
    {
        return View("~/Views/Admin/Category/AddCategory.cshtml");
    }

    [HttpPost]
    [Route("Admin/AddCategory")]
    public async Task<IActionResult> AddCategory(Category category)
    {
        _logger.LogInformation("Attempting to add new category. Data: {@Category}", category);
        
        if (!ModelState.IsValid)
        {
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogWarning("Validation error: {ErrorMessage}", error.ErrorMessage);
                }
            }
            return View("~/Views/Admin/Category/AddCategory.cshtml", category);
        }

        try
        {
            await _categoryService.AddAsync(category);
            _logger.LogInformation("Category added successfully");
            return RedirectToAction("ManageCategory");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding category");
            ModelState.AddModelError("", "An error occurred while adding the category");
            return View("~/Views/Admin/Category/AddCategory.cshtml", category);
        }
    }

    [HttpPost]
    [Route("admin/category/update/{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, Category category)
    {
        _logger.LogInformation("Attempting to update category. ID: {Id}, Data: {@Category}", id, category);
        
        if (!ModelState.IsValid)
        {
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogWarning("Validation error: {ErrorMessage}", error.ErrorMessage);
                }
            }
            return View("~/Views/Admin/Category/EditCategory.cshtml", category);
        }

        try
        {
            category.CategoryId = id;
            var updatedCategory = await _categoryService.UpdateAsync(category);
            if (updatedCategory != null)
            {
                _logger.LogInformation("Category updated successfully");
                return RedirectToAction("ManageCategory");
            }
            else
            {
                _logger.LogWarning("Category not found for update. ID: {Id}", id);
                ModelState.AddModelError("", "Категория не найдена");
                return View("~/Views/Admin/Category/EditCategory.cshtml", category);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category");
            ModelState.AddModelError("", "Произошла ошибка при обновлении категории");
            return View("~/Views/Admin/Category/EditCategory.cshtml", category);
        }
    }

    [HttpPost]
    [Route("admin/category/delete/{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        try
        {
            await _categoryService.DeleteAsync(id);
            _logger.LogInformation("Category deleted successfully. ID: {Id}", id);
            return RedirectToAction("ManageCategory");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category. ID: {Id}", id);
            return RedirectToAction("ManageCategory");
        }
    }
}