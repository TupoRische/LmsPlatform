using Core.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Teacher.Materials
{
    public class CreateMaterialFormVm
    {
        public CreateMaterialVm Material { get; set; } = new();

        public List<OptionVm> Professions { get; set; } = new();
        public List<OptionVm> Categories { get; set; } = new();
    }

}
