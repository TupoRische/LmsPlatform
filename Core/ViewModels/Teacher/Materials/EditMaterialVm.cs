using Core.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Teacher.Materials
{
    public class EditMaterialVm
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Изберете професия.")]
        public int ProfessionId { get; set; }

        [Required(ErrorMessage = "Изберете категория.")]
        public int MaterialCategoryId { get; set; }

        public string? Url { get; set; }

        public IFormFile? File { get; set; }

        public IEnumerable<OptionVm> Professions { get; set; } = new List<OptionVm>();
        public IEnumerable<OptionVm> Categories { get; set; } = new List<OptionVm>();
    }
}
