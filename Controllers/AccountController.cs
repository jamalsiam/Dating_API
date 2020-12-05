using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;
using Api.Interface;
using Api.Repos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class AccountController : BaseApiController
    {
        public IAccountRepo AccountRepo { get; }
        public ITokenService TokenService { get; }
        public AccountController(IAccountRepo accountRepo, ITokenService tokenService)
        {
            TokenService = tokenService;
            AccountRepo = accountRepo;

        }

        [HttpPost("Signup")]
        public async Task<ActionResult<UserDto>> Signup(SignupDto signupObj)
        {
            if (await AccountRepo.UserExists(signupObj.Username)) return BadRequest("User Exists");
            AppUser user = AccountRepo.Signup(signupObj);
            if (!await AccountRepo.SaveChanges())
            {
                return Unauthorized();
            }
            return Ok(new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Token = TokenService.CreateToken(user)
            });
        }

        [HttpPost("Signin")]
        public async Task<ActionResult<UserDto>> Signin(SigninDto signupObj)
        {
            if (!await AccountRepo.UserExists(signupObj.Username)) return BadRequest("Invalid Username");
            AppUser user = await AccountRepo.Signin(signupObj);
            if (user.Id != 0)
            {
                return Ok(new UserDto()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Token = TokenService.CreateToken(user)
                });
            }
            else
            {
                return BadRequest("Invalid Password");

            }

        }

    }
}