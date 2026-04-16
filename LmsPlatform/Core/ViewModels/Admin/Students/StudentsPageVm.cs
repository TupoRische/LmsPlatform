using System.Collections.Generic;

namespace Core.ViewModels.Admin.Students
{
    public class StudentsPageVm
    {
        public int TotalStudents { get; set; }
        public int WithSchoolCount { get; set; }
        public int WithoutSchoolCount { get; set; }
        public int TotalComments { get; set; }
        public IEnumerable<StudentListItemVm> Students { get; set; } = new List<StudentListItemVm>();
    }
}
