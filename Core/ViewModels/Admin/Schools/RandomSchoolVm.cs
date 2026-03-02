using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Admin.Schools
{
    public class RandomSchoolVm
    {
        public int Id { get; set; }
        public string Abbreviation { get; set; } = null!;
        public string City { get; set; } = null!;
    }

}
