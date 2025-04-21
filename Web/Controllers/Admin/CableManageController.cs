using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Admin;

public class CableManageController : Controller
{
    private readonly ICable _cableService;
    private readonly ICategory _categoryService;
    private readonly ILogger<CableManageController> _logger;

    public CableManageController(ICable cableService, ICategory categoryService, ILogger<CableManageController> logger)
    {
        _cableService = cableService;
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    [Route("admin/cable")]
    public async Task<IActionResult> ManageCable()
    {
        var cables = await _cableService.GetCables();
        return View("~/Views/Admin/Cable/ManageCable.cshtml", cables);
    }

    [HttpGet]
    [Route("admin/cable/{id}")]
    public async Task<IActionResult> GetCable(Guid id)
    {
        var cable = await _cableService.GetCableById(id);
        if (cable == null)
        {
            return NotFound();
        }
        var categories = await _categoryService.GetAllAsync();
        ViewBag.Categories = categories;
        return View("~/Views/Admin/Cable/EditCable.cshtml", cable);
    }

    [HttpGet]
    [Route("admin/cable/add")]
    public async Task<IActionResult> AddCable()
    {
        var categories = await _categoryService.GetAllAsync();
        ViewBag.Categories = categories;
        return View("~/Views/Admin/Cable/AddCable.cshtml");
    }

    [HttpPost]
    [Route("admin/cable/add")]
    public async Task<IActionResult> AddCable(Cable cable)
    {
        _logger.LogInformation("Attempting to add new cable. Data: {@Cable}", cable);
        
        if (!ModelState.IsValid)
        {
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogWarning("Validation error: {ErrorMessage}", error.ErrorMessage);
                }
            }
            return View("~/Views/Admin/Cable/AddCable.cshtml", cable);
        }

        try
        {
            await _cableService.AddCable(cable);
            _logger.LogInformation("Cable added successfully");
            return RedirectToAction("ManageCable");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding cable");
            ModelState.AddModelError("", "Произошла ошибка при добавлении кабеля");
            return View("~/Views/Admin/Cable/AddCable.cshtml", cable);
        }
    }

    [HttpPost]
    [Route("admin/cable/update/{id}")]
    public async Task<IActionResult> UpdateCable(Guid id, Cable cable)
    {
        if (ModelState.IsValid)
        {
            cable.CableId = id;
            await _cableService.UpdateCable(cable);
            return RedirectToAction("ManageCable");
        }
        return View("~/Views/Admin/Cable/EditCable.cshtml", cable);
    }

    [HttpPost]
    [Route("admin/cable/delete/{id}")]
    public async Task<IActionResult> DeleteCable(Guid id)
    {
        await _cableService.DeleteCable(id);
        return RedirectToAction("ManageCable");
    }

    [HttpGet]
    [Route("admin/cable/category/{categoryId}")]
    public async Task<IActionResult> GetCablesByCategory(Guid categoryId)
    {
        var cables = await _cableService.GetByCategoryIdAsync(categoryId);
        return View("~/Views/Admin/Cable/ManageCable.cshtml", cables);
    }
}