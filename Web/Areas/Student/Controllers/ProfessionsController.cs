using Core.Contracts;
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
        public async Task<IActionResult> Index(int? schoolId, string professionName)
        {
            ViewBag.Schools = await schools.GetDropdownAsync();

            var allProfessions = await professions.GetAllAsync(null, null);
            ViewBag.ProfessionNames = allProfessions
                .Select(p => p.Name)
                .Distinct()
                .OrderBy(p => p)
                .ToList();

            var model = await professions.GetAllAsync(schoolId, professionName);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Quiz()
        {
            var model = await professions.GetQuizAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await professions.GetByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }
    }
}
