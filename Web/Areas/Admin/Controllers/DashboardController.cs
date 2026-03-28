using Core.Contracts;
using Core.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IAdminDashboardService dashboard;

        public DashboardController(IAdminDashboardService dashboard)
            => this.dashboard = dashboard;

        public async Task<IActionResult> Index()
        {
            var stats = await dashboard.GetStatsAsync();

            var model = new AdminDashboardVm
            {
                UsersCount = stats.TotalUsers,
                Teachers = stats.Teachers,
                Students = stats.Students,
                PendingTeachers = stats.PendingTeachers,
                Schools = stats.Schools,
                Professions = stats.Professions,
                Materials = stats.Materials,
                Comments = stats.Comments,

                RecentUsers = await dashboard.GetLastUsersAsync(),
                RandomTeachers = await dashboard.GetRandomTeachersAsync(),
                RandomStudents = await dashboard.GetRandomStudentsAsync(),
                PendingTeachersPreview = await dashboard.GetPendingTeachersPreviewAsync(),
                RandomSchools = await dashboard.GetRandomSchoolsAsync(),
                RandomProfessions = await dashboard.GetRandomProfessionsAsync(),
                RandomMaterials = await dashboard.GetRandomMaterialsAsync(),
                RandomCommentThreads = await dashboard.GetRandomCommentThreadsAsync(),
            };

            return View(model);
        }

    }
}
