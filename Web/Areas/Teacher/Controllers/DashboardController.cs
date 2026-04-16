using Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher, Admin")]
    public class DashboardController : Controller
    {
        private readonly ITeacherDashboardService dashboard;

        public DashboardController(ITeacherDashboardService dashboard)
            => this.dashboard = dashboard;

        public async Task<IActionResult> Index()
            => View(await dashboard.GetAsync(User));

        public async Task<IActionResult> MyMaterials()
        {
            var model = await dashboard.GetTeacherMaterialsAsync(User);
            return View(model);
        }
    }
}