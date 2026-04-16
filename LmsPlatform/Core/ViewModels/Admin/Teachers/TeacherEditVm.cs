using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Admin.Teachers
{
    public class TeacherEditVm
    {
        public string Id { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public int? SchoolId { get; set; }

        public IEnumerable<SchoolOptionVm> Schools { get; set; }
            = new List<SchoolOptionVm>();
    }
}
