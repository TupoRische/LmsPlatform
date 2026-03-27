using Core.Contracts;

using Core.Services;

using Core.ViewModels.Comments;

using Infrastructure.Data;

using Infrastructure.Identity;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using System.Security.Claims;



namespace Web.Controllers

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

        // Това ще отваря списъка с всички нишки
        public async Task<IActionResult> Index()
        {
            var model = await commentService.GetAllThreadsAsync();
            return View(model);
        }

        // Вече съществуващият ти метод за детайли на конкретен материал
        public async Task<IActionResult> Details(int materialId)
        {
            var model = await commentService.GetCommentsByMaterialAsync(materialId);
            return View(model);
        }
    
    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int commentId, int materialId)
        {
            // Тук не проверяваме CanDeleteAsync, защото администраторът може да трие всичко
            await commentService.DeleteAsync(commentId);

            // Връщаме администратора в същия разговор
            return RedirectToAction(nameof(Details), new { materialId });
        }

    } 
}