using Core.Contracts;
using Core.ViewModels.Teacher;
using Infrastructure.Data;
using Infrastructure.Identity; // Увери се, че това е тук за ApplicationUser
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Core.Services
{
    public class TeacherDashboardService : ITeacherDashboardService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public TeacherDashboardService(
            ApplicationDbContext _context,
            UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

        public async Task<TeacherDashboardVm> GetAsync(ClaimsPrincipal user)
        {
            // Вече userManager няма да е null и този ред ще работи:
            var userId = userManager.GetUserId(user);

            // 1. Списък за материалите
            var materials = await context.Materials
                .Include(m => m.MaterialCategory)
                .Where(m => m.TeacherId == userId)
                .OrderByDescending(m => m.CreatedOn)
                .Select(m => new TeacherMaterialListItemVm
                {
                    Id = m.Id,
                    Title = m.Title,
                    CategoryName = m.MaterialCategory != null ? m.MaterialCategory.Name : "Общи",
                    CreatedOn = m.CreatedOn
                })
                .Take(3)
                .ToListAsync();

            // 2. Списък за коментарите с имената на студентите
            var comments = await context.Comments
                .Include(c => c.User)
                .Include(c => c.Material)
                .Where(c => c.Material.TeacherId == userId)
                .OrderByDescending(c => c.CreatedOn)
                .Select(c => new TeacherCommentVm
                {
                    Id = c.Id,
                    AuthorName = $"{c.User.FirstName} {c.User.LastName}",
                    Content = c.Content,
                    CreatedOn = c.CreatedOn
                })
                .Take(3)
                .ToListAsync();

            // 3. Данни за последна активност
            var lastM = await context.Materials
                .Where(m => m.TeacherId == userId)
                .OrderByDescending(m => m.CreatedOn)
                .FirstOrDefaultAsync();

            var lastC = await context.Comments
                .Include(c => c.User)
                .Where(c => c.Material.TeacherId == userId)
                .OrderByDescending(c => c.CreatedOn)
                .FirstOrDefaultAsync();

            return new TeacherDashboardVm
            {
                MaterialsCount = await context.Materials.CountAsync(m => m.TeacherId == userId),
                NewCommentsCount = await context.Comments.CountAsync(c => c.Material.TeacherId == userId),

                RecentMaterials = materials,
                RecentComments = comments,

                LastMaterial = lastM != null ? lastM.Title : "Няма качени материали",
                LastComment = lastC != null
                    ? $"От {lastC.User.FirstName} {lastC.User.LastName}: {lastC.Content}"
                    : "Няма нови коментари"
            };
        }
    }
}