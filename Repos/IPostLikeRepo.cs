using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;
using Api.Helpers;

namespace Api.Repos
{
    public interface IPostLikeRepo
    {
        Task<PagedList<PostLikeReadDto>> GetLikes(int postId, UserParams userParams,int accountId);
        Task<PostLike> GetLike(int userId, int postId);
        void Like(int userId, int postId);
        void UnLike(PostLike postLike);
        Task<bool> SaveChanges();
    }
}