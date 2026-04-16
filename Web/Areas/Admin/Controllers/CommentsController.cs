using Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await commentService.GetAllThreadsAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int materialId)
        {
            var model = await commentService.GetCommentsByMaterialAsync(materialId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int commentId, int materialId)
        {
            await commentService.DeleteAsync(commentId);
            return RedirectToAction(nameof(Details), new { materialId });
        }
    }
}
