using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Teacher
{
    public class TeacherDashboardVm
    {
        public int MaterialsCount { get; set; }
        public int NewCommentsCount { get; set; }

        public List<TeacherMaterialListItemVm> RecentMaterials { get; set; } = new();
        public List<TeacherCommentVm> RecentComments { get; set; } = new();

        public string LastMaterial { get; set; } = "Няма качени материали";
        public string LastComment { get; set; } = "Няма нови коментари";
    }
}
