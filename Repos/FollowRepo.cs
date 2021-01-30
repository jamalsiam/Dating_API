using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Context;
using Api.Dtos;
using Api.Entities;
using Api.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.Repos
{
    public class FollowRepo : IFollowRepo
    {
        private readonly DBContext Context;
        private readonly IMapper Mapper;

        public FollowRepo(DBContext context, IMapper mapper)
        {
            this.Mapper = mapper;
            this.Context = context;

        }
        public void Follow(FollowAddDto follow)
        {
            var mappedUserFollow = Mapper.Map<UserFollow>(follow);
            Context.Follow.Add(mappedUserFollow);
        }
        public void Unfollowing(UserFollow follow)
        {
            Context.Follow.Remove(follow);
        }

        public async Task<PagedList<FollowReadDto>> GetFollowers(UserParams userParams, int followerId, int accountId)
        {


            var users = Context.Users.OrderBy(u => u.UserName).AsQueryable();
            var Follow = Context.Follow.AsQueryable();

            Follow = Follow.Where(f => f.FollowerId == followerId);
            users = Follow.Select(f => f.Following);

            var userFollowers = users.Select(user => new FollowReadDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                IsFollowByAccount = user.Followings.Any(u => u.FollowingId == accountId),

            });

            return await PagedList<FollowReadDto>
             .CreateAsync(
             userFollowers,
             userParams.PageNumber,
             userParams.PageSize
             );
        }

        public async Task<PagedList<FollowReadDto>> GetFollowings(UserParams userParams, int followingId, int accountId)
        {

            var users = Context.Users.OrderBy(u => u.UserName).AsQueryable();
            var Follow = Context.Follow.AsQueryable();

            Follow = Follow.Where(f => f.FollowingId == followingId);
            users = Follow.Select(f => f.Follower);

            var userFollowings = users.Select(user => new FollowReadDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                IsFollowByAccount = user.Followings.Any(u => u.FollowingId == accountId),

            });


            return await PagedList<FollowReadDto>
               .CreateAsync(
               userFollowings,
               userParams.PageNumber,
               userParams.PageSize
               );

        }
        public async Task<UserFollow> GetFollow(int followingId, int followerId)
        {
            return await Context.Follow.FindAsync(followingId, followerId);
        }

        public async Task<bool> SaveChanges()
        {
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<int> GetFollowersCounter(int FollowerId)
        {
            var FollowerRecordCount = await Context.Follow.Where(u => u.FollowerId == FollowerId).CountAsync();
            return FollowerRecordCount;
        }

        public async Task<int> GetFollowingsCounter(int FollowingId)
        {
            var FollowerRecordCount = await Context.Follow.Where(u => u.FollowingId == FollowingId).CountAsync();
            return FollowerRecordCount;
        }
    }
}