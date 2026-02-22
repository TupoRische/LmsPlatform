using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Teacher.Materials
{
    public class CreateMaterialVm
    {
        [Required, StringLength(200)]
        public string Title { get; set; } = null!;

        [StringLength(2000)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? Url { get; set; }

        public IFormFile? File { get; set; } 

        [Required]
        public int ProfessionId { get; set; }

        [Required]
        public int MaterialCategoryId { get; set; }
    }


}
