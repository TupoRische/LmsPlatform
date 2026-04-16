using Core.ViewModels.Admin.TeacherRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IAdminTeacherRequestsService
    {
        Task<TeacherRequestsPageVm> GetPendingAsync();
        Task ApproveAsync(string userId);
        Task RejectAsync(string userId);
    }
}
