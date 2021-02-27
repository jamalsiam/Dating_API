using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;

namespace Api.Repos
{
    public interface IPostRepo
    {
        void AddPost(Post post);
        void DeletePost(int userId, int postId);
        void UpdatePost(PostUpdateDto post);
        Task<PostReadDto> GetPost(int postId, int accountId);
        Task<IEnumerable<PostReadDto>> GetPosts(int userId, int accountId ,bool main);
        Task<IEnumerable<PostReadDto>> GetPosts(int userId, int accountId);
        Task<bool> SaveChanges();

    }
}