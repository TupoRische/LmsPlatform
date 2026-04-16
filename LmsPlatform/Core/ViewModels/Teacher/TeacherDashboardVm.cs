using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Teacher
{
    public class TeacherDashboardVm
    {
        public int MyMaterials { get; set; }
        public int MyComments { get; set; }
        public List<LatestMaterialVm> LatestMaterials { get; set; } = new();
    }
}
