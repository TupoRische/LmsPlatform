<<<<<<< HEAD
﻿using Core.ViewModels.Common;
using Microsoft.AspNetCore.Http;
=======
﻿using Microsoft.AspNetCore.Http;
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
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
<<<<<<< HEAD
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
=======
        [Required]
        [StringLength(100)]
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
        public string Title { get; set; } = null!;

        [StringLength(2000)]
        public string? Description { get; set; }

<<<<<<< HEAD
        [Required(ErrorMessage = "Изберете професия.")]
        public int ProfessionId { get; set; }

        [Required(ErrorMessage = "Изберете категория.")]
        public int MaterialCategoryId { get; set; }

        public string? Url { get; set; }

        public IFormFile? File { get; set; }

        public IEnumerable<OptionVm> Professions { get; set; } = new List<OptionVm>();
        public IEnumerable<OptionVm> Categories { get; set; } = new List<OptionVm>();
    }
=======
        [StringLength(500)]
        public string? Url { get; set; }
        public IFormFile? File { get; set; }
    }

>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
}
