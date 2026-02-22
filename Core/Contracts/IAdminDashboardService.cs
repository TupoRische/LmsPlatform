using Core.ViewModels.Admin;
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
    }
}