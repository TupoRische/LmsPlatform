using Core.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Materials
{
    public class MaterialIndexVm
    {
        public int? ProfessionId { get; set; }

        public int? MaterialCategoryId { get; set; }

        public IEnumerable<DropdownOptionVm> Professions { get; set; }
            = new List<DropdownOptionVm>();

        public IEnumerable<DropdownOptionVm> Categories { get; set; }
            = new List<DropdownOptionVm>();

        public IEnumerable<MaterialListVm> Materials { get; set; }
            = new List<MaterialListVm>();
    }

}
