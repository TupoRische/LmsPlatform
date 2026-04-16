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
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int materialId, CommentFormVm model)
        {
            if (!ModelState.IsValid)
            {
                TempData["CommentError"] = "Comment must contain text and be up to 1000 characters.";
                return RedirectToAction("Details", "Materials", new { id = materialId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            try
            {
                await commentService.CreateAsync(materialId, userId, model.Content.Trim(), model.ParentCommentId);
            }
            catch (ArgumentException)
            {
                TempData["CommentError"] = "Comment must contain text and be up to 1000 characters.";
            }
            catch (InvalidOperationException)
            {
                TempData["CommentError"] = "The comment you tried to reply to was not found.";
            }

            return RedirectToAction("Details", "Materials", new { id = materialId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int commentId, int materialId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            if (User.IsInRole("Admin") || await commentService.CanDeleteAsync(commentId, userId))
            {
                await commentService.DeleteAsync(commentId);
            }

            return RedirectToAction("Details", "Materials", new { id = materialId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int commentId, int materialId, CommentFormVm model)
        {
            if (!ModelState.IsValid)
            {
                TempData["CommentError"] = "Коментарът трябва да съдържа текст и да е до 1000 символа.";
                return RedirectToAction("Details", "Materials", new { id = materialId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            try
            {
                await commentService.UpdateAsync(commentId, userId, model.Content.Trim());
            }
            catch (ArgumentException)
            {
                TempData["CommentError"] = "Коментарът трябва да съдържа текст и да е до 1000 символа.";
            }
            catch (InvalidOperationException)
            {
                TempData["CommentError"] = "Коментарът, който се опитваш да редактираш, не беше намерен.";
            }
            catch (UnauthorizedAccessException)
            {
                TempData["CommentError"] = "Можеш да редактираш само собствените си коментари.";
            }

            return RedirectToAction("Details", "Materials", new { id = materialId });
        }
    }
}
