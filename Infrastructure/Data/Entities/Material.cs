using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities
{
    public class Material
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? FilePath { get; set; }

        [MaxLength(500)]
        public string? Url { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public int ProfessionId { get; set; }
        public Profession Profession { get; set; } = null!;
        public int MaterialCategoryId { get; set; }
        public MaterialCategory MaterialCategory { get; set; } = null!;
        [Required]
        public string TeacherId { get; set; } = null!;
        public ApplicationUser Teacher { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}
