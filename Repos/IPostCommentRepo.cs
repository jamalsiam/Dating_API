using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;
using Api.Helpers;

namespace Api.Repos
{
    public interface IPostCommentRepo
    {
        Task<PagedList<CommentReadDto>> GetComments(int postId, UserParams userParams, int accountId);
        IEnumerable<CommentReadDto> GetLastComments(int postId, int accountId);
        Task<CommentReadDto> GetComment(int commentId, int accountId);
        void Comment(PostComment commentAddDto);
        void DeleteComment(int commentId, int accountId);
        Task<bool> SaveChanges();

    }
}