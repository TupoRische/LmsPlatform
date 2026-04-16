using Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MaterialsController : Controller
    {
        private readonly IAdminDashboardService dashboard;

        public MaterialsController(IAdminDashboardService dashboard)
        {
            this.dashboard = dashboard;
        }

        public async Task<IActionResult> Index()
        {
            var model = await dashboard.GetMaterialsPageAsync();
            return View(model);
        }
    }
}
