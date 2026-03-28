using Core.Contracts;
using Core.ViewModels.Admin.TeacherRequests;
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

        public AdminTeacherRequestsService(UserManager<ApplicationUser> userManager)
            => this.userManager = userManager;

        public async Task<TeacherRequestsPageVm> GetPendingAsync()
        {
            var requests = await userManager.Users
                .Where(u => u.RequestedTeacher && !u.IsApproved)
                .OrderByDescending(u => u.CreatedOn)
                .Select(u => new TeacherRequestListItemVm
                {
                    Id = u.Id,
                    Email = u.Email!,
                    FullName = (u.FirstName + " " + u.LastName).Trim(),
                    CreatedOn = u.CreatedOn
                })
                .ToListAsync();

            return new TeacherRequestsPageVm { Requests = requests };
        }

        public async Task ApproveAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId)
                       ?? throw new ArgumentException("User not found");

            user.IsApproved = true;
            user.RequestedTeacher = false;

            await userManager.UpdateAsync(user);

            if (!await userManager.IsInRoleAsync(user, "Teacher"))
                await userManager.AddToRoleAsync(user, "Teacher");
        }

        public async Task RejectAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId)
                       ?? throw new ArgumentException("User not found");

            user.IsApproved = false;
            user.RequestedTeacher = false;

            await userManager.UpdateAsync(user);
        }
    }
}
