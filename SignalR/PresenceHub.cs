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
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _tracker;
        private readonly IUserRepo _userRepo;
        public PresenceHub(PresenceTracker tracker, IUserRepo userRepo)
        {
            _userRepo = userRepo;
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {

            var userId = (await _userRepo.GetUserByUsername(Context.User.GetUsername())).Id;
            var isOnline = await _tracker.UserConnected(userId, Context.ConnectionId);
            if (isOnline)
                await Clients.Others.SendAsync("UserIsOnline", userId);

            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = (await _userRepo.GetUserByUsername(Context.User.GetUsername())).Id;
            var isOffline = await _tracker.UserDisconnected(userId, Context.ConnectionId);

            if (isOffline)
                await Clients.Others.SendAsync("UserIsOffline", userId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}