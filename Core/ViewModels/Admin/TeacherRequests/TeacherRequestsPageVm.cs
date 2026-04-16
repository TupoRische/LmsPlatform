using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Admin.TeacherRequests
{
    public class TeacherRequestsPageVm
    {
        public List<TeacherRequestListItemVm> Requests { get; set; } = new();
    }

}
