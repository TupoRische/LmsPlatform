using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Teacher.Materials
{
    public class TeacherMaterialsPageVm
    {
        public int TotalMaterials { get; set; }
        public int WithFilesCount { get; set; }
        public int WithExternalLinksCount { get; set; }
        public int TotalComments { get; set; }
        public List<TeacherMaterialDetailsVm> Materials { get; set; } = new();
    }
}
