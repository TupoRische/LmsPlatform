<<<<<<< HEAD
using Core.Contracts;
using Core.ViewModels.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize]
=======
﻿using Core.Contracts;

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
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

<<<<<<< HEAD
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
=======
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
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
