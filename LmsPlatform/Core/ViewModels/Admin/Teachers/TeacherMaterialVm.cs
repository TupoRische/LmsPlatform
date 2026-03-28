using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Admin.Teachers
{
    public class TeacherMaterialVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Profession { get; set; } = null!;
        public string Category { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }
}
