using System;
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
    public class PostLikeRepo : IPostLikeRepo
    {

        public readonly DBContext Context;
        private readonly IMapper Mapper;
        public PostLikeRepo(DBContext context, IMapper mapper)
        {
            this.Mapper = mapper;
            this.Context = context;

        }


        public async Task<PagedList<PostLikeReadDto>> GetLikes(int postId, UserParams userParams,int accountId)
        {
            var likes = Context
            .PostLikes
            .Where(l => l.PostId == postId)
            .AsQueryable();

            var likesList = likes.Select(l => new PostLikeReadDto
            {
                Id = l.Id,
                PostId = l.PostId,
                UserId = l.AppUserId,
                Fullname = $"{l.AppUser.FirstName} {l.AppUser.LastName}",
                UserPhotoUrl = l.AppUser.Photos.FirstOrDefault(p => p.IsMain).Url,
                CreatedAt = l.CreatedAt,
                FollowingByAccount = Context.Follow.Any(f=> f.FollowerId == l.AppUserId && f.FollowingId == accountId)
            });
            return await PagedList<PostLikeReadDto>
                       .CreateAsync(
                       likesList,
                       userParams.PageNumber,
                       userParams.PageSize
                       );
        }

        public async Task<PostLike> GetLike(int userId, int postId)
        {
            return await Context
            .PostLikes
            .FirstOrDefaultAsync(l => l.AppUserId ==userId && l.PostId ==postId  );
        }

        public void Like(int userId, int postId)
        {
            Context.PostLikes.Add(new PostLike
            {
                AppUserId = userId,
                PostId = postId,
                CreatedAt = DateTime.Now
            });
        }

        public void UnLike(PostLike postLike)
        {
            Context.PostLikes.Remove(postLike);
        }
        public async Task<bool> SaveChanges()
        {
            return await Context.SaveChangesAsync() > 0;
        }


    }
}