using Core.Contracts;
using Core.ViewModels.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService comments;

        public CommentsController(ICommentService comments)
            => this.comments = comments;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int materialId, CommentFormVm model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Details", "Materials", new { id = materialId });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await comments.CreateAsync(materialId, userId, model.Content);

            return RedirectToAction("Details", "Materials", new { id = materialId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int materialId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (await comments.CanDeleteAsync(id, userId))
                await comments.DeleteAsync(id);

            return RedirectToAction("Details", "Materials", new { id = materialId });
        }
    }
}
