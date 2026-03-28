using Core.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentListVm>> GetByMaterialAsync(int materialId);
        Task CreateAsync(int materialId, string userId, string content, int? parentCommentId = null);
        Task<bool> CanEditAsync(int commentId, string userId);
        Task UpdateAsync(int commentId, string userId, string content);
        Task<bool> CanDeleteAsync(int commentId, string userId);
        Task DeleteAsync(int commentId);
        Task<List<CommentListVm>> GetCommentsByMaterialAsync(int materialId);
        Task<List<CommentThreadVm>> GetAllThreadsAsync();
    }
}
