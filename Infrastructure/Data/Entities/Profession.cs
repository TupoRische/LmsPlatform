using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities
{
    public class Profession
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }
        public int SchoolId { get; set; }
        public School School { get; set; } = null!;
        public ICollection<Material> Materials { get; set; } = new HashSet<Material>();
    }
}
