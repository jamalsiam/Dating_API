using System.Linq;
using System.Threading.Tasks;
using Api.Context;
using Api.Entities;
using Api.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Api.Helpers;
using AutoMapper.QueryableExtensions;

namespace Api.Repos
{
    public class MessageRepo : IMessageRepo
    {
        private readonly DBContext Context;
        private readonly IMapper Mapper;
        public MessageRepo(DBContext context, IMapper mapper)
        {
            this.Mapper = mapper;
            this.Context = context;

        }

        public void AddMessage(Message message)
        {
            Context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            Context.Messages.Remove(message);
        }

        public async Task<MessageReadDto> GetMessage(int id, int accountId)
        {
            var query = Context.Messages
                .OrderBy(m => m.CreatedAt)
                .Where(a => a.Id == id)
                .Select(m => new MessageReadDto
                {
                    Id = m.Id,
                    AccountId = accountId,
                    RecipientId = m.RecipientId,
                    RecipientFullname = $"{m.Recipient.FirstName} {m.Recipient.LastName}",
                    RecipientPhotoUrl = m.Recipient.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                    SenderId = m.SenderId,
                    SenderFullname = $"{m.Sender.FirstName} {m.Sender.LastName}",
                    SenderPhotoUrl = m.Sender.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                    Text = m.Text,
                    DateRead = m.DateRead,
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt,
                    SenderDeleted = m.SenderDeleted,
                    RecipientDeleted = m.RecipientDeleted,
                    Deleted = m.Deleted,
                    Reaction = m.Reaction

                });
            return await query.FirstOrDefaultAsync(m => m.Id == id);

        }

        public async Task<PagedListX<MessageReadDto>> GetMessages(UserParams userParams, int accountId, int userId)
        {

            var query = Context.Messages
                 .OrderBy(m => m.CreatedAt)
                 .Where(a =>
                 (a.SenderId == accountId && a.RecipientId == userId) ||
                 (a.SenderId == userId && a.RecipientId == accountId))
                 .Select(m => new MessageReadDto
                 {
                     Id = m.Id,
                     AccountId = accountId,
                     RecipientId = m.RecipientId,
                     RecipientFullname = $"{m.Recipient.FirstName} {m.Recipient.LastName}",
                     RecipientPhotoUrl = m.Recipient.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                     SenderId = m.SenderId,
                     SenderFullname = $"{m.Sender.FirstName} {m.Sender.LastName}",
                     SenderPhotoUrl = m.Sender.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                     Text = m.Text,
                     DateRead = m.DateRead,
                     CreatedAt = m.CreatedAt,
                     UpdatedAt = m.UpdatedAt,
                     SenderDeleted = m.SenderDeleted,
                     RecipientDeleted = m.RecipientDeleted,
                     Deleted = m.Deleted,
                     Reaction = m.Reaction

                 }).OrderByDescending(i=>i.Id);

            return await PagedListX<MessageReadDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<UserChatDto> GetUserInfo(int id)
        {
            return await Context
            .Users
            .AsQueryable()
            .Where(usr => usr.Id == id)
            .Select(usr => new UserChatDto
            {
                UserId = usr.Id,
                AccountId = 0,
                Fullname = $"{usr.UserName } {usr.LastName}",
                PhotoUrl = usr.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                TextMessage = "",
                Seen = false,
                MessageCreateAt = DateTime.Now,
                Active = false,
            })
            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserChatDto>> GetUsersList(int accountId)
        {
            //.SenderId
            var messages = Context.Messages
                .Where(m => m.SenderId == accountId || m.RecipientId == accountId)
                .OrderByDescending(m => m.CreatedAt)
                .GroupBy(m => new {m.SenderId, m.RecipientId, m.Reaction})                
                .Select(group => new UserChatDto(){
                            UserId = group.Key.SenderId == accountId ? group.First().RecipientId : group.First().SenderId,
                            AccountId = accountId,
                            Fullname = group.First().SenderId == accountId ? $"{group.First().Recipient.UserName } {group.First().Recipient.LastName}" : $"{group.First().Sender.UserName } {group.First().Sender.LastName}",
                            PhotoUrl = group.First().SenderId == accountId ? group.First().Recipient.Photos.FirstOrDefault(ph => ph.IsMain).Url
                            : group.First().Sender.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                            TextMessage = group.First().Text,
                            Seen = false,
                            MessageCreateAt = group.First().UpdatedAt,
                            Active = false,
                          })
                ;

            // var messages = from m in Context.Messages
            // where  m.SenderId == accountId || m.RecipientId == accountId
            // group m by m.SenderId == accountId ? m.RecipientId : m.SenderId into g
            // select g.OrderByDescending(m => m.CreatedAt)
            //         .Select(m => new UserChatDto(){
            //                 UserId = m.SenderId == accountId ? m.RecipientId : m.SenderId,
            //                 AccountId = accountId,
            //                 Fullname = m.SenderId == accountId ? $"{m.Recipient.UserName } {m.Recipient.LastName}" : $"{m.Sender.UserName } {m.Sender.LastName}",
            //                 PhotoUrl = m.SenderId == accountId ? m.Recipient.Photos.FirstOrDefault(ph => ph.IsMain).Url
            //                 : m.Sender.Photos.FirstOrDefault(ph => ph.IsMain).Url,
            //                 TextMessage = m.Text,
            //                 Seen = false,
            //                 MessageCreateAt = m.UpdatedAt,
            //                 Active = false,
            //               }).First();


            return  messages;
            //return  null;
        }

        public async Task<bool> SaveChanges()
        {
            return await Context.SaveChangesAsync() > 0;
        }
    }
}