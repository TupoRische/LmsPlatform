using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Materials
{
    public class MaterialFormVm
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }

        // Upload (по желание)
        public IFormFile? Upload { get; set; }

        // Link (по желание)
        [MaxLength(500)]
        public string? Url { get; set; }

        [Required]
        public int ProfessionId { get; set; }

        [Required]
        public int MaterialCategoryId { get; set; }
    }

}
