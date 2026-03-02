using Core.Contracts;
using Core.ViewModels.Admin;
using Core.ViewModels.Admin.Professions;
using Core.ViewModels.Admin.Schools;
using Core.ViewModels.Admin.Students;
using Core.ViewModels.Admin.Teachers;
using Core.ViewModels.Admin.Users;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Identity;
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories.Implementations;
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
            private readonly IRepository<ApplicationUser> userRepository;
            private readonly IRepository<Material> materialRepository;
            private readonly IRepository<School> schoolRepository;

        public AdminDashboardService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IRepository<ApplicationUser> userRepository, IRepository<Material> materialRepository, IRepository<School> schoolRepository)
        {
            this.context = context;
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.materialRepository = materialRepository;
            this.schoolRepository = schoolRepository;
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

                public async Task<IEnumerable<RecentUserVm>> GetLastUsersAsync(int count = 3)
                {
                    var users = await userManager.Users
                        .OrderByDescending(u => u.CreatedOn) 
                        .Take(count)
                        .ToListAsync();

                    var result = new List<RecentUserVm>();

                    foreach (var user in users)
                    {
                        var roles = await userManager.GetRolesAsync(user);

                        result.Add(new RecentUserVm
                        {
                            Id = user.Id,
                            FullName = $"{user.FirstName} {user.LastName}",
                            Email = user.Email!,
                            Role = roles.FirstOrDefault() ?? "No role"
                        });
                }

                return result;
                }

            public async Task<IEnumerable<RandomTeacherVm>>
            GetRandomTeachersAsync(int count = 3)
            {
                var teachers = await userManager.GetUsersInRoleAsync("Teacher");

                return teachers
                    .OrderBy(x => Guid.NewGuid())
                    .Take(count)
                    .Select(t => new RandomTeacherVm
                    {
                        Id = t.Id,
                        FullName = $"{t.FirstName} {t.LastName}",
                        Email = t.Email!
                    })
                    .ToList();
            }

            public async Task<IEnumerable<RandomStudentVm>>
            GetRandomStudentsAsync(int count = 3)
                {
                    var students = await userManager.GetUsersInRoleAsync("Student");

                    return students
                        .OrderBy(x => Guid.NewGuid())
                        .Take(count)
                        .Select(s => new RandomStudentVm
                        {
                            Id = s.Id,
                            FullName = $"{s.FirstName} {s.LastName}"
                        })
                        .ToList();
                }

        public async Task<IEnumerable<PendingTeacherVm>> GetPendingTeachersPreviewAsync(int count = 3)
        {
            return await userManager.Users
                .Where(u => u.RequestedTeacher && !u.IsApproved)
                .OrderByDescending(u => u.CreatedOn)
                .Take(count)
                .Select(u => new PendingTeacherVm
                {
                    Id = u.Id,
                    FullName = $"{u.FirstName} {u.LastName}",
                    Email = u.Email!,
                    RequestedRole = "Teacher"
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<RandomSchoolVm>> GetRandomSchoolsAsync(int count = 3)
        {
            return await context.Schools
                .OrderBy(x => Guid.NewGuid())
                .Take(count)
                .Select(s => new RandomSchoolVm
                {
                    Id = s.Id,
                    Abbreviation = s.Abbreviation,
                    City = s.City
                })
                .ToListAsync(); 
        }

        public async Task<IEnumerable<RandomProfessionVm>>
             GetRandomProfessionsAsync(int count = 3)
            {
                return await context.Professions
                    .OrderBy(x => Guid.NewGuid())
                    .Take(count)
                    .Select(p => new RandomProfessionVm
                    {
                        Id = p.Id,
                        Name = p.Name
                    })
                    .ToListAsync();
            }

            public async Task<IEnumerable<TeacherListVm>> GetTeachersAsync()
            {
                return await userRepository
                    .AllReadonly()
                    .Include(u => u.School)
                    .Where(u => u.IsApproved)
                    .Where(u => context.UserRoles.Any(r =>
                        r.UserId == u.Id &&
                        context.Roles.Any(role =>
                            role.Id == r.RoleId &&
                            role.Name == "Teacher")))
                    .Select(u => new TeacherListVm
                    {
                        Id = u.Id,
                        FullName = u.FirstName + " " + u.LastName,
                        School = u.School != null
        ? u.School.Abbreviation + " - " + u.School.City
        : ""
                    })
                    .ToListAsync();
            }

        public async Task<IEnumerable<TeacherMaterialVm>> GetTeacherMaterialsAsync(string teacherId)
        {
            if (string.IsNullOrEmpty(teacherId))
                return new List<TeacherMaterialVm>();

            return await materialRepository
                .All()
                .Where(m => m.TeacherId == teacherId)
                .Include(m => m.Profession)
                .Include(m => m.MaterialCategory)
                .Select(m => new TeacherMaterialVm
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    Profession = m.Profession != null ? m.Profession.Name : "",
                    Category = m.MaterialCategory != null ? m.MaterialCategory.Name : "",
                    CreatedOn = m.CreatedOn
                })
                .ToListAsync();
        }


        public async Task<TeacherDetailsVm?> GetTeacherDetailsAsync(string id)
        {
            return await userRepository.AllReadonly()
                .Where(u => u.Id == id)
                .Select(u => new TeacherDetailsVm
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email ?? "",
                    School = u.School != null ? u.School.Abbreviation : "",
                    City = u.School != null ? u.School.City : ""
                })
                .FirstOrDefaultAsync();
        }

        public async Task<TeacherEditVm> GetTeacherForEditAsync(string id)
        {
            var teacher = await userManager.Users
                .FirstAsync(u => u.Id == id);

            return new TeacherEditVm
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email,
                SchoolId = teacher.SchoolId,
                Schools = await GetSchoolsAsync()
            };
        }

        public async Task UpdateTeacherAsync(TeacherEditVm model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.SchoolId = model.SchoolId;

            await userManager.UpdateAsync(user);
        }

        public async Task<IEnumerable<SchoolOptionVm>> GetSchoolsAsync()
        {
            return await schoolRepository
                .All()
                .Select(s => new SchoolOptionVm
                {
                    Id = s.Id,
                    Name = s.Abbreviation + " - " + s.City
                })
                .ToListAsync();
        }

        public async Task<int> GetUsersCountAsync()
            {
                return await userManager.Users.CountAsync();
            }

            public async Task<int> GetTeachersCountAsync()
            {
                var teachers = await userManager.GetUsersInRoleAsync("Teacher");
                return teachers.Count;
            }

            public async Task<int> GetStudentsCountAsync()
            {
                var students = await userManager.GetUsersInRoleAsync("Student");
                return students.Count;
            }

            public async Task<int> GetPendingTeachersCountAsync()
            {
                return await userManager.Users
                    .Where(u => u.RequestedTeacher && !u.IsApproved)
                    .CountAsync();
            }

            public async Task<int> GetSchoolsCountAsync()
            {
                return await context.Schools.CountAsync();
            }

            public async Task<int> GetProfessionsCountAsync()
            {
                return await context.Professions.CountAsync();
            }

            public async Task<int> GetMaterialsCountAsync()
            {
                return await context.Materials.CountAsync();
            }

            public async Task<int> GetCommentsCountAsync()
            {
                return await context.Comments.CountAsync();
            }
        }
}
