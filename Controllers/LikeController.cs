using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Extensions;
using Api.Helpers;
using Api.Repos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class LikeController : BaseApiController
    {

        private readonly IPostLikeRepo PostLikeRepo;
        private readonly IMapper Mapper;
        private readonly IUserRepo UserRepo;
        private readonly IPostRepo PostRepo;

        public LikeController(
            IPostLikeRepo postLikeRepo,
            IMapper mapper,
            IUserRepo userRepo,
            IPostRepo postRepo
          )
        {
            this.PostLikeRepo = postLikeRepo;
            this.Mapper = mapper;
            this.UserRepo = userRepo; 
            this.PostRepo = postRepo;
        }
        [HttpPost]
        public async Task<ActionResult> Like(LikeAddDeleteDto likeAddDelete )
        {
            var user = await UserRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return BadRequest();
            var like = await PostLikeRepo.GetLike(user.Id, likeAddDelete.PostId);
            if (like == null)
            {
                PostLikeRepo.Like(user.Id, likeAddDelete.PostId);
            }
            else
            {
                PostLikeRepo.UnLike(like);
            }
            if (await PostLikeRepo.SaveChanges())
            {
                return Ok();
            }

            return BadRequest("Something Went Wrong");
        }

        public async Task<ActionResult<IEnumerable<PostLikeReadDto>>> GetLikes([FromQuery] int postId, [FromQuery] UserParams userParams)
        {
            var user = await UserRepo.GetUserByUsername(User.GetUsername());
            var post = await PostRepo.GetPost(postId, user.Id);
            var likes = await PostLikeRepo.GetLikes(post.Id, userParams, user.Id);
            Response.AddPaginationHeader(likes.CurrentPage, likes.PageSize,
              likes.TotalCount, likes.TotalPages);
            return Ok(likes);
        }


    }
}