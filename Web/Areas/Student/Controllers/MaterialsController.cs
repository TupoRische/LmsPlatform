using Core.ViewModels.Common;
using Core.ViewModels.Materials;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Web.Areas.Student.Controllers
{
    [Area("Student")]
    public class MaterialsController : Controller
    {
        private readonly ApplicationDbContext data;

        public MaterialsController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public async Task<IActionResult> Index(int? professionId, int? materialCategoryId)
        {
            var materialsQuery = data.Materials
                .Include(m => m.Profession)
                .Include(m => m.MaterialCategory)
                .Include(m => m.Teacher)
                .AsQueryable();


            if (professionId.HasValue)
            {
                materialsQuery = materialsQuery
                    .Where(m => m.ProfessionId == professionId.Value);
            }

            if (materialCategoryId.HasValue)
            {
                materialsQuery = materialsQuery
                    .Where(m => m.MaterialCategoryId == materialCategoryId.Value);
            }

            var model = new MaterialIndexVm
            {
                ProfessionId = professionId,
                MaterialCategoryId = materialCategoryId,

                Professions = await data.Professions
                    .Select(p => new DropdownOptionVm
                    {
                        Id = p.Id,
                        Name = p.Name
                    })
                    .ToListAsync(),

                Categories = await data.MaterialCategories
                    .Select(c => new DropdownOptionVm
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                    .ToListAsync(),

                Materials = await materialsQuery
            .OrderByDescending(m => m.CreatedOn)
            .Select(m => new MaterialListVm
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                ProfessionName = m.Profession.Name,
                CategoryName = m.MaterialCategory.Name,
                CreatedOn = m.CreatedOn,
                TeacherName = m.Teacher.FirstName + " " + m.Teacher.LastName
            })
            .ToListAsync()
            };

            return View(model);
        }

    }
}
