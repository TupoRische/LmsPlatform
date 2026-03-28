using Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Core.ViewModels.Professions;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ProfessionService : IProfessionService
    {
        private readonly IRepository<Profession> repo;

        public ProfessionService(IRepository<Profession> repo)
            => this.repo = repo;

        public Task<int> CreateAsync(ProfessionFormVm model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProfessionListVm>> GetAllAsync(int? schoolId)
        {
            var query = repo.AllReadonly();

            if (schoolId.HasValue)
                query = query.Where(p => p.SchoolId == schoolId.Value);

            return await query
                .Select(p => new ProfessionListVm
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    SchoolName = p.School.Name
                })
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public Task<IEnumerable<ProfessionListVm>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ProfessionDetailsVm?> GetByIdAsync(int id)
            => await repo.AllReadonly()
        .Where(p => p.Id == id)
        .Select(p => new ProfessionDetailsVm
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            SchoolName = p.School.Name
        })
        .FirstOrDefaultAsync();

        public async Task<IEnumerable<ProfessionIndexVm>> GetRandomThreeAsync()
        {
            const int previewPoolSize = 12;

            var recentProfessions = await repo.AllReadonly()
                .OrderByDescending(p => p.Id)
                .Take(previewPoolSize)
                .Select(p => new ProfessionIndexVm
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                })
                .ToListAsync();

            return recentProfessions
                .OrderBy(_ => Guid.NewGuid())
                .Take(3)
                .ToList();
        }


        public Task UpdateAsync(int id, ProfessionFormVm model)
        {
            throw new NotImplementedException();
        }
    }
}
