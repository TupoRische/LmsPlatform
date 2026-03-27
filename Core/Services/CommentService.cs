using Core.Contracts;
using Core.ViewModels.Comments;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Contracts;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
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

        private readonly UserManager<ApplicationUser> userManager;



        public CommentService(IRepository<Comment> commentsRepo, UserManager<ApplicationUser> userManager)

        {

            this.commentsRepo = commentsRepo;

            this.userManager = userManager;

        }
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



        public async Task<List<CommentListVm>> GetCommentsByMaterialAsync(int materialId)

        {

            var comments = await commentsRepo.All()

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

                    Role = role

                });

            }



            return result;

        }
    public async Task<List<CommentThreadVm>> GetAllThreadsAsync()
        {
            var allComments = await commentsRepo.AllReadonly()
                .Include(c => c.Material)
                .Include(c => c.User)
                .ToListAsync();

            var threads = allComments
                .GroupBy(c => c.MaterialId)
                .Select(async g =>
                {
                    var lastComment = g.OrderByDescending(c => c.CreatedOn).First();
                    var participants = g.Select(c => c.User.FirstName + " " + c.User.LastName).Distinct().ToList();

                    return new CommentThreadVm
                    {
                        MaterialId = g.Key,
                        MaterialTitle = g.First().Material.Title, // Предполагаме, че свойството е Title
                        CommentsCount = g.Count(),
                        LastComment = lastComment.Content.Length > 50
                            ? lastComment.Content.Substring(0, 47) + "..."
                            : lastComment.Content,
                        LastCommentDate = lastComment.CreatedOn,
                        Participants = participants
                    };
                })
                .Select(t => t.Result)
                .OrderByDescending(t => t.LastCommentDate)
                .ToList();

            return threads;
        }

    } 
}