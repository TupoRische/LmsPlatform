using Core.ViewModels.Professions;
using Core.ViewModels.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Home
{
    public class HomeIndexVm
    {
        public IEnumerable<ProfessionIndexVm> Professions { get; set; } = new List<ProfessionIndexVm>();
        public IEnumerable<SchoolListVm> Schools { get; set; } = new List<SchoolListVm>();
    }
}
