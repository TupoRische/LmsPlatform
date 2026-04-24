using Core.Contracts;
using Core.ViewModels.Admin.Teachers;
using Core.ViewModels.Teacher;
using Core.ViewModels.Teacher.Materials;
using Infrastructure.Data;
using Infrastructure.Identity; 
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
            var userId = userManager.GetUserId(user);

            // Списък за материалите
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

            // Списък за коментарите с имената на студентите
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

            // Данни за последна активност
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

        public async Task<TeacherMaterialsPageVm> GetTeacherMaterialsAsync(ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            var materialsQuery = context.Materials
                .Include(m => m.MaterialCategory)
                .Include(m => m.Profession)
                .Include(m => m.Comments)
                .Where(m => m.TeacherId == userId);

            var materialsList = await materialsQuery
                .OrderByDescending(m => m.CreatedOn)
                .Select(m => new TeacherMaterialDetailsVm
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    CreatedOn = m.CreatedOn,
                    CategoryName = m.MaterialCategory.Name,
                    ProfessionName = m.Profession.Name,
                    CommentsCount = m.Comments.Count,
                    TeacherName = $"{m.Teacher.FirstName} {m.Teacher.LastName}"
                })
                .ToListAsync();

            return new TeacherMaterialsPageVm
            {
                TotalMaterials = materialsList.Count,
                WithFilesCount = await materialsQuery.CountAsync(m => m.FilePath != null),
                WithExternalLinksCount = await materialsQuery.CountAsync(m => m.FilePath != null),
                TotalComments = materialsList.Sum(m => m.CommentsCount),
                Materials = materialsList
            };
        }

        public async Task<TeacherCommentsPageVm> GetAllCommentsForTeacherAsync(string userId)
        {
            var comments = await context.Comments
                .Where(c => c.Material.TeacherId == userId)
                .OrderByDescending(c => c.CreatedOn)
                .Select(c => new TeacherCommentVm
                {
                    Id = c.Id,
                    AuthorName = $"{c.User.FirstName} {c.User.LastName}",
                    Content = c.Content,
                    CreatedOn = c.CreatedOn,
                })
                .ToListAsync();

            return new TeacherCommentsPageVm
            {
                TotalComments = comments.Count,
                NewCommentsToday = comments.Count(c => c.CreatedOn.Date == DateTime.Today),
                Comments = comments
            };
        }
    }
}