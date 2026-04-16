using Core.Contracts;
using Core.ViewModels.Admin;
using Core.ViewModels.Admin.Professions;
using Core.ViewModels.Admin.Schools;
using Core.ViewModels.Admin.Students;
using Core.ViewModels.Admin.Teachers;
using Core.ViewModels.Admin.Users;
using Core.ViewModels.Admin.Materials;
using Core.ViewModels.Comments;
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

                var teacherRoleId = await context.Roles
                    .Where(r => r.Name == "Teacher")
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync();

                var studentRoleId = await context.Roles
                    .Where(r => r.Name == "Student")
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync();

                var teachers = string.IsNullOrWhiteSpace(teacherRoleId)
                    ? 0
                    : await context.UserRoles.CountAsync(ur => ur.RoleId == teacherRoleId);

                var students = string.IsNullOrWhiteSpace(studentRoleId)
                    ? 0
                    : await context.UserRoles.CountAsync(ur => ur.RoleId == studentRoleId);

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
                const int previewPoolSize = 12;
                var teacherRoleId = await context.Roles
                    .Where(r => r.Name == "Teacher")
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrWhiteSpace(teacherRoleId))
                {
                    return new List<RandomTeacherVm>();
                }

                var teachers = await context.Users
                    .AsNoTracking()
                    .Where(u => context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == teacherRoleId))
                    .OrderByDescending(u => u.CreatedOn)
                    .Take(Math.Max(count * 4, previewPoolSize))
                    .Select(t => new RandomTeacherVm
                    {
                        Id = t.Id,
                        FullName = $"{t.FirstName} {t.LastName}",
                        Email = t.Email!
                    })
                    .ToListAsync();

                return teachers
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(count)
                    .ToList();
            }

            public async Task<IEnumerable<RandomStudentVm>>
            GetRandomStudentsAsync(int count = 3)
                {
                    const int previewPoolSize = 12;
                    var studentRoleId = await context.Roles
                        .Where(r => r.Name == "Student")
                        .Select(r => r.Id)
                        .FirstOrDefaultAsync();

                    if (string.IsNullOrWhiteSpace(studentRoleId))
                    {
                        return new List<RandomStudentVm>();
                    }

                    var students = await context.Users
                        .AsNoTracking()
                        .Where(u => context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == studentRoleId))
                        .OrderByDescending(u => u.CreatedOn)
                        .Take(Math.Max(count * 4, previewPoolSize))
                        .Select(s => new RandomStudentVm
                        {
                            Id = s.Id,
                            FullName = $"{s.FirstName} {s.LastName}"
                        })
                        .ToListAsync();

                    return students
                        .OrderBy(_ => Guid.NewGuid())
                        .Take(count)
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
            const int previewPoolSize = 12;

            var schools = await context.Schools
                .AsNoTracking()
                .OrderByDescending(s => s.Id)
                .Take(Math.Max(count * 4, previewPoolSize))
                .Select(s => new RandomSchoolVm
                {
                    Id = s.Id,
                    Abbreviation = s.Abbreviation,
                    City = s.City
                })
                .ToListAsync();

            return schools
                .OrderBy(_ => Guid.NewGuid())
                .Take(count)
                .ToList();
        }

        public async Task<IEnumerable<RandomProfessionVm>>
             GetRandomProfessionsAsync(int count = 3)
            {
                const int previewPoolSize = 12;

                var professions = await context.Professions
                    .AsNoTracking()
                    .OrderByDescending(p => p.Id)
                    .Take(Math.Max(count * 4, previewPoolSize))
                    .Select(p => new RandomProfessionVm
                    {
                        Id = p.Id,
                        Name = p.Name
                    })
                    .ToListAsync();

                return professions
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(count)
                    .ToList();
            }

        public async Task<AdminUsersPageVm> GetUsersPageAsync()
        {
            var users = await context.Users
                .AsNoTracking()
                .OrderByDescending(u => u.CreatedOn)
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.SchoolId,
                    SchoolName = u.School != null ? u.School.Abbreviation + " - " + u.School.City : "Без училище",
                    u.CreatedOn,
                    u.IsApproved,
                    u.RequestedTeacher
                })
                .ToListAsync();

            var roleLookup = await context.UserRoles
                .Join(context.Roles,
                    userRole => userRole.RoleId,
                    role => role.Id,
                    (userRole, role) => new { userRole.UserId, RoleName = role.Name! })
                .ToListAsync();

            var mappedUsers = users
                .Select(u => new AdminUserListItemVm
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName,
                    Email = u.Email ?? "",
                    Role = roleLookup.FirstOrDefault(r => r.UserId == u.Id)?.RoleName ?? "Без роля",
                    SchoolName = u.SchoolName,
                    CreatedOn = u.CreatedOn,
                    IsApproved = u.IsApproved
                })
                .ToList();

            return new AdminUsersPageVm
            {
                TotalUsers = mappedUsers.Count,
                AdminsCount = mappedUsers.Count(u => u.Role == "Admin"),
                TeachersCount = mappedUsers.Count(u => u.Role == "Teacher"),
                StudentsCount = mappedUsers.Count(u => u.Role == "Student"),
                PendingTeachersCount = users.Count(u => u.RequestedTeacher && !u.IsApproved),
                Users = mappedUsers
            };
        }

        public async Task<StudentsPageVm> GetStudentsPageAsync()
        {
            var studentRoleId = await context.Roles
                .Where(r => r.Name == "Student")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(studentRoleId))
            {
                return new StudentsPageVm();
            }

            var students = await context.Users
                .AsNoTracking()
                .Where(u => context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == studentRoleId))
                .OrderByDescending(u => u.CreatedOn)
                .Select(u => new StudentListItemVm
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName,
                    Email = u.Email ?? "",
                    SchoolName = u.School != null
                        ? u.School.Abbreviation + " - " + u.School.City
                        : "Без училище",
                    HasSchool = u.SchoolId.HasValue,
                    CreatedOn = u.CreatedOn,
                    CommentsCount = u.Comments.Count,
                    QuizResultsCount = u.QuizResults.Count
                })
                .ToListAsync();

            return new StudentsPageVm
            {
                TotalStudents = students.Count,
                WithSchoolCount = students.Count(s => s.HasSchool),
                WithoutSchoolCount = students.Count(s => !s.HasSchool),
                TotalComments = students.Sum(s => s.CommentsCount),
                Students = students
            };
        }

        public async Task<ProfessionsPageVm> GetProfessionsPageAsync()
        {
            var professions = await context.Professions
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .Select(p => new ProfessionListItemVm
                {
                    Id = p.Id,
                    Name = p.Name,
                    SchoolName = p.School.Abbreviation + " - " + p.School.City,
                    Description = p.Description,
                    MaterialsCount = p.Materials.Count
                })
                .ToListAsync();

            return new ProfessionsPageVm
            {
                TotalProfessions = professions.Count,
                SchoolsRepresented = professions
                    .Select(p => p.SchoolName)
                    .Distinct()
                    .Count(),
                WithMaterialsCount = professions.Count(p => p.MaterialsCount > 0),
                TotalMaterials = professions.Sum(p => p.MaterialsCount),
                Professions = professions
            };
        }

        public async Task<AdminMaterialsPageVm> GetMaterialsPageAsync()
        {
            var materials = await context.Materials
                .AsNoTracking()
                .OrderByDescending(m => m.CreatedOn)
                .Select(m => new AdminMaterialListItemVm
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    ProfessionName = m.Profession.Name,
                    CategoryName = m.MaterialCategory.Name,
                    TeacherName = m.Teacher.FirstName + " " + m.Teacher.LastName,
                    CommentsCount = m.Comments.Count,
                    HasFile = !string.IsNullOrWhiteSpace(m.FilePath),
                    HasExternalUrl = !string.IsNullOrWhiteSpace(m.Url),
                    CreatedOn = m.CreatedOn
                })
                .ToListAsync();

            return new AdminMaterialsPageVm
            {
                TotalMaterials = materials.Count,
                WithFilesCount = materials.Count(m => m.HasFile),
                WithExternalLinksCount = materials.Count(m => m.HasExternalUrl),
                TotalComments = materials.Sum(m => m.CommentsCount),
                Materials = materials
            };
        }

            public async Task<IEnumerable<TeacherListVm>> GetTeachersAsync()
            {
                return await userRepository
                    .AllReadonly()
                    .Include(u => u.School)
                    .Include(u => u.MaterialsCreated)
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
                        : "",
                        MaterialsCount = u.MaterialsCreated.Count()
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

        public async Task<UserEditVm> GetTeacherForEditAsync(string id)
        {
            var teacher = await userManager.Users
                .FirstAsync(u => u.Id == id);

            return new UserEditVm
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email,
                SchoolId = teacher.SchoolId,
                Schools = await GetSchoolsAsync()
            };
        }

        public async Task UpdateTeacherAsync(UserEditVm model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.SchoolId = model.SchoolId;

            await userManager.UpdateAsync(user);
        }

        public async Task<UserEditVm> GetStudentForEditAsync(string id)
        {
            var student = await userManager.Users
                .FirstAsync(u => u.Id == id);

            return new UserEditVm
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                SchoolId = student.SchoolId,
                Schools = await GetSchoolsAsync()
            };
        }

        public async Task UpdateStudentAsync(UserEditVm model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.SchoolId = model.SchoolId;

                await userManager.UpdateAsync(user);
            }
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

        public async Task<IEnumerable<RandomMaterialVm>> GetRandomMaterialsAsync(int count = 3)
        {
            const int previewPoolSize = 12;

            var materials = await materialRepository
                .AllReadonly()
                .OrderByDescending(m => m.CreatedOn)
                .Take(Math.Max(count * 4, previewPoolSize))
                .Select(m => new RandomMaterialVm
                {
                    Id = m.Id,
                    Title = m.Title,
                    ProfessionName = m.Profession.Name
                })
                .ToListAsync();

            return materials
                .OrderBy(_ => Guid.NewGuid())
                .Take(count)
                .ToList();
        }

        public async Task<IEnumerable<CommentThreadVm>> GetRandomCommentThreadsAsync(int count = 3)
        {
            const int previewPoolSize = 24;

            var recentComments = await context.Comments
                .AsNoTracking()
                .Include(c => c.Material)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedOn)
                .Take(Math.Max(count * 8, previewPoolSize))
                .ToListAsync();

            return recentComments
                .GroupBy(c => c.MaterialId)
                .Select(group =>
                {
                    var lastComment = group
                        .OrderByDescending(c => c.CreatedOn)
                        .First();

                    return new CommentThreadVm
                    {
                        MaterialId = group.Key,
                        MaterialTitle = lastComment.Material.Title,
                        CommentsCount = group.Count(),
                        LastComment = lastComment.Content.Length > 48
                            ? lastComment.Content[..45] + "..."
                            : lastComment.Content,
                        LastCommentDate = lastComment.CreatedOn,
                        Participants = group
                            .Select(c => c.User.FirstName + " " + c.User.LastName)
                            .Distinct()
                            .Take(3)
                            .ToList()
                    };
                })
                .OrderBy(_ => Guid.NewGuid())
                .Take(count)
                .ToList();
        }


        public async Task<int> GetUsersCountAsync()
            {
                return await userManager.Users.CountAsync();
            }

            public async Task<int> GetTeachersCountAsync()
            {
                var teacherRoleId = await context.Roles
                    .Where(r => r.Name == "Teacher")
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync();

                return string.IsNullOrWhiteSpace(teacherRoleId)
                    ? 0
                    : await context.UserRoles.CountAsync(ur => ur.RoleId == teacherRoleId);
            }

            public async Task<int> GetStudentsCountAsync()
            {
                var studentRoleId = await context.Roles
                    .Where(r => r.Name == "Student")
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync();

                return string.IsNullOrWhiteSpace(studentRoleId)
                    ? 0
                    : await context.UserRoles.CountAsync(ur => ur.RoleId == studentRoleId);
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
