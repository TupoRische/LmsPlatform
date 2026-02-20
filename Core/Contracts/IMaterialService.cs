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
    }
}
