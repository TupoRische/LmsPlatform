using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Contracts
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();
        IQueryable<T> AllReadonly();

        Task<T?> GetByIdAsync(int id);

        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(int id);

        Task<int> SaveChangesAsync();
    }
}
