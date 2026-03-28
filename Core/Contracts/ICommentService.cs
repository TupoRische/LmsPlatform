<<<<<<< HEAD
using Core.ViewModels.Comments;
=======
﻿using Core.ViewModels.Comments;
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
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
<<<<<<< HEAD
        Task CreateAsync(int materialId, string userId, string content, int? parentCommentId = null);
        Task<bool> CanEditAsync(int commentId, string userId);
        Task UpdateAsync(int commentId, string userId, string content);
=======
        Task CreateAsync(int materialId, string userId, string content);
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
        Task<bool> CanDeleteAsync(int commentId, string userId);
        Task DeleteAsync(int commentId);
        Task<List<CommentListVm>> GetCommentsByMaterialAsync(int materialId);
        Task<List<CommentThreadVm>> GetAllThreadsAsync();
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
