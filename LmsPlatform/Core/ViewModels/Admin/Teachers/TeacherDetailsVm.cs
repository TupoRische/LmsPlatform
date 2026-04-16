using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Admin.Teachers
{
    public class TeacherDetailsVm
    {
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int? SchoolId { get; set; }
        public string School { get; set; } = null!;
        public string City { get; set; } = null!;

        public IEnumerable<SchoolOptionVm> Schools { get; set; }
            = new List<SchoolOptionVm>();
    }
}
