using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Home
{
    public class SearchResultVm
    {
        public int ProfessionId { get; set; }
        public string ProfessionName { get; set; } = null!;
        public string SchoolName { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
