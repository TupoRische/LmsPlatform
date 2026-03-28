using Core.ViewModels.Home;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class HomeService : IHomeService
    {
        private readonly IRepository<Profession> professionRepository;

        public HomeService(IRepository<Profession> professionRepository)
        {
            this.professionRepository = professionRepository;
        }

        public async Task<IEnumerable<SearchResultVm>> SearchAsync(string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<SearchResultVm>();

            query = query.ToLower();

            return await professionRepository
                .All()
                .Where(p =>
                    p.Name.ToLower().Contains(query) ||
                    (p.School != null && p.School.Name.ToLower().Contains(query)) || 
                    (p.School != null && p.School.City.ToLower().Contains(query)))   
                .Select(p => new SearchResultVm
                {
                    ProfessionId = p.Id,
                    ProfessionName = p.Name,
                    SchoolName = p.School != null ? p.School.Name : "Няма училище",
                    City = p.School != null ? p.School.City : ""
                })
                .ToListAsync();
        }
    }
}