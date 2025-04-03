using Domain.Entities;
using Domain.Entities.DTO.Category;
using Domain.Entities.UserDto;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Admin;

public class CategoryManageController : Controller
{
    private readonly ICategory _categoryService;

    public CategoryManageController(ICategory categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [Route("admin/managecable")]
    public async Task<IActionResult> ManageCategories()
    {
        var categories = await _categoryService.GetAllAsync();
        return View("~/Views/Admin/Category/ManageCategory.cshtml", categories);
    }

    [HttpGet]
    [Route("admin/EditCategory/{id}")]
    public async Task<IActionResult> EditUser(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        var categoryDto = new UpdateCategoryDto()
        {
            CategoryId = Guid.NewGuid(),
            Title = category.Title
        };

        return View("~/Views/Admin/User/EditCategory.cshtml", categoryDto);
    }

    [HttpPost]
    [Route("admin/UpdateCategory/{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id)
    {
        if (ModelState.IsValid)
        {
            var updatedUser = await _categoryService.UpdateAsync(id);
            if (updatedUser != null)
            {
                return RedirectToAction("ManageCategories");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to update category.");
            }
        }

        return View("~/Views/Admin/User/EditCategory.cshtml");
    }

    [HttpGet]
    [Route("Admin/AddCategory")]
    public IActionResult AddCategory()
    {
        return View("~/Views/Admin/Category/AddCategory.cshtml");
    }

    [HttpPost]
    [Route("Admin/AddCable")]
    public async Task<IActionResult> AddCategory(Category category)
    {
        if (ModelState.IsValid)
        {
            var userWithPassword = new Category()
            {
                CategoryId = Guid.NewGuid(),
                Title = category.Title
            };
            var newCategory = await _categoryService.AddAsync(userWithPassword);
            if (newCategory != null)
            {
                return RedirectToAction("ManageCategories");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to add category.");
            }
        }

        return View("~/Views/Admin/Category/AddCategory.cshtml");
    }
}