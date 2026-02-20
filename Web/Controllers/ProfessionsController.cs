using Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{   
    public class ProfessionsController : Controller
        {
            private readonly IProfessionService professions;
            private readonly ISchoolService schools;

            public ProfessionsController(IProfessionService professions, ISchoolService schools)
            {
                this.professions = professions;
                this.schools = schools;
            }

            public async Task<IActionResult> Index(int? schoolId)
            {
                ViewBag.Schools = await schools.GetDropdownAsync();
                ViewBag.SelectedSchoolId = schoolId;

                var model = await professions.GetAllAsync(schoolId);
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