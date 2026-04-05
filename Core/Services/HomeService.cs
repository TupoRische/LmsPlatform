using Core.Contracts;
using Core.ViewModels.Home;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

public class HomeService : IHomeService
{
    private readonly IRepository<Profession> professionRepository;
    private readonly IRepository<School> schoolRepository;
    private readonly IRepository<Material> materialRepository; // Добави това

    public HomeService(
        IRepository<Profession> professionRepository,
        IRepository<School> schoolRepository,
        IRepository<Material> materialRepository) // И тук
    {
        this.professionRepository = professionRepository;
        this.schoolRepository = schoolRepository;
        this.materialRepository = materialRepository;
    }

    public async Task<IEnumerable<SearchResultVm>> SearchAsync(string? query)
    {
        if (string.IsNullOrWhiteSpace(query)) return new List<SearchResultVm>();
        query = query.ToLower();

        // 1. Професии
        var professions = await professionRepository.All()
            .Where(p => p.Name.ToLower().Contains(query))
            .Select(p => new SearchResultVm
            {
                Id = p.Id,
                Title = p.Name,
                Description = "Професионално направление",
                Type = "Profession",
                Url = $"/Student/Professions/Details/{p.Id}"
            }).ToListAsync();

        // 2. Училища
        var schools = await schoolRepository.All()
            .Where(s => s.Name.ToLower().Contains(query))
            .Select(s => new SearchResultVm
            {
                Id = s.Id,
                Title = s.Name,
                Description = s.City,
                Type = "School",
                Url = $"/Student/Schools/Index#school-{s.Id}"
            }).ToListAsync();

        // 3. Материали (НОВО)
        var materials = await materialRepository.All()
            .Where(m => m.Title.ToLower().Contains(query) || m.Description.ToLower().Contains(query))
            .Select(m => new SearchResultVm
            {
                Id = m.Id,
                Title = m.Title,
                Description = "Учебен материал / Ресурс",
                Type = "Material",
                Url = $"/Student/Materials/Details/{m.Id}" // Директен линк към детайлите
            }).ToListAsync();

        return professions.Concat(schools).Concat(materials);
    }
}