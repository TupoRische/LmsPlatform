using Core.ViewModels.Materials;
using Core.ViewModels.Teacher.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface ITeacherMaterialsService
    {
        Task<TeacherMaterialsMyVm> GetMineAsync(ClaimsPrincipal user);

        Task<CreateMaterialFormVm> GetCreateFormAsync();
        Task<int> CreateAsync(ClaimsPrincipal user, CreateMaterialVm model, string? filePath);

        Task<TeacherMaterialDetailsVm> GetDetailsAsync(ClaimsPrincipal user, int id);

        Task<EditMaterialVm> GetEditAsync(ClaimsPrincipal user, int id);

<<<<<<< HEAD
        Task EditAsync(ClaimsPrincipal user, int id, EditMaterialVm model, string? filePath);
=======
        Task EditAsync(ClaimsPrincipal user, int id, EditMaterialVm model);
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56

        Task DeleteAsync(ClaimsPrincipal user, int id);
    }

}
