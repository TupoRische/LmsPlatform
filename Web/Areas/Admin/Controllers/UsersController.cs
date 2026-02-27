using Core.ViewModels.Admin.Users;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            // ❌ 1. Забрана за изтриване на себе си
            if (user.Id == _userManager.GetUserId(User))
                return BadRequest("Не можете да изтриете собствения си профил.");

            // ❌ 2. Забрана за изтриване на администратор
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return BadRequest("Администраторски профил не може да бъде изтрит.");

            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Teachers()
        {
            var teachers = await _userManager.GetUsersInRoleAsync("Teacher");

            var model = teachers.Select(t => new RecentUserVm
            {
                Id = t.Id,
                Email = t.Email!,
                Role = "Teacher"
            });

            return View(model);
        }
    }
}
