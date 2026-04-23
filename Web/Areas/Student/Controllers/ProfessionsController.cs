using Core.Contracts;
using Core.ViewModels.Professions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Web.Areas.Student.Controllers
{
    [Area("Student")]
    public class ProfessionsController : Controller
    {
        private const string QuizSessionKey = "student.professions.quiz.saved-result";
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

            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrWhiteSpace(userId))
            {
                model.SavedResult = await professions.GetLatestQuizResultAsync(userId);
            }
            else
            {
                var sessionResult = HttpContext.Session.GetString(QuizSessionKey);

                if (!string.IsNullOrWhiteSpace(sessionResult))
                {
                    model.SavedResult = JsonSerializer.Deserialize<ProfessionQuizSavedResultVm>(sessionResult);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveQuizResult([FromBody] ProfessionQuizSubmissionVm submission)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await professions.ProcessQuizSubmissionAsync(submission, userId);

            if (result is null)
            {
                return BadRequest(new { message = "Невалидни отговори за въпросника." });
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                HttpContext.Session.SetString(QuizSessionKey, JsonSerializer.Serialize(result));
            }

            return Json(result);
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
