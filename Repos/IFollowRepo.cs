using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;
using Api.Helpers;

namespace Api.Repos
{
    public interface IFollowRepo
    {
        void Follow(FollowAddDto follow);
        void Unfollowing(UserFollow follow);
        Task<UserFollow> GetFollow(int followerId, int followingId);
        Task<PagedList<FollowReadDto>> GetFollowers(UserParams userParams, int FollowerId, int accountId);
        Task<PagedList<FollowReadDto>> GetFollowings(UserParams userParams, int FollowingId, int accountId);
        Task<int> GetFollowersCounter(int FollowerId);
        Task<int> GetFollowingsCounter(int FollowingId);
        Task<bool> SaveChanges();
    }
}