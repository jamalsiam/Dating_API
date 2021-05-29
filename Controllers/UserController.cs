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
using Microsoft.AspNetCore.Http;
using Api.Interface;
using System.Linq;
using System;
using Api.Helpers;

namespace Api.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IPhotoService _photoService;
        private readonly IUserRepo _userepo;
        private readonly IAccountRepo _accountRepo;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserController(IUserRepo userepo,
                              IMapper mapper,
                              IAccountRepo accountRepo,
                              ITokenService tokenService,
                              IPhotoService PhotoService)
        {
            _userepo = userepo;
            _mapper = mapper;
            _photoService = PhotoService;
            _accountRepo = accountRepo;
            _tokenService = tokenService;
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

            var user = await _userepo.GetUserByUsername(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);

            _userepo.Update(user);

            if (await _userepo.SaveChanges()) return Ok(await _userepo.GetMember(user.Id));

            return BadRequest("Failed to update user");
        }


        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file, [FromForm] bool skipMian, [FromForm] int postId)
        {
            try
            {
                var user = await _userepo.GetUserByUsername(User.GetUsername());
                var member = await _userepo.GetMember(user.Id);

                var result = await _photoService.AddPhoto(file);

                if (result.Error != null) return BadRequest(result.Error.Message);

                var photo = new Photo
                {
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId,

                };
                if (postId != 0)
                {
                    photo.PostId = postId;
                }


                if (skipMian)
                {
                    photo.IsMain = false;

                }
                else
                {
                    var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
                    if (currentMain != null) currentMain.IsMain = false;
                    photo.IsMain = true;
                }

                user.Photos.Add(photo);

                if (await _userepo.SaveChanges())
                {
                    var MappedPhoto = _mapper.Map<PhotoDto>(photo);
                    return Ok(MappedPhoto);
                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return BadRequest("Problem addding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userepo.GetUserByUsername(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This photo already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _userepo.SaveChanges()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userepo.GetUserByUsername(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhoto(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _userepo.SaveChanges()) return Ok();

            return BadRequest("Failed to delete the photo");
        }




        [HttpPut("updatePassword")]
        public async Task<ActionResult> UpdatePassword(UpdatePasswordDto password)
        {
            var account = await _userepo.GetUserByUsername(User.GetUsername());

            if (account ==null) return BadRequest("invalid user");

                var result = await _accountRepo.ChangePassword(account, password.OldPassword, password.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new UserDto()
                {
                    Id = account.Id,
                    Username = account.UserName,
                    Token = await _tokenService.CreateToken(account)
                });
            }
            return BadRequest("Something Went Error");




        }

    }
}
