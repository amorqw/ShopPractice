using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Admin;

public class CategoryManageController : Controller
{
    private readonly ICategory _categoryService;

    public CategoryManageController(ICategory categoryService, ILogger<CategoryManageController> logger)
    {
        _categoryService = categoryService;
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
        
        if (!ModelState.IsValid)
        {
            
            return View("~/Views/Admin/Category/AddCategory.cshtml", category);
        }

        try
        {
            await _categoryService.AddAsync(category);
            return RedirectToAction("ManageCategory");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while adding the category");
            return View("~/Views/Admin/Category/AddCategory.cshtml", category);
        }
    }

    [HttpPost]
    [Route("admin/category/update/{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, Category category)
    {
        
        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Category/EditCategory.cshtml", category);
        }

        try
        {
            category.CategoryId = id;
            var updatedCategory = await _categoryService.UpdateAsync(category);
            if (updatedCategory != null)
            {
                return RedirectToAction("ManageCategory");
            }
            else
            {
                ModelState.AddModelError("", "Категория не найдена");
                return View("~/Views/Admin/Category/EditCategory.cshtml", category);
            }
        }
        catch (Exception ex)
        {
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
            return RedirectToAction("ManageCategory");
        }
        catch (Exception ex)
        {
            return RedirectToAction("ManageCategory");
        }
    }
}