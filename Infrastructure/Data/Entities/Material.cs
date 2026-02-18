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

        // Ако качвате файл: пазите път/име (реално файла е в wwwroot/uploads)
        [MaxLength(500)]
        public string? FilePath { get; set; }

        // или линк към ресурс
        [MaxLength(500)]
        public string? Url { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        // FK към Profession
        public int ProfessionId { get; set; }
        public Profession Profession { get; set; } = null!;

        // FK към Category
        public int MaterialCategoryId { get; set; }
        public MaterialCategory MaterialCategory { get; set; } = null!;

        // FK към Teacher (Identity)
        [Required]
        public string TeacherId { get; set; } = null!;
        public ApplicationUser Teacher { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}
