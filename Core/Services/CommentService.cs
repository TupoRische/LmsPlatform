using Core.Contracts;
using Core.ViewModels.Comments;
using Infrastructure.Data.Entities;
using Infrastructure.Identity;
using Infrastructure.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> commentsRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentService(IRepository<Comment> commentsRepo, UserManager<ApplicationUser> userManager)
        {
            this.commentsRepo = commentsRepo;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<CommentListVm>> GetByMaterialAsync(int materialId)
        {
            var comments = await commentsRepo.AllReadonly()
                .Where(c => c.MaterialId == materialId)
                .Include(c => c.User)
                .OrderBy(c => c.CreatedOn)
                .ToListAsync();

            var mapped = new List<CommentListVm>();

            foreach (var comment in comments)
            {
                var roles = await userManager.GetRolesAsync(comment.User);

                mapped.Add(new CommentListVm
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    AuthorId = comment.UserId,
                    AuthorName = comment.User.FirstName + " " + comment.User.LastName,
                    Role = roles.FirstOrDefault() ?? "User",
                    CreatedOn = comment.CreatedOn,
                    ParentCommentId = comment.ParentCommentId
                });
            }

            return BuildThreadTree(mapped);
        }

        public async Task CreateAsync(int materialId, string userId, string content, int? parentCommentId = null)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Comment content is required.", nameof(content));
            }

            if (parentCommentId.HasValue)
            {
                var parentExists = await commentsRepo.AllReadonly()
                    .AnyAsync(c => c.Id == parentCommentId.Value && c.MaterialId == materialId);

                if (!parentExists)
                {
                    throw new InvalidOperationException("Parent comment was not found for this material.");
                }
            }

            var comment = new Comment
            {
                MaterialId = materialId,
                UserId = userId,
                Content = content.Trim(),
                CreatedOn = DateTime.UtcNow,
                ParentCommentId = parentCommentId
            };

            await commentsRepo.AddAsync(comment);
            await commentsRepo.SaveChangesAsync();
        }

        public async Task<bool> CanEditAsync(int commentId, string userId)
            => await commentsRepo.AllReadonly()
                .AnyAsync(c => c.Id == commentId && c.UserId == userId);

        public async Task UpdateAsync(int commentId, string userId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Comment content is required.", nameof(content));
            }

            var comment = await commentsRepo.All()
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                throw new InvalidOperationException("Comment was not found.");
            }

            if (comment.UserId != userId)
            {
                throw new UnauthorizedAccessException("Users can edit only their own comments.");
            }

            comment.Content = content.Trim();
            commentsRepo.Update(comment);
            await commentsRepo.SaveChangesAsync();
        }

        public async Task<bool> CanDeleteAsync(int commentId, string userId)
            => await commentsRepo.AllReadonly()
                .AnyAsync(c => c.Id == commentId && c.UserId == userId);

        public async Task DeleteAsync(int commentId)
        {
            var rootComment = await commentsRepo.AllReadonly()
                .Where(c => c.Id == commentId)
                .Select(c => new { c.Id, c.MaterialId })
                .FirstOrDefaultAsync();

            if (rootComment == null)
            {
                return;
            }

            var comments = await commentsRepo.All()
                .Where(c => c.MaterialId == rootComment.MaterialId)
                .ToListAsync();

            var idsToDelete = CollectReplyIds(commentId, comments)
                .OrderByDescending(id => id)
                .ToList();

            foreach (var id in idsToDelete)
            {
                await commentsRepo.DeleteAsync(id);
            }

            await commentsRepo.SaveChangesAsync();
        }

        public async Task<List<CommentThreadVm>> GetAllThreadsAsync()
        {
            var allComments = await commentsRepo.AllReadonly()
                .Include(c => c.Material)
                .Include(c => c.User)
                .ToListAsync();

            var threads = allComments
                .GroupBy(c => c.MaterialId)
                .Select(g =>
                {
                    var lastComment = g.OrderByDescending(c => c.CreatedOn).First();
                    var participants = g
                        .Select(c => c.User.FirstName + " " + c.User.LastName)
                        .Distinct()
                        .ToList();

                    return new CommentThreadVm
                    {
                        MaterialId = g.Key,
                        MaterialTitle = g.First().Material.Title,
                        CommentsCount = g.Count(),
                        LastComment = lastComment.Content.Length > 50
                            ? lastComment.Content[..47] + "..."
                            : lastComment.Content,
                        LastCommentDate = lastComment.CreatedOn,
                        Participants = participants
                    };
                })
                .OrderByDescending(t => t.LastCommentDate)
                .ToList();

            return threads;
        }

        public async Task<List<CommentListVm>> GetCommentsByMaterialAsync(int materialId)
        {
            var comments = await commentsRepo.AllReadonly()
                .Where(c => c.MaterialId == materialId)
                .Include(c => c.User)
                .OrderBy(c => c.CreatedOn)
                .ToListAsync();

            var result = new List<CommentListVm>();

            foreach (var comment in comments)
            {
                var roles = await userManager.GetRolesAsync(comment.User);
                var role = roles.FirstOrDefault() ?? "User";

                result.Add(new CommentListVm
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    AuthorName = comment.User.FirstName + " " + comment.User.LastName,
                    AuthorId = comment.UserId,
                    CreatedOn = comment.CreatedOn,
                    Role = role,
                    ParentCommentId = comment.ParentCommentId
                });
            }


            return BuildThreadTree(result);
        }

        private static List<CommentListVm> BuildThreadTree(List<CommentListVm> comments)
        {
            var byId = comments.ToDictionary(c => c.Id);
            var roots = new List<CommentListVm>();

            foreach (var comment in comments)
            {
                if (comment.ParentCommentId.HasValue &&
                    byId.TryGetValue(comment.ParentCommentId.Value, out var parent))
                {
                    parent.Replies.Add(comment);
                }
                else
                {
                    roots.Add(comment);
                }
            }

            SortRepliesRecursively(roots);
            return roots.OrderByDescending(c => c.CreatedOn).ToList();
        }

        private static void SortRepliesRecursively(List<CommentListVm> comments)
        {
            foreach (var comment in comments)
            {
                comment.Replies = comment.Replies
                    .OrderBy(c => c.CreatedOn)
                    .ToList();

                SortRepliesRecursively(comment.Replies);
            }
        }

        private static HashSet<int> CollectReplyIds(int rootCommentId, List<Comment> comments)
        {
            var result = new HashSet<int>();
            var childrenLookup = comments
                .Where(c => c.ParentCommentId.HasValue)
                .GroupBy(c => c.ParentCommentId!.Value)
                .ToDictionary(g => g.Key, g => g.Select(c => c.Id).ToList());

            void Visit(int id)
            {
                if (!result.Add(id))
                {
                    return;
                }

                if (childrenLookup.TryGetValue(id, out var children))
                {
                    foreach (var childId in children)
                    {
                        Visit(childId);
                    }
                }
            }

            Visit(rootCommentId);
            return result;
        }
    public async Task<IEnumerable<CommentThreadVm>> GetAllForTeacherAsync(string teacherId)
        {
            var allComments = await commentsRepo.AllReadonly()
                .Where(c => c.Material.TeacherId == teacherId) // Филтрираме само материалите на този учител
                .Include(c => c.Material)
                .Include(c => c.User)
                .ToListAsync();

            var threads = allComments
                .GroupBy(c => c.MaterialId)
                .Select(g =>
                {
                    var lastComment = g.OrderByDescending(c => c.CreatedOn).First();
                    var participants = g
                        .Select(c => c.User.FirstName + " " + c.User.LastName)
                        .Distinct()
                        .ToList();

                    return new CommentThreadVm
                    {
                        MaterialId = g.Key,
                        MaterialTitle = g.First().Material.Title,
                        CommentsCount = g.Count(),
                        LastComment = lastComment.Content.Length > 50
                            ? lastComment.Content[..47] + "..."
                            : lastComment.Content,
                        LastCommentDate = lastComment.CreatedOn,
                        Participants = participants
                    };
                })
                .OrderByDescending(t => t.LastCommentDate)
                .ToList();

            return threads;
        }
    }
}
