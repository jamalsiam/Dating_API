using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;
using Api.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.Extensions;
using AutoMapper;

namespace Api.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
    {
        public IUserRepo _userepo { get; }
        public IMapper _mapper { get; }

        public UserController(IUserRepo userepo,IMapper mapper)
        {
            _userepo = userepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> AllMembers()
        {
            return Ok(await _userepo.AllMembers());
        }


        [HttpGet("filter/{text}")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembersByText(string text)
        {
            return Ok(await _userepo.GetMembersByText(text.AddSpace()));
        }


        
        [HttpGet("{Id}")]
       
        public async Task<ActionResult<MemberDto>> GetMember(int Id)
        {

            return Ok(await _userepo.GetMember(Id));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userepo.GetUserByUsername(username);

            _mapper.Map(memberUpdateDto, user);

            _userepo.Update(user);

            if (await _userepo.SaveChanges()) return NoContent();

            return BadRequest("Failed to update user");
        }
    }
}