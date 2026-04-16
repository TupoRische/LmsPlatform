using Core.ViewModels.Professions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IProfessionService
    : IService<ProfessionListVm, ProfessionDetailsVm, ProfessionFormVm>
    {
        Task<IEnumerable<ProfessionListVm>> GetAllAsync(int? schoolId, string professionName);
        Task<IEnumerable<ProfessionIndexVm>> GetRandomThreeAsync();
        Task<ProfessionQuizPageVm> GetQuizAsync();
    }
}
