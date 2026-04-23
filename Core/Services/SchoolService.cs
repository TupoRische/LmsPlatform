using Core.Contracts;
using Core.ViewModels.School;
using Infrastructure.Data.Entities;        
using Infrastructure.Repositories;       
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;


namespace Core.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly IRepository<School> repo;

        public SchoolService(IRepository<School> repo) => this.repo = repo;

        public async Task<IEnumerable<SchoolListVm>> GetAllAsync(string? city = null, string? sortOrder = null)
        {
            var schoolsQuery = repo.AllReadonly();

            if (!string.IsNullOrEmpty(city))
            {
                schoolsQuery = schoolsQuery.Where(s => s.City == city);
            }

            var resultQuery = schoolsQuery.Select(s => new SchoolListVm
            {
                Id = s.Id,
                Name = s.Name,
                City = s.City,
                WebsiteUrl = s.WebsiteUrl,
                ProfessionsCount = s.Professions.Count,
                UsersCount = s.Users.Count
            });

            resultQuery = sortOrder switch
            {
                "name_desc" => resultQuery.OrderByDescending(s => s.Name),
                "city_asc" => resultQuery.OrderBy(s => s.City).ThenBy(s => s.Name),
                "city_desc" => resultQuery.OrderByDescending(s => s.City),
                _ => resultQuery.OrderBy(s => s.Name) 
            };

            return await resultQuery.ToListAsync();
        }
        public async Task<IEnumerable<SchoolListVm>> GetRandomThreeAsync()
        {
            const int previewPoolSize = 12;

            var recentSchools = await repo
                .AllReadonly()
                .OrderByDescending(s => s.Id)
                .Take(previewPoolSize)
                .Select(s => new SchoolListVm
                {
                    Id = s.Id,
                    Name = s.Name,
                    City = s.City
                })
                .ToListAsync();

            return recentSchools
                .OrderBy(_ => Guid.NewGuid())
                .Take(3)
                .ToList();
        }

        public async Task<SchoolDetailsVm?> GetByIdAsync(int id)
            => await repo.AllReadonly()
                .Where(s => s.Id == id)
                .Select(s => new SchoolDetailsVm
                {
                    Id = s.Id,
                    Name = s.Name,
                    City = s.City,
                    Description = s.Description,
                })
                .FirstOrDefaultAsync();

        public async Task<int> CreateAsync(SchoolFormVm model)
        {
            var school = new School { Name = model.Name, City = model.City, Description = model.Description };
            await repo.AddAsync(school);
            await repo.SaveChangesAsync();
            return school.Id;
        }

        public async Task UpdateAsync(int id, SchoolFormVm model)
        {
            var school = await repo.GetByIdAsync(id);
            if (school == null) return;

            school.Name = model.Name;
            school.City = model.City;
            school.Description = model.Description;

            repo.Update(school);
            await repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await repo.DeleteAsync(id);
                await repo.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<IEnumerable<SchoolDropdownVm>> GetDropdownAsync()
            => await repo.AllReadonly()
                .OrderBy(s => s.Name)
                .Select(s => new SchoolDropdownVm { Id = s.Id, Name = s.Name })
                .ToListAsync();

        
    }
}
