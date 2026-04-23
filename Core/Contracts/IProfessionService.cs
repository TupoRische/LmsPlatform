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
        Task<IEnumerable<ProfessionListVm>> GetAllAsync(int? schoolId, string sortOrder);
        Task<IEnumerable<ProfessionIndexVm>> GetRandomThreeAsync();
        Task<ProfessionQuizPageVm> GetQuizAsync();
        Task<ProfessionQuizSavedResultVm?> GetLatestQuizResultAsync(string userId);
        Task<ProfessionQuizSavedResultVm?> ProcessQuizSubmissionAsync(ProfessionQuizSubmissionVm submission, string? userId = null);
    }
}
