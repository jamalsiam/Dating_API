using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Enums;
using Api.Extensions;
using Api.Helpers;
using Api.Repos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    public class FollowController : BaseApiController
    {
        private readonly IFollowRepo _followRepo;
        private readonly IMapper mapper;
        private readonly IUserRepo _useRepo;

        public FollowController(
            IFollowRepo followRepo,
            IMapper mapper,
            IUserRepo userRepo
          )
        {
            this._followRepo = followRepo;
            this.mapper = mapper;
            this._useRepo = userRepo;
        }

        [HttpPost]
        public async Task<ActionResult> Follow(FollowAddDto followAddDto)
        {
            var followingUser = await _useRepo.GetUserByUsername(User.GetUsername());
            var followerUser = await _useRepo.GetUser(followAddDto.FollowingId);
            if (followerUser == null) return BadRequest("User Not Found");

            FollowAddDto follow = new FollowAddDto
            {
                FollowerId = followerUser.Id,
                FollowingId = followingUser.Id,
            };
            var isFollow = await _followRepo.GetFollow(followingUser.Id, followerUser.Id);
            if (isFollow != null)
            {
                return BadRequest("Already Followed");
            }

            _followRepo.Follow(follow);
            if (await _followRepo.SaveChanges())
            {
                return Ok();
            }
            return BadRequest("Faild to Follow");
        }


        [HttpDelete("{UserId}")]
        public async Task<ActionResult> Unfollow(int UserId)
        {
            var followingUser = await _useRepo.GetUserByUsername(User.GetUsername());
            var followerUser = await _useRepo.GetUser(UserId);
            if (followerUser == null) return BadRequest("User Not Found");

            var isFollow = await _followRepo.GetFollow(followingUser.Id, followerUser.Id);
            if (isFollow != null)
            {
                _followRepo.Unfollowing(isFollow);
                if (await _followRepo.SaveChanges())
                {
                    return Ok();
                }
            }


            return BadRequest("Faild To Unfollow");
        }


      [HttpGet("pagination")]
        public async Task<ActionResult<PagedList<FollowReadDto>>> GetFollows([FromQuery] UserParams userParams, [FromQuery] int id, [FromQuery] int followBy)
        {
            var accountId = (await _useRepo.GetUserByUsername(User.GetUsername())).Id;

            if ((int)FollowEnum.ByUser == followBy)
            {
                var followers = await _followRepo.GetFollowers(userParams, id, accountId);
               
                return Ok(followers);
            }
            
            var followings = await _followRepo.GetFollowings(userParams, id, accountId);
   
            return Ok(followings);



        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<int>> GetFollowsCount(int Id, [FromQuery] int followBy)

        {
            if ((int)FollowEnum.ByUser == followBy)
            {
                return Ok(await _followRepo.GetFollowersCounter(Id));
            }
            return Ok(await _followRepo.GetFollowingsCounter(Id));
        }


        [HttpGet("checkFollow")]
        public async Task<ActionResult> CheckFollow([FromQuery] int id)
        {
            var followingUser = await _useRepo.GetUserByUsername(User.GetUsername());
            return Ok(await _followRepo.GetFollow(followingUser.Id, id) != null);
        }


    }
}
