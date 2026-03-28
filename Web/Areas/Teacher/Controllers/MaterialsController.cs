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
    [Authorize(Roles = "Teacher,Admin")]
    public class MaterialsController : Controller
    {
        private readonly ITeacherMaterialsService materials;
        private readonly IWebHostEnvironment _env;

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
            var form = await materials.GetCreateFormAsync();
            return View(form);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMaterialFormVm form)
        {
            // form.Material е реалният CreateMaterialVm
            if (!ModelState.IsValid)
            {
                var reload = await materials.GetCreateFormAsync();
                reload.Material = form.Material;   // запазваш въведеното
                return View(reload);
            }

            string? filePath = null;
            var model = form.Material;

            if (model.File != null && model.File.Length > 0)
            {
                if (model.File.Length > 20 * 1024 * 1024)
                {
                    ModelState.AddModelError("Material.File", "Файлът е прекалено голям (макс 20MB).");

                    var reload = await materials.GetCreateFormAsync();
                    reload.Material = form.Material;
                    return View(reload);
                }

                var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "materials");
                Directory.CreateDirectory(uploadsDir);

                var ext = Path.GetExtension(model.File.FileName);
                var safeName = $"{Guid.NewGuid():N}{ext}";
                var fullPath = Path.Combine(uploadsDir, safeName);

                await using var fs = System.IO.File.Create(fullPath);
                await model.File.CopyToAsync(fs);

                filePath = $"/uploads/materials/{safeName}";
            }

            var id = await materials.CreateAsync(User, model, filePath);
            return RedirectToAction(nameof(Details), new { id });
        }



        //private async Task LoadDropdownsAsync()
        //{
        //    ViewBag.Professions = new List<SelectListItem>(); // TODO
        //    ViewBag.Categories = new List<SelectListItem>();  // TODO
        //}

        public async Task<IActionResult> Details(int id)
            => View(await materials.GetDetailsAsync(User, id));

        public async Task<IActionResult> Edit(int id)
            => View(await materials.GetEditAsync(User, id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditMaterialVm model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            string? newFilePath = null;

            if (model.File != null && model.File.Length > 0)
            {
                if (model.File.Length > 20 * 1024 * 1024)
                {
                    ModelState.AddModelError("File", "Файлът е прекалено голям (макс 20MB).");
                }
                else
                {
                    var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "materials");
                    Directory.CreateDirectory(uploadsDir);

                    var ext = Path.GetExtension(model.File.FileName);
                    var safeName = $"{Guid.NewGuid():N}{ext}";
                    var fullPath = Path.Combine(uploadsDir, safeName);

                    await using var fs = System.IO.File.Create(fullPath);
                    await model.File.CopyToAsync(fs);

                    newFilePath = $"/uploads/materials/{safeName}";
                }
            }

            if (!ModelState.IsValid)
            {
                var form = await materials.GetCreateFormAsync();
                model.Professions = form.Professions;
                model.Categories = form.Categories;
                return View(model);
            }

            await materials.EditAsync(User, id, model, newFilePath);

            TempData["SuccessMessage"] = "Материалът е редактиран успешно.";
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