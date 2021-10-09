using System;
using System.Threading.Tasks;
using Api.Extensions;
using Api.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Api.SignalR
{
    [Authorize]
    public class CallHub : Hub
    {
        private readonly PresenceTracker _tracker;
        private readonly IUserRepo _userRepo;
        public CallHub(PresenceTracker tracker, IUserRepo userRepo)
        {
            _userRepo = userRepo;
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        { }
        public async Task RequestCall(bool Start, int UserId, bool HasVideo,string offer)
        {
            var accountId = (await _userRepo.GetUserByUsername(Context.User.GetUsername())).Id;
            var isOnline = _tracker.CheckUserOnline(UserId);
            if (isOnline)
            {
                await Clients.Others.SendAsync("ReceiveRequstCall/" + UserId, UserId, accountId,HasVideo,offer);
            } 
        }

          public async Task ResponseCall(bool Start, int UserId, bool HasVideo,string answer)
        {
            var accountId = (await _userRepo.GetUserByUsername(Context.User.GetUsername())).Id;
            var isOnline = _tracker.CheckUserOnline(UserId);
            if (isOnline)
            {
                await Clients.Others.SendAsync("ReceiveResponseCall/" + UserId, UserId, accountId,HasVideo,answer);
            } 
        }



        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}