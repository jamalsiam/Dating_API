using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;
using Api.Extensions;
using Api.Repos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    public class PostController : BaseApiController
    {

        private readonly IPostRepo _postRepo;
        private readonly IMapper mapper;
        private readonly IUserRepo _userRepo;

        public PostController(
            IPostRepo postRepo,
            IMapper mapper,
            IUserRepo userRepo
          )
        {
            this._postRepo = postRepo;
            this.mapper = mapper;
            this._userRepo = userRepo;
        }

        [HttpPost]
        public async Task<ActionResult<PostReadDto>> AddPost(PostAddDto post)
        {
            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            post.UserId = user.Id;
            var mappedPost = mapper.Map<Post>(post);
            _postRepo.AddPost(mappedPost);
            if (await _postRepo.SaveChanges())
            {
                return Ok(await _postRepo.GetPost(mappedPost.Id));

            }
            return BadRequest("Something went wrong");

        }

        [HttpGet("{id}", Name = "GetPost")]
        public async Task<ActionResult<PostReadDto>> GetPost(int id)
        {
            return Ok(await _postRepo.GetPost(id));
        }

        [HttpPut]
        public async Task<ActionResult<PostReadDto>> UpdatePost(PostUpdateDto post)
        {
            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            var initPost = await _postRepo.GetPost(post.Id);
            if (user.Id != post.UserId || initPost.UserId != user.Id) return Unauthorized();

            _postRepo.UpdatePost(post);

            if (await _postRepo.SaveChanges())
            {
                return Ok();
            }
            return BadRequest("Something Went wrong");

        }


        [HttpGet]
        public async Task<ActionResult> GetHomePosts([FromQuery] int id, [FromQuery] bool userPost)
        {
            if (userPost)
            {
                return Ok(await _postRepo.GetPosts(id, false));
            }
            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            return Ok(await _postRepo.GetPosts(user.Id, true));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePosts(int id)
        {
            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            _postRepo.DeletePost(user.Id, id);

            if (await _postRepo.SaveChanges())
            {
                return Ok();
            }
            return BadRequest("Can not delete this post");

        }




    }
}


// IFormFile file