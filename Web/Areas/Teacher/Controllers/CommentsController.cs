using Core.Contracts;
using Core.ViewModels.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Areas.Teacher.Controllers
{
    [Authorize]
    [Area("Teacher")]
    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await commentService.GetAllForTeacherAsync(userId);
            return View(model);
        }


        [HttpGet]
        public IActionResult Details(int materialId)
        {

            return RedirectToAction("Details", "Materials", new { area = "", id = materialId });
        }
    }
}