using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Professions
{
    public class ProfessionListVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string SchoolName { get; set; } = null!;
        public int MaterialsCount { get; set; }
    }
}
