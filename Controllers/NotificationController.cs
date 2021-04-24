using System.Threading.Tasks;
using Api.Extensions;
using Api.Helpers;
using Api.Repos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    public class NotificationController : BaseApiController
    {
        private readonly INotificationRepo NotificationRepo;
        private readonly IMapper Mapper;
        private readonly IUserRepo UserRepo;

        public NotificationController(
       INotificationRepo notificationRepo,
       IMapper Mapper,
       IUserRepo userRepo)
        {
            this.NotificationRepo = notificationRepo;
            this.Mapper = Mapper;
            this.UserRepo = userRepo;
        }

        [HttpGet("pagination")]
        public async Task<ActionResult> GetNotification([FromQuery] UserParams UserParams, [FromQuery] int id = 0)
        {
            var account = await UserRepo.GetUserByUsername(User.GetUsername());
            if (account == null) return BadRequest("User error");
            if (id == account.Id || id == 0)
            {
                var notification = await NotificationRepo.GetAccountNotification(UserParams, account.Id);
                return Ok(notification);
            }
            else
            {
                var notification = await NotificationRepo.GetUserNotification(UserParams, account.Id, id);
                return Ok(notification);
            }

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNotification(int id)
        {
            var user = await UserRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return BadRequest("User error");
            NotificationRepo.Delete(id, user.Id);
            if (await NotificationRepo.SaveChanges())
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}


