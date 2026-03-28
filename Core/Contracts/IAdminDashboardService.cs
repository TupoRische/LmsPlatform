using Core.ViewModels.Admin;
using Core.ViewModels.Admin.Materials;
using Core.ViewModels.Admin.Professions;
using Core.ViewModels.Admin.Schools;
using Core.ViewModels.Admin.Students;
using Core.ViewModels.Admin.Teachers;
using Core.ViewModels.Admin.Users;
using Core.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardVm> GetStatsAsync();
        Task<int> GetUsersCountAsync();
        Task<int> GetTeachersCountAsync();
        Task<int> GetStudentsCountAsync();
        Task<int> GetPendingTeachersCountAsync();
        Task<int> GetSchoolsCountAsync();
        Task<int> GetProfessionsCountAsync();
        Task<int> GetMaterialsCountAsync();
        Task<int> GetCommentsCountAsync();

        Task<IEnumerable<RecentUserVm>> GetLastUsersAsync(int count = 3);
        Task<IEnumerable<RandomTeacherVm>> GetRandomTeachersAsync(int count = 3);
        Task<IEnumerable<RandomStudentVm>>GetRandomStudentsAsync(int count = 3);
        Task<IEnumerable<PendingTeacherVm>>GetPendingTeachersPreviewAsync(int count = 3);
        Task<IEnumerable<RandomSchoolVm>>GetRandomSchoolsAsync(int count = 3);
        Task<IEnumerable<RandomProfessionVm>>GetRandomProfessionsAsync(int count = 3);
        Task<AdminUsersPageVm> GetUsersPageAsync();
        Task<IEnumerable<TeacherListVm>> GetTeachersAsync();
        Task<StudentsPageVm> GetStudentsPageAsync();
        Task<ProfessionsPageVm> GetProfessionsPageAsync();
        Task<AdminMaterialsPageVm> GetMaterialsPageAsync();
        Task<TeacherDetailsVm> GetTeacherDetailsAsync(string id);
        Task<IEnumerable<TeacherMaterialVm>> GetTeacherMaterialsAsync(string id);
        Task<TeacherEditVm> GetTeacherForEditAsync(string id);
        Task UpdateTeacherAsync(TeacherEditVm model);
        Task<IEnumerable<SchoolOptionVm>> GetSchoolsAsync();
        Task<IEnumerable<RandomMaterialVm>> GetRandomMaterialsAsync(int count = 3);
        Task<IEnumerable<CommentThreadVm>> GetRandomCommentThreadsAsync(int count = 3);
    }
}
