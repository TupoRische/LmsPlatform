using Core.ViewModels.Teacher;
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
    }

}
