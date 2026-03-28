using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IService<TListVm, TDetailsVm, TFormVm>
    {
        Task<IEnumerable<TListVm>> GetAllAsync();
        Task<TDetailsVm?> GetByIdAsync(int id);

        Task<int> CreateAsync(TFormVm model);
        Task UpdateAsync(int id, TFormVm model);
        Task<bool> DeleteAsync(int id);
    }

}
