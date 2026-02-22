using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Teacher
{
    public class LatestMaterialVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }
}
