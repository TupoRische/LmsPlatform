using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Admin.Materials
{
    public class RandomMaterialVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ProfessionName { get; set; } = null!;
    }
}
