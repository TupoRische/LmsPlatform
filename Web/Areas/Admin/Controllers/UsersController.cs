using Core.Contracts;
using Core.Services;
using Core.ViewModels.Admin.Teachers;
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
        private readonly IAdminDashboardService service;

        public UsersController(UserManager<ApplicationUser> userManager, IAdminDashboardService service)
        {
            _userManager = userManager;
            this.service = service;
        }

<<<<<<< HEAD
        public async Task<IActionResult> Index()
        {
            var model = await service.GetUsersPageAsync();
            return View(model);
=======
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            if (user.Id == _userManager.GetUserId(User))
                return BadRequest("Не можете да изтриете собствения си профил.");

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return BadRequest("Администраторски профил не може да бъде изтрит.");

            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Materials(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var materials = await service.GetTeacherMaterialsAsync(id);

            return View(materials);
        }

        public async Task<IActionResult> Teachers()
        {
            var model = await service.GetTeachersAsync();
            return View(model);
        }

<<<<<<< HEAD
        public async Task<IActionResult> Students()
        {
            var model = await service.GetStudentsPageAsync();
            return View(model);
        }

=======
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
        [HttpGet]
        public async Task<IActionResult> TeacherDetails(string id)
        {
            var teacher = await service.GetTeacherDetailsAsync(id);

            if (teacher == null)
                return NotFound();

            return View("Details", teacher);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await service.GetTeacherForEditAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TeacherEditVm model) 
        {
            if (!ModelState.IsValid)
            {
                model.Schools = await service.GetSchoolsAsync();
                return View(model);
            }
            await service.UpdateTeacherAsync(model);

            return RedirectToAction(nameof(Teachers));
        }

    }
}
