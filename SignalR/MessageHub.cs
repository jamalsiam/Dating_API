using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;
using Api.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Api.Interface;
using Api.Repos;
using Microsoft.AspNetCore.Mvc;

namespace Api.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly PresenceTracker _tracker;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepo UserRepo;
        public MessageHub(IMapper mapper, IUnitOfWork unitOfWork, IHubContext<PresenceHub> presenceHub,

            PresenceTracker tracker, IUserRepo userRepo)
        {

            _unitOfWork = unitOfWork;
            _tracker = tracker;
            _presenceHub = presenceHub;
            _mapper = mapper;
            UserRepo = userRepo;
        }

        public override async Task OnConnectedAsync()
        {

            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["otherUsername"].ToString();
            var user = await UserRepo.GetUserByUsername(Context.User.GetUsername());
            var groupName = GetGroupName(user.Id.ToString(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            if (_unitOfWork.HasChanges()) await _unitOfWork.Complete();

            await Clients.Caller.SendAsync("ReceiveMessageThread");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(MessageAddDto createMessageDto)
        {
            var user = await UserRepo.GetUserByUsername(Context.User.GetUsername());


            if (user.Id == createMessageDto.RecipientId)
                throw new HubException("You cannot send messages to yourself");


            var recipient = await _unitOfWork.UserRepository.GetUser(createMessageDto.RecipientId);

            if (recipient == null) throw new HubException("Not found user");

            var message = new Message
            {


                SenderId = user.Id,
                RecipientId = recipient.Id,
                Text = createMessageDto.Text

            };

            var groupName = GetGroupName(user.Id.ToString(), recipient.Id.ToString());

            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.UserId == recipient.Id))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = await _tracker.GetConnectionsForUser(3);
                if (connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                        new
                        {
                            UserId = user.Id,
                            KnownAs = $"{user.FirstName} {user.LastName}",
                            Text = createMessageDto.Text
                        });
                }
            }

            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.Complete())
            {
                await Clients.Group(groupName).SendAsync("NewMessage",
                 await _unitOfWork.
                 MessageRepository.
                 GetMessage(message.Id, 0));
            }
        }

        public async Task DeleteMessage(int MesssageId)
        {
            var user = await UserRepo.GetUserByUsername(Context.User.GetUsername());
            var message = await _unitOfWork.MessageRepository.GetMessage(MesssageId, user.Id);
            var mappedMessage = _mapper.Map<Message>(message);
            var RecipientId = mappedMessage.RecipientId;

            _unitOfWork.MessageRepository.DeleteMessage(mappedMessage);
            // if (user.Id  == createMessageDto.RecipientId)
            //     throw new HubException("You cannot send messages to yourself");



            if (RecipientId == 0) throw new HubException("Not found user");



            var groupName = GetGroupName(user.Id.ToString(), RecipientId.ToString());

            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);



            if (await _unitOfWork.Complete())
            {
                await Clients.Group(groupName).SendAsync("OnDeleteMessage", MesssageId);
            }
        }
       
        private async Task<Group> AddToGroup(string groupName)
        {
            var user = await UserRepo.GetUserByUsername(Context.User.GetUsername());
            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, user.Id);

            if (group == null)
            {
                group = new Group(groupName);
                _unitOfWork.MessageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            if (await _unitOfWork.Complete()) return group;

            throw new HubException("Failed to join group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _unitOfWork.MessageRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            _unitOfWork.MessageRepository.RemoveConnection(connection);
            if (await _unitOfWork.Complete()) return group;

            throw new HubException("Failed to remove from group");
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}