using Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Student.Controllers
{
    [Area("Student")]
    public class SchoolsController : Controller
    {
        private readonly ISchoolService schools;

        public SchoolsController(ISchoolService schools)
            => this.schools = schools;

        public async Task<IActionResult> Index(string city, string sortOrder)
        {
            var allForFilter = await schools.GetAllAsync();
            ViewBag.Cities = allForFilter.Select(s => s.City).Distinct().OrderBy(c => c).ToList();

            ViewBag.CurrentCity = city;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CitySortParm = sortOrder == "city_asc" ? "city_desc" : "city_asc";

            var model = await schools.GetAllAsync(city, sortOrder);
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var model = await schools.GetByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }
    }
}