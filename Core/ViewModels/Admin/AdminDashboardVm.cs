using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Admin
{
    public class AdminDashboardVm
    {
        public int TotalUsers { get; set; }
        public int Teachers { get; set; }
        public int Students { get; set; }
        public int PendingTeachers { get; set; }

        public int Schools { get; set; }
        public int Professions { get; set; }
        public int Materials { get; set; }
        public int Comments { get; set; }
        public List<SimpleUserVm> LatestPendingTeachers { get; set; } = new();
        public List<SimpleUserVm> LatestTeachers { get; set; } = new();
        public List<SimpleUserVm> LatestStudents { get; set; } = new();
    }
}
