using Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Student.Controllers
{
    [Area("Student")]
    public class ProfessionsController : Controller
    {
        private readonly IProfessionService professions;
        private readonly ISchoolService schools;

        public ProfessionsController(IProfessionService professions, ISchoolService schools)
        {
            this.professions = professions;
            this.schools = schools;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? schoolId, string sortOrder)
        {
            ViewBag.Schools = await schools.GetDropdownAsync();

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentSchoolId = schoolId;

            var model = await professions.GetAllAsync(schoolId, null);

            if (sortOrder == "name_desc")
            {
                model = model.OrderByDescending(p => p.Name).ToList();
            }
            else
            {
                model = model.OrderBy(p => p.Name).ToList();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Quiz()
        {
            var model = await professions.GetQuizAsync();
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var model = await professions.GetByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }
    }
}
