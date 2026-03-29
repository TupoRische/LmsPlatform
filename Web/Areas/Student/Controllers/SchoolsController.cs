using Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Student.Controllers
{
    [Area("Student")]
    public class SchoolsController : Controller
    {
        private readonly ISchoolService schools;

        public SchoolsController(ISchoolService schools)
            => this.schools = schools;

        public async Task<IActionResult> Index()
            => View(await schools.GetAllAsync());

        public async Task<IActionResult> Details(int id)
        {
            var model = await schools.GetByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }
    }

}
