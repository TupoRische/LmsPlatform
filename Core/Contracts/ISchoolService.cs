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
        // ако ти трябват допълнителни методи само за училища – добавяш тук
        Task<IEnumerable<SchoolDropdownVm>> GetDropdownAsync();
    }

}
