using Core.Contracts;
using Core.ViewModels.Materials;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IRepository<Material> repo;

        public MaterialService(IRepository<Material> repo)
            => this.repo = repo;

        public async Task<IEnumerable<MaterialListVm>> GetByProfessionAsync(int professionId)
            => await repo.AllReadonly()
                .Where(m => m.ProfessionId == professionId)
                .Select(m => new MaterialListVm
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description
                })
                .ToListAsync();

            public async Task<MaterialDetailsVm?> GetByIdAsync(int id)
                 => await repo.AllReadonly()
            .Where(m => m.Id == id)
            .Select(m => new MaterialDetailsVm
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                FilePath = m.FilePath,
                Url = m.Url,
                ProfessionName = m.Profession.Name,
                CategoryName = m.MaterialCategory.Name,
                TeacherName = m.Teacher.UserName!,
                CreatedOn = m.CreatedOn
            })
            .FirstOrDefaultAsync();
    }
}
