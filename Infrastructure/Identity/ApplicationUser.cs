using Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsApproved { get; set; } = false;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public ICollection<Material> MaterialsCreated { get; set; } = new HashSet<Material>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<QuizResult> QuizResults { get; set; } = new HashSet<QuizResult>();
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public bool RequestedTeacher { get; set; } = false;
        public int? SchoolId { get; set; }
        public School? School { get; set; }
    }
}
