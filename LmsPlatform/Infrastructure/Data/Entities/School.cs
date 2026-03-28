using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities
{
    public class School
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(100)]
        public string City { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }
        public string Abbreviation { get; set; } = null!;

        public ICollection<Profession> Professions { get; set; } = new HashSet<Profession>();
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
