using Core.Contracts;
using Core.ViewModels.School;
using Infrastructure.Data.Entities;        // School
using Infrastructure.Repositories;         // IRepository<T>
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;


namespace Core.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly IRepository<School> repo;

        public SchoolService(IRepository<School> repo) => this.repo = repo;

        public async Task<IEnumerable<SchoolListVm>> GetAllAsync()
            => await repo.AllReadonly()
                .Select(s => new SchoolListVm
                {
                    Id = s.Id,
                    Name = s.Name,
                    City = s.City,
                    ProfessionsCount = s.Professions.Count,
                    UsersCount = s.Users.Count
                })
                .OrderBy(s => s.City).ThenBy(s => s.Name)
                .ToListAsync();
        public async Task<IEnumerable<SchoolListVm>> GetRandomThreeAsync()
        {
<<<<<<< HEAD
            const int previewPoolSize = 12;

            var recentSchools = await repo
                .AllReadonly()
                .OrderByDescending(s => s.Id)
                .Take(previewPoolSize)
=======
            return await repo
                .All()
                .OrderBy(x => Guid.NewGuid())
                .Take(3)
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
                .Select(s => new SchoolListVm
                {
                    Id = s.Id,
                    Name = s.Name,
                    City = s.City
                })
                .ToListAsync();
<<<<<<< HEAD

            return recentSchools
                .OrderBy(_ => Guid.NewGuid())
                .Take(3)
                .ToList();
=======
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
        }

        public async Task<SchoolDetailsVm?> GetByIdAsync(int id)
            => await repo.AllReadonly()
                .Where(s => s.Id == id)
                .Select(s => new SchoolDetailsVm
                {
                    Id = s.Id,
                    Name = s.Name,
                    City = s.City,
                    Description = s.Description
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
