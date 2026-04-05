using Core.ViewModels.Teacher;
using Core.ViewModels.Teacher.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface ITeacherDashboardService
    {
        Task<TeacherDashboardVm> GetAsync(ClaimsPrincipal user);
        Task<TeacherMaterialsPageVm> GetTeacherMaterialsAsync(ClaimsPrincipal user);
    }

}
