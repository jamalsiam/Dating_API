using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
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
        public async Task<ActionResult<IEnumerable<MemberDto>>> AllMembers()
        {
            return Ok(await _userepo.AllMembers());
        }

        [AllowAnonymous]
        [HttpGet("{Id}")]
        public async Task<ActionResult<MemberDto>> GetMember(int Id)
        {
            
            return Ok(await _userepo.GetMember(Id));
        }


    }
}