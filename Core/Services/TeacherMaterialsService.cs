using Core.Contracts;
using Core.ViewModels.Common;
using Core.ViewModels.Teacher.Materials;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class TeacherMaterialsService : ITeacherMaterialsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Infrastructure.Identity.ApplicationUser> _userManager;

        public TeacherMaterialsService(ApplicationDbContext context,
            UserManager<Infrastructure.Identity.ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string UserId(ClaimsPrincipal user) => _userManager.GetUserId(user)!;

        public async Task<TeacherMaterialsMyVm> GetMineAsync(ClaimsPrincipal user)
        {
            var userId = UserId(user);

            var list = await _context.Materials
                .Where(m => m.TeacherId == userId)               // <-- ако се казва друго: OwnerId / UserId / CreatedById
                .OrderByDescending(m => m.CreatedOn)            // <-- ако е CreatedAt / CreatedOnUtc и т.н.
                .Select(m => new TeacherMaterialListItemVm
                {
                    Id = m.Id,
                    Title = m.Title,
                    CreatedOn = m.CreatedOn
                })
                .ToListAsync();

            return new TeacherMaterialsMyVm { Materials = list };
        }

        public async Task<int> CreateAsync(ClaimsPrincipal user, CreateMaterialVm model, string? filePath)
        {
            var userId = UserId(user);

            var entity = new Infrastructure.Data.Entities.Material
            {
                Title = model.Title,
                Description = model.Description,
                Url = model.Url,
                TeacherId = userId,
                CreatedOn = DateTime.UtcNow,
                FilePath = filePath,
                ProfessionId = model.ProfessionId,
                MaterialCategoryId = model.MaterialCategoryId
            };

            _context.Materials.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<CreateMaterialFormVm> GetCreateFormAsync()
        {
            var professions = await _context.Professions
                .OrderBy(p => p.Name)
                .Select(p => new OptionVm { Id = p.Id, Name = p.Name })
                .ToListAsync();

            var categories = await _context.MaterialCategories
                .OrderBy(c => c.Name)
                .Select(c => new OptionVm { Id = c.Id, Name = c.Name })
                .ToListAsync();

            return new CreateMaterialFormVm
            {
                Professions = professions,
                Categories = categories
            };
        }

        public async Task<TeacherMaterialDetailsVm> GetDetailsAsync(ClaimsPrincipal user, int id)
        {
            var userId = UserId(user);

            var material = await _context.Materials
                .Where(m => m.Id == id && m.TeacherId == userId)
                .Select(m => new TeacherMaterialDetailsVm
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    Url = m.Url,
                    CreatedOn = m.CreatedOn
                })
                .FirstOrDefaultAsync();

            if (material == null) throw new InvalidOperationException("Not found");
            return material;
        }

        public async Task<EditMaterialVm> GetEditAsync(ClaimsPrincipal user, int id)
        {
            var userId = UserId(user);

            var model = await _context.Materials
                .Where(m => m.Id == id && m.TeacherId == userId)
                .Select(m => new EditMaterialVm
                {
                    Title = m.Title,
                    Description = m.Description,
                    Url = m.Url
                })
                .FirstOrDefaultAsync();

            if (model == null) throw new InvalidOperationException("Not found");
            return model;
        }

        public async Task EditAsync(ClaimsPrincipal user, int id, EditMaterialVm model)
        {
            var userId = UserId(user);

            var entity = await _context.Materials
                .FirstOrDefaultAsync(m => m.Id == id && m.TeacherId == userId);

            if (entity == null) throw new InvalidOperationException("Not found");

            entity.Title = model.Title;
            entity.Description = model.Description;
            entity.Url = model.Url;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ClaimsPrincipal user, int id)
        {
            var userId = UserId(user);

            var entity = await _context.Materials
                .FirstOrDefaultAsync(m => m.Id == id && m.TeacherId == userId);

            if (entity == null) throw new InvalidOperationException("Not found");

            _context.Materials.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

}
