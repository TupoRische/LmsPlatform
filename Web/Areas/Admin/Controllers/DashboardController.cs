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

                RecentUsers = await dashboard.GetLastUsersAsync(),
                RandomTeachers = await dashboard.GetRandomTeachersAsync(),
                RandomStudents = await dashboard.GetRandomStudentsAsync(),
                PendingTeachersPreview = await dashboard.GetPendingTeachersPreviewAsync(),
                RandomSchools = await dashboard.GetRandomSchoolsAsync(),
                RandomProfessions = await dashboard.GetRandomProfessionsAsync(),

            };

            return View(model);
        }

    }
}
