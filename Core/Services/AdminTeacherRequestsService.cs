using Core.Contracts;
using Core.ViewModels.Admin.TeacherRequests;
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
    public class AdminTeacherRequestsService : IAdminTeacherRequestsService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;

        public AdminTeacherRequestsService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public async Task<TeacherRequestsPageVm> GetPendingAsync()
        {
            var usersWithRoles = await context.UserRoles
                .Select(ur => ur.UserId)
                .Distinct()
                .ToListAsync();

            var requests = await userManager.Users
                .Where(u => !u.IsApproved &&
                    !usersWithRoles.Contains(u.Id) &&
                    (u.RequestedTeacher || u.RequestedStudent))
                .OrderByDescending(u => u.CreatedOn)
                .Select(u => new TeacherRequestListItemVm
                {
                    Id = u.Id,
                    Email = u.Email!,
                    FullName = (u.FirstName + " " + u.LastName).Trim(),
                    RequestedRole = u.RequestedTeacher ? "Teacher" : "Student",
                    CreatedOn = u.CreatedOn
                })
                .ToListAsync();

            return new TeacherRequestsPageVm { Requests = requests };
        }

        public async Task ApproveAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId)
                       ?? throw new ArgumentException("User not found");

            var currentRoles = await userManager.GetRolesAsync(user);
            if (currentRoles.Any())
                throw new InvalidOperationException("User already has a role.");
            if (!user.RequestedTeacher && !user.RequestedStudent)
                throw new InvalidOperationException("User has no pending role request.");

            var role = user.RequestedTeacher ? "Teacher" : "Student";

            user.IsApproved = true;
            user.RequestedTeacher = false;
            user.RequestedStudent = false;

            await userManager.UpdateAsync(user);

            if (!await userManager.IsInRoleAsync(user, role))
                await userManager.AddToRoleAsync(user, role);
        }

        public async Task RejectAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId)
                       ?? throw new ArgumentException("User not found");

            user.IsApproved = false;
            user.RequestedTeacher = false;
            user.RequestedStudent = false;

            var currentRoles = await userManager.GetRolesAsync(user);
            if (currentRoles.Any())
                await userManager.RemoveFromRolesAsync(user, currentRoles);

            await userManager.UpdateAsync(user);
        }
    }
}
