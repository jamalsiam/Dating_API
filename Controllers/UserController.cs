using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Entities;
using Api.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    public class UserController : BaseApiController
    {
        public IUserRepo _userepo { get; }
        public UserController(IUserRepo userepo)
        {
            _userepo = userepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> AllUsers()
        {
            return Ok(await _userepo.AllUsers());
        }

        [AllowAnonymous]
        [HttpGet("{Id}")]
        public async Task<ActionResult<AppUser>> GetUser(int Id)
        {
            // await Seed.SeedUsers(DBContext);
            return Ok(await _userepo.GetUser(Id));
        }


    }
}