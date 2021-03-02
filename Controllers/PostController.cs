using System.Collections.Generic;
using System.Linq;
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
        private readonly IMapper Mapper;
        private readonly IUserRepo _userRepo;

        public PostController(
            IPostRepo postRepo,
            IMapper Mapper,
            IUserRepo userRepo
          )
        {
            this._postRepo = postRepo;
            this.Mapper = Mapper;
            this._userRepo = userRepo;
        }

        [HttpPost]
        public async Task<ActionResult<PostReadDto>> AddPost(PostAddDto post)
        {
            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            post.UserId = user.Id;
            var mappedPost = Mapper.Map<Post>(post);
            _postRepo.AddPost(mappedPost);
            if (await _postRepo.SaveChanges())
            {
                return Ok(await _postRepo.GetPost(mappedPost.Id, user.Id));

            }
            return BadRequest("Something went wrong");

        }

        [HttpGet("{id}", Name = "GetPost")]
        public async Task<ActionResult<PostReadDto>> GetPost(int id)
        {
            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            return Ok(await _postRepo.GetPost(id, user.Id));
        }

        [HttpPut]
        public async Task<ActionResult<PostReadDto>> UpdatePost(PostUpdateDto post)
        {
            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            var initPost = await _postRepo.GetPost(post.Id, user.Id);
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
            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            if (userPost)
            {
                return Ok(await _postRepo.GetPosts(id, user.Id));
            }

            return Ok(await _postRepo.GetPosts(user.Id, user.Id));
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

        [HttpPost("share/{postId}")]
        public async Task<ActionResult<PostReadDto>> share(int postId)
        {
            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            var post = await _postRepo.GetPost(postId, user.Id);
            var x = new List<Photo>(Mapper.Map<IEnumerable<Photo>>(post.Photos));
            var y =  x.Select(e => new Photo{
                Url= e.Url,

            }).ToList();
            var mappedPhotos = Mapper.Map<ICollection<Photo>>(post.Photos);

            var newPost = new Post
            {
                Text = post.Text,
                Feeling = post.Feeling,
                AppUserId = user.Id,
                Photos = y
                

            };
            _postRepo.AddPost(newPost);
            if (await _postRepo.SaveChanges())
            {
                return Ok();

            }
            return BadRequest("Something went wrong");
        }
    }
}
