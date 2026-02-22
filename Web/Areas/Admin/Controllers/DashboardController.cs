using Core.Contracts;
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
            => View(await dashboard.GetStatsAsync());
    }
}
