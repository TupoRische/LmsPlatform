using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ViewModels.School;

namespace Core.Contracts
{
    public interface ISchoolService
    : IService<SchoolListVm, SchoolDetailsVm, SchoolFormVm>
    {
        Task<IEnumerable<SchoolListVm>> GetAllAsync(string? city = null, string? sortOrder = null);
        Task<IEnumerable<SchoolDropdownVm>> GetDropdownAsync();
        Task<IEnumerable<SchoolListVm>> GetRandomThreeAsync();

    }

}
