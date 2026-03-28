using Core.Contracts;
using Core.ViewModels.School;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SchoolsController : Controller
    {
        private readonly ISchoolService schools;

        public SchoolsController(ISchoolService schools) => this.schools = schools;

        public async Task<IActionResult> Index()
            => View(await schools.GetAllAsync());

        public IActionResult Create() => View(new SchoolFormVm());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SchoolFormVm model)
        {
            if (!ModelState.IsValid) return View(model);

            await schools.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var s = await schools.GetByIdAsync(id);
            if (s == null) return NotFound();

            return View(new SchoolFormVm
            {
                Name = s.Name,
                City = s.City,
                Description = s.Description
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SchoolFormVm model)
        {
            if (!ModelState.IsValid) return View(model);

            await schools.UpdateAsync(id, model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var s = await schools.GetByIdAsync(id);
            if (s == null) return NotFound();
            return View(s);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ok = await schools.DeleteAsync(id);
            if (!ok)
            {
                TempData["Error"] = "Не може да изтриеш училище, което има професии/потребители.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
