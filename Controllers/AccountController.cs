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
            AppUser user = await AccountRepo.Signup(signupObj);

            if (user != null)
            {
                return Ok(new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Token = await TokenService.CreateToken(user)
                });
            }
            return BadRequest("invalid Sginup");


        }

        [HttpPost("Signin")]
        public async Task<ActionResult<UserDto>> Signin( SigninDto signupObj)
        {
            if (!await AccountRepo.UserExists(signupObj.Username)) return BadRequest("Invalid Username");
            AppUser user = await AccountRepo.Signin(signupObj);
            if (user.Id != 0)
            {
                return Ok(new UserDto()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Token = await TokenService.CreateToken(user)
                });
            }
            else
            {
                return BadRequest("Invalid Password");

            }

        }
        [HttpGet]
        public ActionResult Get()
        {
            
            return  Ok("ssfsfs");
        }

    }
}