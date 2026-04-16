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

        public async Task<IActionResult> Index(string city)
        {
            // 1. Вземаме всички училища (или филтрирани, ако услугата поддържа параметър)
            var allSchools = await schools.GetAllAsync();

            // 2. Извличаме уникалните градове за падащото меню
            ViewBag.Cities = allSchools
                .Select(s => s.City)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // 3. Филтрираме списъка, ако е избран конкретен град
            if (!string.IsNullOrEmpty(city))
            {
                allSchools = allSchools.Where(s => s.City == city);
            }

            return View(allSchools);
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