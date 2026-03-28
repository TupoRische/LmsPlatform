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
<<<<<<< HEAD
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
=======
            var model = new AdminDashboardVm
            {
                UsersCount = await dashboard.GetUsersCountAsync(),
                Teachers = await dashboard.GetTeachersCountAsync(),
                Students = await dashboard.GetStudentsCountAsync(),
                PendingTeachers = await dashboard.GetPendingTeachersCountAsync(),
                Schools = await dashboard.GetSchoolsCountAsync(),
                Professions = await dashboard.GetProfessionsCountAsync(),
                Materials = await dashboard.GetMaterialsCountAsync(),
                Comments = await dashboard.GetCommentsCountAsync(),
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56

                RecentUsers = await dashboard.GetLastUsersAsync(),
                RandomTeachers = await dashboard.GetRandomTeachersAsync(),
                RandomStudents = await dashboard.GetRandomStudentsAsync(),
                PendingTeachersPreview = await dashboard.GetPendingTeachersPreviewAsync(),
                RandomSchools = await dashboard.GetRandomSchoolsAsync(),
                RandomProfessions = await dashboard.GetRandomProfessionsAsync(),
<<<<<<< HEAD
                RandomMaterials = await dashboard.GetRandomMaterialsAsync(),
                RandomCommentThreads = await dashboard.GetRandomCommentThreadsAsync(),
=======

>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
            };

            return View(model);
        }

    }
}
