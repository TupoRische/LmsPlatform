using Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProfessionsController : Controller
    {
        private readonly IAdminDashboardService dashboard;

        public ProfessionsController(IAdminDashboardService dashboard)
        {
            this.dashboard = dashboard;
        }

        public async Task<IActionResult> Index()
        {
            var model = await dashboard.GetProfessionsPageAsync();
            return View(model);
        }
    }
}
