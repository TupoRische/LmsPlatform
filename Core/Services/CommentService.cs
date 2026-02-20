using Core.Contracts;
using Core.ViewModels.Comments;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> commentsRepo;

        public CommentService(IRepository<Comment> commentsRepo)
            => this.commentsRepo = commentsRepo;

        public async Task<IEnumerable<CommentListVm>> GetByMaterialAsync(int materialId)
            => await commentsRepo.AllReadonly()
                .Where(c => c.MaterialId == materialId)
                .OrderByDescending(c => c.CreatedOn)
                .Select(c => new CommentListVm
                {
                    Id = c.Id,
                    Content = c.Content,
                    AuthorId = c.UserId,
                    AuthorName = c.User.UserName!,
                    CreatedOn = c.CreatedOn
                })
                .ToListAsync();

        public async Task CreateAsync(int materialId, string userId, string content)
        {
            var comment = new Comment
            {
                MaterialId = materialId,
                UserId = userId,
                Content = content,
                CreatedOn = DateTime.UtcNow
            };

            await commentsRepo.AddAsync(comment);
            await commentsRepo.SaveChangesAsync();
        }

        public async Task<bool> CanDeleteAsync(int commentId, string userId)
            => await commentsRepo.AllReadonly()
                .AnyAsync(c => c.Id == commentId && c.UserId == userId);

        public async Task DeleteAsync(int commentId)
        {
            await commentsRepo.DeleteAsync(commentId);
            await commentsRepo.SaveChangesAsync();
        }
    }

}
