using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;
using Api.Extensions;
using Api.Helpers;
using Api.Repos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class CommentController : BaseApiController
    {

        private readonly IPostCommentRepo PostCommentRepo;
        private readonly IMapper Mapper;
        private readonly IUserRepo UserRepo;
        private readonly IPostRepo PostRepo;

        public CommentController(
            IPostCommentRepo postCommentRepo,
            IMapper mapper,
            IUserRepo userRepo,
            IPostRepo postRepo
          )
        {
            this.PostCommentRepo = postCommentRepo;
            this.Mapper = mapper;
            this.UserRepo = userRepo;
            this.PostRepo = postRepo;
        }
        [HttpPost]
        public async Task<ActionResult> AddComment(CommentAddDto comment)
        {
            var user = await UserRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return BadRequest();
            var post = await PostRepo.GetPost(comment.PostId, user.Id);
            if (post == null) return BadRequest();
            comment.UserId = user.Id;
            comment.PostId = post.Id;
            var mappedComment = Mapper.Map<PostComment>(comment);
            PostCommentRepo.Comment(mappedComment);

            if (await PostCommentRepo.SaveChanges())
            {
                return Ok(await PostCommentRepo.GetComment(mappedComment.Id, user.Id));
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var user = await UserRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return BadRequest();
            PostCommentRepo.DeleteComment(id, user.Id);
            if (await PostCommentRepo.SaveChanges())
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostLikeReadDto>>> GetComments([FromQuery] int postId, [FromQuery] UserParams userParams)
        {
            var user = await UserRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return BadRequest();
            var comments= await PostCommentRepo
            .GetComments(postId, userParams, user.Id);
             Response.AddPaginationHeader(comments.CurrentPage, comments.PageSize,
              comments.TotalCount, comments.TotalPages);
            return Ok(comments);
        }


    }
}