using Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TeacherRequestsController : Controller
    {
        private readonly IAdminTeacherRequestsService service;

        public TeacherRequestsController(IAdminTeacherRequestsService service)
            => this.service = service;

        public async Task<IActionResult> Index()
            => View(await service.GetPendingAsync());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(string id)
        {
            await service.ApproveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(string id)
        {
            await service.RejectAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
