using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;
using Api.Extensions;
using Api.Helpers;
using Api.Repos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MessageController : BaseApiController
    {
        private readonly IMessageRepo MessageRepo;
        private readonly IMapper Mapper;
        private readonly IUserRepo UserRepo;

        public MessageController(
            IMessageRepo messageRepo,
            IMapper mapper,
            IUserRepo userRepo
          )
        {
            this.MessageRepo = messageRepo;
            this.Mapper = mapper;
            this.UserRepo = userRepo;
        }

        [HttpPost]
        public async Task<ActionResult> addMessage(MessageAddDto message)
        {
            var user = await UserRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return BadRequest("User error");

            message.SenderId = user.Id;
            var mappedMessage = Mapper.Map<Message>(message);
            MessageRepo.AddMessage(mappedMessage);
            if (await MessageRepo.SaveChanges())
            {
                return Ok(await MessageRepo.GetMessage(mappedMessage.Id, user.Id));
            }
            return BadRequest("User error");
        }

        [HttpGet("UsersList")]
        public async Task<ActionResult> GetUsersList()
        {
            var user = await UserRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return BadRequest("User error");


            return Ok(await MessageRepo.GetUsersList(user.Id));

        }

        [HttpGet("usersList/{Id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await UserRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return BadRequest("User error");
            return Ok(await MessageRepo.GetUserInfo(id));

        }
        [HttpGet("pagination")]
        public async Task<PagedListX<MessageReadDto>> getMessages([FromQuery] UserParams userParams, [FromQuery] int id)
        {
            var user = await UserRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return null;
            var messageList = await MessageRepo.GetMessages(userParams, user.Id, id);

            return messageList;
        }

        [HttpDelete("deleteFromMe/{Id}")]
        public async Task<ActionResult> DeleteFromMe(int id)
        {
            var user = await UserRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return BadRequest("User error");
            return Ok(await MessageRepo.GetUserInfo(id));

        }

        [HttpDelete("unsend/{Id}")]
        public async Task<ActionResult> Unsend(int id)
        {
            var user = await UserRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return BadRequest("User error");

            var message = await MessageRepo.GetMessage(id, user.Id);
            var mappedMessage = Mapper.Map<Message>(message);
            MessageRepo.DeleteMessage(mappedMessage);

            if (await MessageRepo.SaveChanges())
            {
                return Ok();
            }
            return BadRequest("User error");

        }





    }
}