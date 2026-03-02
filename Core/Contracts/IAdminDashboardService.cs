using Core.ViewModels.Admin;
using Core.ViewModels.Admin.Professions;
using Core.ViewModels.Admin.Schools;
using Core.ViewModels.Admin.Students;
using Core.ViewModels.Admin.Teachers;
using Core.ViewModels.Admin.Users;
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
        Task<IEnumerable<TeacherListVm>> GetTeachersAsync();
    }
}