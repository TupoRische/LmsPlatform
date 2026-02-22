using Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.ViewModels.Materials;
using System.Security.Claims;

namespace Web.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class MaterialsController : Controller 
    {
        private readonly IMaterialService materials;
        public MaterialsController(IMaterialService materials) => this.materials = materials;

        public async Task<IActionResult> Index()
            => View(await materials.GetMineAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialFormVm model)
        {
            if (!ModelState.IsValid) return View(model);

            await materials.CreateAsync(model, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await materials.GetForEditAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaterialFormVm model)
        {
            if (!ModelState.IsValid) return View(model);

            var ok = await materials.UpdateAsync(id, model, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!ok) return Forbid();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await materials.DeleteAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!ok) return Forbid();

            return RedirectToAction(nameof(Index));
        }
    }
}