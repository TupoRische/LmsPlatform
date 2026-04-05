using Core.ViewModels.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Admin.Teachers
{
    public class TeacherCommentsPageVm
    {
        public int TotalComments { get; set; }
        public int NewCommentsToday { get; set; }
        public List<TeacherCommentVm> Comments { get; set; } = new();
    }
}
