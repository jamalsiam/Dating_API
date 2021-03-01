using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Context;
using Api.Dtos;
using Api.Entities;
using Api.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Repos
{
    public class PostCommentRepo : IPostCommentRepo
    {
        public readonly DBContext Context;
        private readonly IMapper Mapper;
        public PostCommentRepo(DBContext context, IMapper mapper)
        {
            this.Mapper = mapper;
            this.Context = context;

        }

        public void Comment(PostComment comment)
        {
            Context.PostComments.Add(comment);
        }

        public void DeleteComment(int commentId, int accountId)
        {
            var comment = Context.PostComments
                .FirstOrDefault(c => c.AppUserId == accountId && c.Id == commentId);
            Context.PostComments.Remove(comment);
        }

        public async Task<CommentReadDto> GetComment(int commentId, int accountId)
        {
            return await Context.PostComments.Select(c => new CommentReadDto
            {
                Id = c.Id,
                PostId = c.PostId,
                UserId = c.AppUserId,
                Fullname = $"{c.AppUser.FirstName } {c.AppUser.LastName}",
                UserPhotoUrl = c.AppUser.Photos.FirstOrDefault(p => p.IsMain).Url,
                Text = c.Text,
                CommentPhotoUrl = null,
                CreatedAt = c.CreatedAt,
                IsAuthor = c.AppUserId == c.Post.AppUserId,
                IsAccountComment = c.AppUserId == accountId,
            })
            .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<PagedList<CommentReadDto>> GetComments(int postId, UserParams userParams, int accountId)
        {
            var comments = Context.PostComments.AsQueryable();
            var commentsList = comments.Select(c => new CommentReadDto
            {
                Id = c.Id,
                PostId = c.PostId,
                UserId = c.AppUserId,
                Fullname = $"{c.AppUser.FirstName } {c.AppUser.LastName}",
                UserPhotoUrl = c.AppUser.Photos.FirstOrDefault(p => p.IsMain).Url,
                Text = c.Text,
                CommentPhotoUrl = null,
                CreatedAt = c.CreatedAt,
                IsAuthor = c.AppUserId == c.Post.AppUserId,
                IsAccountComment = c.AppUserId == accountId,

            })
             .Where(c => c.PostId == postId).OrderByDescending(c => c.Id);
            return await PagedList<CommentReadDto>
                       .CreateAsync(
                       commentsList,
                       userParams.PageNumber,
                       userParams.PageSize
                       );
        }

        public IEnumerable<CommentReadDto> GetLastComments(int postId, int accountId)
        {
            return Context.PostComments.Select(c => new CommentReadDto
            {
                Id = c.Id,
                PostId = c.PostId,
                UserId = c.AppUserId,
                Fullname = $"{c.AppUser.FirstName } {c.AppUser.LastName}",
                UserPhotoUrl = c.AppUser.Photos.FirstOrDefault(p => p.IsMain).Url,
                Text = c.Text,
                CommentPhotoUrl = null,
                CreatedAt = c.CreatedAt,
                IsAuthor = c.AppUserId == c.Post.AppUserId,
                IsAccountComment = c.AppUserId == accountId,

            })
             .Where(c => c.PostId == postId).OrderByDescending(c => c.Id).Take(5);
        }

        public async Task<bool> SaveChanges()
        {
            return await Context.SaveChangesAsync() > 0;
        }
    }
}