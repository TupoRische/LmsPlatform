using Core.Contracts;
using Core.ViewModels.Materials;
using Core.ViewModels.Teacher.Materials;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Web.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class MaterialsController : Controller
    {
        private readonly ITeacherMaterialsService materials;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        public MaterialsController(ITeacherMaterialsService materials, IWebHostEnvironment env)
        {
            this.materials = materials;
            _env = env;
        }

        public async Task<IActionResult> My()
            => View(await materials.GetMineAsync(User));

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMaterialVm model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return View(model);
            }

            string? filePath = null;

            if (model.File != null && model.File.Length > 0)
            {
                if (model.File.Length > 20 * 1024 * 1024) // 20MB
                {
                    ModelState.AddModelError(nameof(model.File), "Файлът е прекалено голям (макс 20MB).");
                    await LoadDropdownsAsync();
                    return View(model);
                }

                var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "materials");
                Directory.CreateDirectory(uploadsDir);

                var ext = Path.GetExtension(model.File.FileName);
                var safeName = $"{Guid.NewGuid()}{ext}";
                var fullPath = Path.Combine(uploadsDir, safeName);

                await using var fs = System.IO.File.Create(fullPath);
                await model.File.CopyToAsync(fs);

                filePath = $"/uploads/materials/{safeName}";
            }

            var id = await materials.CreateAsync(User, model, filePath);
            return RedirectToAction(nameof(Details), new { id });
        }
        private async Task LoadDropdownsAsync()
        {
            ViewBag.Professions = await _context.Professions
                .OrderBy(p => p.Name) // ако е Title – смени на Title
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
                .ToListAsync();

            ViewBag.Categories = await _context.MaterialCategories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

        }

        private async Task LoadCategoriesAsync()
        {
            ViewBag.Categories = await _context.MaterialCategories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();
        }

        public async Task<IActionResult> Details(int id)
            => View(await materials.GetDetailsAsync(User, id));

        public async Task<IActionResult> Edit(int id)
            => View(await materials.GetEditAsync(User, id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditMaterialVm model)
        {
            if (!ModelState.IsValid) return View(model);
            await materials.EditAsync(User, id, model);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await materials.DeleteAsync(User, id);
            return RedirectToAction(nameof(My));
        }


    }
}