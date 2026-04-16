using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Teacher.Materials
{
    public class TeacherMaterialDetailsVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Url { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CategoryName { get; set; } = null!;
        public string ProfessionName { get; set; } = null!;
        public int CommentsCount { get; set; }
        public string TeacherName { get; set; } = null!;
    }
}
