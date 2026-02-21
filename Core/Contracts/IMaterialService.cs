using Core.ViewModels.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IMaterialService
    {
        Task<IEnumerable<MaterialListVm>> GetByProfessionAsync(int professionId);
        Task<MaterialDetailsVm?> GetByIdAsync(int id);
        Task<IEnumerable<MaterialListVm>> GetMineAsync(string teacherId);
        Task CreateAsync(MaterialFormVm model, string teacherId);
        Task<MaterialFormVm?> GetForEditAsync(int id, string teacherId);
        Task<bool> UpdateAsync(int id, MaterialFormVm model, string teacherId);
        Task<bool> DeleteAsync(int id, string teacherId);
    }
}
