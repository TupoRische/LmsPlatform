using Core.Contracts;
using Core.ViewModels.Teacher;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class TeacherDashboardService : ITeacherDashboardService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<Infrastructure.Identity.ApplicationUser> userManager;

        public TeacherDashboardService(ApplicationDbContext context, UserManager<Infrastructure.Identity.ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<TeacherDashboardVm> GetAsync(ClaimsPrincipal user)
        {
            var userId = userManager.GetUserId(user);

            var myMaterials = await context.Materials.CountAsync(m => m.TeacherId == userId);
            var myComments = await context.Comments.CountAsync(c => c.UserId == userId);

            var latest = await context.Materials
                .Where(m => m.TeacherId == userId)
                .OrderByDescending(m => m.CreatedOn)
                .Take(5)
                .Select(m => new LatestMaterialVm
                {
                    Id = m.Id,
                    Title = m.Title,
                    CreatedOn = m.CreatedOn
                })
                .ToListAsync();

            return new TeacherDashboardVm
            {
                MyMaterials = myMaterials,
                MyComments = myComments,
                LatestMaterials = latest
            };
        }
    }

}
