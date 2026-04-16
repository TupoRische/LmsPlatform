using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<T> dbSet;

        public EfRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public IQueryable<T> All() => dbSet;

        public IQueryable<T> AllReadonly() => dbSet.AsNoTracking();

        public async Task<T?> GetByIdAsync(int id)
            => await dbSet.FindAsync(id);

        public async Task AddAsync(T entity)
            => await dbSet.AddAsync(entity);

        public void Update(T entity)
            => dbSet.Update(entity);

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        public Task<int> SaveChangesAsync()
            => context.SaveChangesAsync();
    }
}
