using Core.Contracts;
using Core.ViewModels.Admin;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminDashboardService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<AdminDashboardVm> GetStatsAsync()
        {
            var totalUsers = await userManager.Users.CountAsync();

            var pendingTeachers = await userManager.Users
                .CountAsync(u => u.RequestedTeacher && !u.IsApproved);

            var teachers = (await userManager.GetUsersInRoleAsync("Teacher")).Count;
            var students = (await userManager.GetUsersInRoleAsync("Student")).Count;

            var latestPending = await userManager.Users
                .Where(u => u.RequestedTeacher && !u.IsApproved)
                .OrderByDescending(u => u.CreatedOn)
                .Take(5)
                .Select(u => new SimpleUserVm
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName
                })
                .ToListAsync();

            return new AdminDashboardVm
            {
                TotalUsers = totalUsers,
                Teachers = teachers,
                Students = students,
                PendingTeachers = pendingTeachers,
                Schools = await context.Schools.CountAsync(),
                Professions = await context.Professions.CountAsync(),
                Materials = await context.Materials.CountAsync(),
                Comments = await context.Comments.CountAsync(),
                LatestPendingTeachers = latestPending,
            };
        }
    }
}
