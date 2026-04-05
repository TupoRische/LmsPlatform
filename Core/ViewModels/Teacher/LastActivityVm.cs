using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Teacher
{
    public class LastActivityVm
    {
        public string LastMaterialTitle { get; set; }
        public DateTime? LastMaterialDate { get; set; }

        public string LastCommentText { get; set; }
        public DateTime? LastCommentDate { get; set; }
    }
}
